namespace RxBim.Di
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Provides ability to read configuration parameters from JSON by environment variable.
    /// </summary>
    public static class EnvironmentConfigurationExtensions
    {
        /// <summary>
        /// Read configuration from JSON by environment variable.
        /// </summary>
        /// <param name="configurationBuilder"><see cref="IConfigurationBuilder"/>.</param>
        /// <param name="basePath">Base location.</param>
        /// <param name="configFile">Configuration JSON file.</param>
        public static IConfigurationBuilder AddEnvironmentJsonFile(
            this IConfigurationBuilder configurationBuilder,
            string basePath,
            string configFile)
        {
            return configurationBuilder.Add(new EnvironmentConfigurationSource(
                basePath,
                configFile));
        }
    }
}