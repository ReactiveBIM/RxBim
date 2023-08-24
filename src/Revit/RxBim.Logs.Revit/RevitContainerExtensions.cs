namespace RxBim.Logs.Revit
{
    using System;
    using System.Reflection;
    using Autodesk.Revit.UI;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    /// <summary>
    /// DI container extensions.
    /// </summary>
    public static class RevitContainerExtensions
    {
        /// <summary>
        /// Adds logs into a DI container.
        /// </summary>
        /// <param name="services">The DI container.</param>
        /// <param name="pluginAssembly">The plugin assembly.</param>
        /// <param name="cfg">The configuration.</param>
        public static void AddRevitLogs(
            this IContainer services,
            Assembly? pluginAssembly = null,
            IConfiguration? cfg = null)
        {
            pluginAssembly ??= Assembly.GetCallingAssembly();
            services.AddLogs(cfg,
                (provider, configuration) => EnrichWithRevitData(provider, configuration, pluginAssembly));
        }

        private static void EnrichWithRevitData(
            IServiceProvider provider,
            LoggerConfiguration config,
            Assembly pluginAssembly)
        {
            try
            {
                var uiApp = provider.GetRequiredService<UIApplication>();
                config.Enrich.With(new RevitEnricher(uiApp, pluginAssembly));
            }
            catch
            {
                // ignore
            }
        }
    }
}