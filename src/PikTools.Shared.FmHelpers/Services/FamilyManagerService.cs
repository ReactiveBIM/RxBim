namespace PikTools.Shared.FmHelpers.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Autodesk.Revit.DB;
    using FamilyManager.Shared.Enums;
    using FamilyManager.V2.Dto.Enums;
    using FamilyManager.V2.Dto.Filter;
    using FamilyManager.V2.SDK;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using PikTools.Shared.FmHelpers.Abstractions;
    using PikTools.Shared.FmHelpers.Models;

    /// <summary>
    /// Сервис работы с FM
    /// </summary>
    public class FamilyManagerService : IFamilyManagerService
    {
        private const string CachePath = @"\PIK\Auth\cache.json";
        private const string FamilyManagerVersion = "2020.7.4";

        private readonly IConfiguration _config;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="config">Конфигурация</param>
        public FamilyManagerService(IConfiguration config)
        {
            _config = config;
        }

        /// <inheritdoc/>
        public FamilySymbol GetTargetFamilySymbol(Document doc, string familyName)
        {
            var fileResult = Task.Run(() => GetFamilyFromFamilyManager(familyName)).Result;
            if (string.IsNullOrWhiteSpace(fileResult))
                return null;

            Family family = null;
            using (var trans = new Transaction(doc, $"FM - Загрузка семейства {familyName}"))
            {
                trans.Start();

                doc.LoadFamily(fileResult, out family);

                trans.Commit();
            }

            var loadedFamilySymbolId = family.GetFamilySymbolIds().FirstOrDefault();
            if (loadedFamilySymbolId == null)
                return null;

            return doc.GetElement(loadedFamilySymbolId) as FamilySymbol;
        }

        private async Task<string> GetFamilyFromFamilyManager(string name)
        {
            try
            {
                var localAppPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var readText = File.ReadAllText(localAppPath + CachePath);
                var loginResult = JsonConvert.DeserializeObject<TokenModel>(readText);

                var familyManagerConnectionString = _config.GetSection("FmEndPoint").Value;
                var httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(familyManagerConnectionString)
                };
                httpClient.DefaultRequestHeaders.Authorization =
                       new AuthenticationHeaderValue(loginResult.TokenType, loginResult.AccessToken);

                var client = new FamilyManagerClient(httpClient, AppType.Revit, new Version(FamilyManagerVersion));

                var quickSearch = await client.Families.QuickSearch(new QuickSearchFilter() { FamilyName = name, AppType = AppType.Revit });
                var family = quickSearch.FirstOrDefault();
                if (family == null)
                    return string.Empty;

                var bytes = await client.FamilyVersions.GetFamilyFile(family.CurrentVersion.VersionId, FileTypes.Rvt);

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
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
