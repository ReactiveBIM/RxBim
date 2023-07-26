namespace RxBim.Logs.Autocad
{
    using System.Reflection;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    /// <summary>
    /// The DI container extensions. 
    /// </summary>
    public static class AutocadContainerExtensions
    {
        /// <summary>
        /// Adds logger into a container.
        /// </summary>
        /// <param name="container">The DI container.</param>
        /// <param name="pluginAssembly">The plugin assembly.</param>
        /// <param name="cfg">The configuration.</param>
        public static void AddLogs(
            this IContainer container,
            Assembly? pluginAssembly = null,
            IConfiguration? cfg = null)
        {
            pluginAssembly ??= Assembly.GetCallingAssembly();
            container.AddLogs(cfg, (_, configuration) => EnrichWithAutocadData(configuration, pluginAssembly));
        }

        private static void EnrichWithAutocadData(LoggerConfiguration config, Assembly assembly)
        {
            try
            {
                config.Enrich.With(new AutocadEnricher(assembly));
            }
            catch
            {
                // ignore
            }
        }
    }
}