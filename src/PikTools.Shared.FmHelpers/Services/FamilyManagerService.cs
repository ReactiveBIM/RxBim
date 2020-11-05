namespace PikTools.Shared.FmHelpers.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Abstractions;
    using Autodesk.Revit.DB;
    using Bimlab.Security.Client;
    using CSharpFunctionalExtensions;
    using FamilyManager.Shared.Enums;
    using FamilyManager.V2.Dto.Enums;
    using FamilyManager.V2.SDK;
    using Models;

    /// <summary>
    /// Сервис работы с FM
    /// </summary>
    public class FamilyManagerService : IFamilyManagerService
    {
        private readonly TokenManager _tokenManager;
        private readonly FmSettings _settings;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="tokenManager">Управление токеном</param>
        /// <param name="settings">Конфигурация</param>
        public FamilyManagerService(
            TokenManager tokenManager, FmSettings settings)
        {
            _tokenManager = tokenManager;
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
                try
                {
                    trans.Start();
                    action?.Invoke();
                    trans.Commit();
                }
                catch
                {
                    trans.RollBack();
                    throw;
                }
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
                var loginResult = await _tokenManager.Login();

                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_settings.FmEndPoint)
                };
                httpClient.DefaultRequestHeaders.Authorization =
                       new AuthenticationHeaderValue(loginResult.token_type, loginResult.access_token);

                var client = new FamilyManagerClient(
                    httpClient, AppType.Revit, new Version(_settings.ClientVersion));

                var quickSearch = await client.Families.Search(name);
                var family = quickSearch.FirstOrDefault(f => f.Name == name);
                if (family == null)
                    return Result.Failure<string>($"Семейство {name} не найдено");

                var fileType = family.IsSystem ? FileTypes.Rvt : FileTypes.Rfa;
                var bytes = await client.FamilyVersions.GetFamilyFile(
                    family.LastVersionId, fileType);

                var tempPath = Path.Combine(Path.GetTempPath(), "FamilyManager");
                Directory.CreateDirectory(tempPath);
                var tempFile = Path.Combine(tempPath, family.Name);
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
