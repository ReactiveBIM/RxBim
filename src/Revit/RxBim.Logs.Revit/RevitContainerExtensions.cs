namespace RxBim.Logs.Revit
{
    using System;
    using System.Reflection;
    using Autodesk.Revit.UI;
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
            this IServiceCollection services,
            Assembly? pluginAssembly = null,
            IConfiguration? cfg = null)
        {
            pluginAssembly ??= Assembly.GetCallingAssembly();
            services.AddLogs(cfg,
                (serviceProvider, configuration) => EnrichWithRevitData(serviceProvider, configuration, pluginAssembly));
        }

        private static void EnrichWithRevitData(
            IServiceProvider serviceProvider,
            LoggerConfiguration config,
            Assembly pluginAssembly)
        {
            try
            {
                var uiApp = serviceProvider.GetService<UIApplication>();
                config.Enrich.With(new RevitEnricher(uiApp, pluginAssembly));
            }
            catch
            {
                // ignore
            }
        }
    }
}