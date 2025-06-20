namespace RxBim.Di
{
    using System.IO;
    using System.Xml;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Json;
    using Microsoft.Win32;

    /// <summary>
    /// Provider configuration JSON file by Environment.
    /// </summary>
    internal class EnvironmentConfigurationSource : JsonConfigurationSource
    {
        private const string PackageContentsFileName = "PackageContents.xml";
        private const int MaxRecursionLevel = 3;

        private readonly string _basePath;
        private readonly string _configFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentConfigurationSource"/> class.
        /// </summary>
        /// <param name="basePath">Base location.</param>
        /// <param name="configFile">Configuration JSON file.</param>
        public EnvironmentConfigurationSource(
            string basePath,
            string configFile)
        {
            _basePath = basePath;
            _configFile = configFile;
        }

        /// <inheritdoc/>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var environment = GetEnvironment(_basePath);
            Path = $"{System.IO.Path.GetFileNameWithoutExtension(_configFile)}.{environment}.json";
            Optional = true;

            return base.Build(builder);
        }

        private string GetEnvironment(string basePath, int recursionLevel = 0)
        {
            if (recursionLevel == MaxRecursionLevel)
                return EnvironmentRegistryConstants.DefaultEnvironment;

            var packageContentsFileDir = Directory.GetParent(basePath);
            var environment = EnvironmentRegistryConstants.DefaultEnvironment;

            if (packageContentsFileDir is null)
                return EnvironmentRegistryConstants.DefaultEnvironment;

            var packageContentsFile = System.IO.Path.Combine(
                packageContentsFileDir.FullName,
                PackageContentsFileName);
            if (!File.Exists(packageContentsFile))
            {
                recursionLevel++;
                return GetEnvironment(packageContentsFileDir.FullName, recursionLevel);
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(packageContentsFile);
            var productCode = xmlDoc.DocumentElement
                ?.SelectSingleNode("//ApplicationPackage")
                ?.Attributes?
                .GetNamedItem("ProductCode")
                .Value;

            if (!string.IsNullOrWhiteSpace(productCode))
            {
                environment = Registry.CurrentUser
                    .OpenSubKey($"{EnvironmentRegistryConstants.RxBimEnvironmentRegPath}\\{productCode}")
                    ?.GetValue(EnvironmentRegistryConstants.EnvironmentRegKeyName)
                    ?.ToString() ?? EnvironmentRegistryConstants.DefaultEnvironment;
            }

            return environment;
        }
    }
}