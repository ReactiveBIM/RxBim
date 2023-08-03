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
            var packageContentsFileDir = Directory.GetParent(_basePath);
            var environment = EnvironmentRegistryConstants.DefaultEnvironment;

            if (packageContentsFileDir is not null)
            {
                var packageContentsFile = System.IO.Path.Combine(
                    packageContentsFileDir.FullName,
                    PackageContentsFileName);

                if (File.Exists(packageContentsFile))
                {
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
                }
            }

            Path = $"{System.IO.Path.GetFileNameWithoutExtension(_configFile)}.{environment}.json";
            Optional = true;

            return base.Build(builder);
        }
    }
}