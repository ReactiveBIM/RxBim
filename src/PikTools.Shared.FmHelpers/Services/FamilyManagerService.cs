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
            if (string.IsNullOrWhiteSpace(symbolName))
                return Result.Failure<FamilySymbol>("Имя типоразмера должно быть задано");

            return GetFamilySymbolsByFilter(doc,
                    new FmSearchFilter
                        { Name = familyName, SymbolName = symbolName, Limit = 1 })
                .Map(x => x.First());
        }

        /// <inheritdoc/>
        public Result<Family> GetTargetFamily(
            Document doc,
            string familyName,
            bool useTransaction = true,
            IFamilyLoadOptions familyLoadOptions = null)
        {
            if (string.IsNullOrWhiteSpace(familyName))
                return Result.Failure<Family>("Имя семейства должно быть задано");

            return GetFamiliesByFilter(doc,
                    new FmSearchFilter { Name = familyName, Limit = 1 },
                    useTransaction,
                    familyLoadOptions)
                .Map(x => x.First());
        }

        /// <inheritdoc />
        public Result<List<Family>> GetFamiliesByFilter(
            Document doc,
            FmSearchFilter filter,
            bool useTransaction = true,
            IFamilyLoadOptions familyLoadOptions = null)
        {
            return Task.Run(() => SearchFamilies(doc, filter)).Result
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
                        $"Загрузка семейств. Метод: {nameof(GetFamiliesByFilter)}",
                        useTransaction);
                    return families;
                });
        }

        /// <inheritdoc />
        public Result<List<FamilySymbol>> GetFamilySymbolsByFilter(
            Document doc,
            FmSearchFilter filter,
            bool useTransaction = true,
            IFamilyLoadOptions familyLoadOptions = null)
        {
            if (string.IsNullOrWhiteSpace(filter?.SymbolName))
                return Result.Failure<List<FamilySymbol>>("Имя типоразмера должно быть задано");

            return Task.Run(() => SearchFamilies(doc, filter)).Result
                .Map(fileResult =>
                {
                    Action action;
                    var result = new List<FamilySymbol>();

                    if (familyLoadOptions == null)
                    {
                        action = () => fileResult.ForEach(f =>
                        {
                            doc.LoadFamilySymbol(f, filter.SymbolName, out var symbol);
                            result.Add(symbol);
                        });
                    }
                    else
                    {
                        action = () => fileResult.ForEach(f =>
                        {
                            doc.LoadFamilySymbol(f, filter.SymbolName, familyLoadOptions, out var symbol);
                            result.Add(symbol);
                        });
                    }

                    TransactionMethod(
                        doc,
                        action,
                        $"FM - Загрузка типоразмера {filter.SymbolName} Метод: {nameof(GetFamilySymbolsByFilter)}",
                        useTransaction);
                    return result;
                });
        }

        private async Task<Result<List<string>>> SearchFamilies(Document doc, FmSearchFilter searchFilter)
        {
            searchFilter ??= new FmSearchFilter();

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
                    FamilySymbolName = searchFilter.SymbolName,
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
