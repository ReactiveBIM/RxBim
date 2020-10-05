namespace PikTools.Shared.FmHelpers.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Autodesk.Revit.DB;
    using CSharpFunctionalExtensions;
    using FamilyManager.Shared.Enums;
    using FamilyManager.V2.Dto.Enums;
    using FamilyManager.V2.Dto.Filter;
    using FamilyManager.V2.SDK;
    using Newtonsoft.Json;
    using PikTools.Shared.FmHelpers.Abstractions;
    using PikTools.Shared.FmHelpers.Models;

    /// <summary>
    /// Сервис работы с FM
    /// </summary>
    public class FamilyManagerService : IFamilyManagerService
    {
        private readonly FmSettings _settings;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="settings">Конфигурация</param>
        public FamilyManagerService(FmSettings settings)
        {
            _settings = settings;
        }

        /// <inheritdoc/>
        public Result<FamilySymbol> GetTargetFamilySymbol(
            Document doc, string familyName, string symbolName, bool useTransaction = true)
        {
            return Task.Run(() => GetFamilyFromFamilyManager(familyName)).Result
                .Map(fileResult =>
                {
                    FamilySymbol familySymbol = null;
                    TransactionMethod(
                            doc,
                            () => doc.LoadFamilySymbol(fileResult, symbolName, out familySymbol),
                            $"FM - Загрузка семейства {familyName}",
                            useTransaction);
                    return familySymbol;
                });
        }

        /// <inheritdoc/>
        public Result<Family> GetTargetFamily(Document doc, string familyName, bool useTransaction = true)
        {
            return Task.Run(() => GetFamilyFromFamilyManager(familyName)).Result
                .Map(fileResult =>
                {
                    Family family = null;
                    TransactionMethod(
                            doc,
                            () => doc.LoadFamily(fileResult, out family),
                            $"FM - Загрузка семейства {familyName}",
                            useTransaction);
                    return family;
                });
        }

        private void TransactionMethod(Document doc, Action action, string transTitle, bool useTransaction)
        {
            if (useTransaction)
            {
                using var trans = new Transaction(doc, transTitle);
                trans.Start();
                action?.Invoke();
                trans.Commit();
            }
            else
            {
                action?.Invoke();
            }
        }

        private async Task<Result<string>> GetFamilyFromFamilyManager(string name)
        {
            try
            {
                var localAppPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var readText = File.ReadAllText(localAppPath + _settings.AuthCachePath);
                var loginResult = JsonConvert.DeserializeObject<TokenModel>(readText);

                var httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(_settings.FmEndPoint)
                };
                httpClient.DefaultRequestHeaders.Authorization =
                       new AuthenticationHeaderValue(loginResult.TokenType, loginResult.AccessToken);

                var client = new FamilyManagerClient(
                    httpClient, AppType.Revit, new Version(_settings.ClientVersion));

                var quickSearch = await client.Families.QuickSearch(
                    new QuickSearchFilter() { FamilyName = name, AppType = AppType.Revit });
                var family = quickSearch.FirstOrDefault();
                if (family == null)
                    return Result.Failure<string>($"Семейство {name} не найдено");

                var bytes = await client.FamilyVersions.GetFamilyFile(
                    family.CurrentVersion.VersionId, FileTypes.Rvt);

                var tempPath = Path.Combine(Path.GetTempPath(), "FamilyManager");
                Directory.CreateDirectory(tempPath);
                var tempFile = Path.Combine(tempPath, family.Name);
                var fileType = family.IsSystem
                    ? FileTypes.Rvt
                    : FileTypes.Rfa;
                tempFile += $".{fileType.ToString().ToLower()}";
                File.WriteAllBytes(tempFile, bytes);

                return tempFile;
            }
            catch (Exception exception)
            {
                return Result.Failure<string>(exception.Message);
            }
        }
    }
}
