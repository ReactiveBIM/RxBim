namespace RxBim.Logs.Autocad
{
    using System;
    using System.Reflection;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    /// <summary>
    /// The DI container extensions.
    /// </summary>
    public static class AutocadContainerExtensions
    {
        /// <summary>
        /// Adds logger into a container.
        /// </summary>
        /// <param name="services">The DI container.</param>
        /// <param name="pluginAssembly">The plugin assembly.</param>
        /// <param name="cfg">The configuration.</param>
        /// <param name="useDefaultEnrichers">Indicates, that default enrichers will be used.</param>
        /// <param name="additionalConfiguration">An action for additional logs configuration.</param>
        public static void AddAutocadLogs(
            this IServiceCollection services,
            Assembly? pluginAssembly = null,
            IConfiguration? cfg = null,
            bool useDefaultEnrichers = true,
            Action<IServiceProvider, LoggerConfiguration>? additionalConfiguration = null)
        {
            pluginAssembly ??= Assembly.GetCallingAssembly();
            services.AddLogs(cfg, useDefaultEnrichers, (serviceProvider, configuration) =>
            {
                EnrichWithAutocadData(configuration, pluginAssembly);
                additionalConfiguration?.Invoke(serviceProvider, configuration);
            });
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