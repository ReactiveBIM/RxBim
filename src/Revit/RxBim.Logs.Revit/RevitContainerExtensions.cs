namespace RxBim.Logs.Revit
{
    using System.Reflection;
    using Autodesk.Revit.UI;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    /// <summary>
    /// DI container extensions.
    /// </summary>
    public static class RevitContainerExtensions
    {
        /// <summary>
        /// Adds logs into a DI container.
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
            container.AddLogs(cfg,
                (container1, configuration) => EnrichWithRevitData(container1, configuration, pluginAssembly));
        }

        private static void EnrichWithRevitData(
            IContainer container,
            LoggerConfiguration config,
            Assembly pluginAssembly)
        {
            try
            {
                var uiApp = container.GetService<UIApplication>();
                config.Enrich.With(new RevitEnricher(uiApp, pluginAssembly));
            }
            catch
            {
                // ignore
            }
        }
    }
}