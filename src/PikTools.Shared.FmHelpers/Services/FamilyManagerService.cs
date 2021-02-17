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
            return Task.Run(() => GetFamilyFromFamilyManager(doc, familyName)).Result
                .Map(fileResult =>
                {
                    FamilySymbol familySymbol = null;
                    var action = familyLoadOptions == null
                        ? (Action)(() => doc.LoadFamilySymbol(fileResult, symbolName, out familySymbol))
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
            return Task.Run(() => GetFamilyFromFamilyManager(doc, familyName)).Result
                .Map(fileResult =>
                {
                    Family family = null;
                    Action action;
                    if (familyLoadOptions == null)
                        action = () => doc.LoadFamily(fileResult, out family);
                    else
                        action = () => doc.LoadFamily(fileResult, familyLoadOptions, out family);

                    TransactionMethod(
                            doc,
                            action,
                            $"FM - Загрузка семейства {familyName}",
                            useTransaction);
                    return family;
                });
        }

        /// <inheritdoc />
        public Result<List<Family>> GetFamiliesByFunctionalType(
            Document doc,
            string ftName,
            IFamilyLoadOptions familyLoadOptions = null)
        {
            return Task.Run(() => GetFamiliesByFt(doc, ftName)).Result
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
                        $"Загрузка семейств функционального типа {ftName}",
                        true);
                    return families;
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

        private async Task<Result<string>> GetFamilyFromFamilyManager(Document doc, string name)
        {
            try
            {
                var client = await CreateFmClient();

                var searchFilter = new FamilySearchFilter
                {
                    AppType = AppType.Revit,
                    Name = name,
                    FilePath = GetProjectPath(doc)
                };

                var familySearch = await client.TreeItems.Search(searchFilter);
                var family = familySearch.Result.FirstOrDefault(f => f.Name.Equals(name));
                if (family == null)
                    return Result.Failure<string>($"Семейство {name} не найдено");

                return await GetFamilyVersion(family)
                    .Bind(v => WriteToDisk(family, v.Id, client));
            }
            catch (Exception exception)
            {
                return Result.Failure<string>(exception.Message);
            }
        }

        private Result<TreeItemFamilyVersionDto> GetFamilyVersion(FamilySearchDto family)
        {
            return family.Versions
                .OrderBy(v => v.VersionNumber)
                .LastOrDefault(v => v.Status == FamilyVersionStatusEnum.Allowed)
                   ?? Result.Failure<TreeItemFamilyVersionDto>($"Не найдена разрешенная версия семейства {family.Name}");
        }

        private async Task<Result<string>> WriteToDisk(
            FamilySearchDto family,
            long familyVersionId,
            FamilyManagerClient client)
        {
            var fileType = family.IsSystem ? FileTypes.Rvt : FileTypes.Rfa;
            var bytes = await client.FamilyVersions.GetFamilyFile(
                familyVersionId, fileType);

            var tempPath = Path.Combine(Path.GetTempPath(), "FamilyManager");
            Directory.CreateDirectory(tempPath);
            var tempFile = Path.Combine(tempPath, family.Name);
            tempFile += $".{fileType.ToString().ToLower()}";
            File.WriteAllBytes(tempFile, bytes);
            return tempFile;
        }

        private async Task<Result<List<string>>> GetFamiliesByFt(Document doc, string ftName)
        {
            try
            {
                var client = await CreateFmClient();
                var filter = new FamilySearchFilter
                {
                    AppType = AppType.Revit,
                    FunctionalTypeName = ftName,
                    FilePath = GetProjectPath(doc)
                };

                var families = await client.TreeItems.Search(filter);
                if (families.Count == 0)
                    return Result.Failure<List<string>>($"Семейства с ФТ {ftName} не найдены");

                var result = new List<string>();

                foreach (var family in families.Result)
                {
                    var current = await GetFamilyVersion(family)
                        .Bind(v => WriteToDisk(family, v.Id, client));

                    if (current.IsFailure)
                        return Result.Failure<List<string>>(current.Error);

                    result.Add(current.Value);
                }

                return result;
            }
            catch (Exception exception)
            {
                return Result.Failure<List<string>>(exception.Message);
            }
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
