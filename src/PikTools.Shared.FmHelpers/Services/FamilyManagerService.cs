namespace PikTools.Shared.FmHelpers.Services
{
    using System;
    using System.Collections.Generic;
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
    using FamilyManager.V2.Dto.Filter;
    using FamilyManager.V2.Dto.Tree;
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
            Document doc,
            string familyName,
            string symbolName,
            bool useTransaction = true,
            IFamilyLoadOptions familyLoadOptions = null)
        {
            return Task.Run(() => SearchInternal(doc, new FmSearchFilter { Name = familyName, Limit = 1 })).Result
                .Map(fileResult =>
                {
                    FamilySymbol familySymbol = null;
                    var action = familyLoadOptions == null
                        ? (Action)(() => doc.LoadFamilySymbol(fileResult[0], symbolName, out familySymbol))
                        : () => doc.LoadFamilySymbol(familyName, symbolName, familyLoadOptions, out familySymbol);

                    TransactionMethod(
                            doc,
                            action,
                            $"FM - Загрузка семейства {familyName}",
                            useTransaction);
                    return familySymbol;
                });
        }

        /// <inheritdoc/>
        public Result<Family> GetTargetFamily(
            Document doc,
            string familyName,
            bool useTransaction = true,
            IFamilyLoadOptions familyLoadOptions = null)
        {
            return Search(doc, new FmSearchFilter { Name = familyName, Limit = 1 }, familyLoadOptions)
                .Map(x => x.FirstOrDefault());
        }

        /// <inheritdoc />
        public Result<List<Family>> Search(
            Document doc,
            FmSearchFilter filter,
            IFamilyLoadOptions familyLoadOptions = null)
        {
            return Task.Run(() => SearchInternal(doc, filter)).Result
                .Map(result =>
                {
                    var families = new List<Family>();
                    Action action;
                    if (familyLoadOptions == null)
                    {
                        action = () => result.ForEach(r =>
                        {
                            doc.LoadFamily(r, out var family);
                            families.Add(family);
                        });
                    }
                    else
                    {
                        action = () => result.ForEach(r =>
                        {
                            doc.LoadFamily(r, familyLoadOptions, out var family);
                            families.Add(family);
                        });
                    }

                    TransactionMethod(
                        doc,
                        action,
                        $"Загрузка семейств. Метод: {nameof(Search)}",
                        true);
                    return families;
                });
        }

        private async Task<Result<List<string>>> SearchInternal(Document doc, FmSearchFilter searchFilter)
        {
            try
            {
                var client = await CreateFmClient();
                var filter = new FamilySearchFilter
                {
                    AppType = AppType.Revit,
                    Limit = searchFilter.Limit,
                    Name = searchFilter.Name,
                    FunctionalTypeName = searchFilter.FunctionalTypeName,
                    CategoryName = searchFilter.CategoryName,
                    FilePath = GetProjectPath(doc)
                };

                var families = await client.TreeItems.Search(filter);
                if (families.Count == 0)
                    return Result.Failure<List<string>>("Семейства не найдены");

                var result = new List<string>();

                foreach (var familyDto in families.Result)
                {
                    var family = await WriteToDisk(familyDto, client);

                    if (family.IsFailure)
                        return Result.Failure<List<string>>(family.Error);

                    result.Add(family.Value);
                }

                return result;
            }
            catch (Exception exception)
            {
                return Result.Failure<List<string>>(exception.Message);
            }
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

        private async Task<Result<string>> WriteToDisk(
            FamilySearchDto family,
            FamilyManagerClient client)
        {
            var versionId = family.Versions
                .OrderBy(v => v.VersionNumber)
                .LastOrDefault(v => v.Status == FamilyVersionStatusEnum.Allowed)?.Id;

            if (versionId == null)
                return Result.Failure<string>($"Семейство {family.Name} не имеет разрешенных версий");

            var fileType = family.IsSystem ? FileTypes.Rvt : FileTypes.Rfa;
            var bytes = await client.FamilyVersions.GetFamilyFile(
                versionId.Value, fileType);

            var tempPath = Path.Combine(Path.GetTempPath(), "FamilyManager");
            Directory.CreateDirectory(tempPath);
            var tempFile = Path.Combine(tempPath, family.Name);
            tempFile += $".{fileType.ToString().ToLower()}";
            File.WriteAllBytes(tempFile, bytes);
            return tempFile;
        }

        private async Task<FamilyManagerClient> CreateFmClient()
        {
            var loginResult = await _tokenManager.Login();

            var httpClient = new HttpClient { BaseAddress = new Uri(_settings.FmEndPoint) };

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(loginResult.token_type, loginResult.access_token);

            return new FamilyManagerClient(httpClient, AppType.Revit, new Version(_settings.ClientVersion));
        }

        private string GetProjectPath(Document doc)
        {
            ModelPath modelPath = null;

            if (doc.IsWorkshared)
                modelPath = doc.GetWorksharingCentralModelPath();

            if (modelPath == null
                || modelPath.Empty)
                return doc.PathName;

            return ModelPathUtils.ConvertModelPathToUserVisiblePath(modelPath);
        }
    }
}
