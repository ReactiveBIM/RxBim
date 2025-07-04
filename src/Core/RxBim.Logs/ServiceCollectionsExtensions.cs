namespace RxBim.Logs
{
    using System;
    using Di;
    using Enrichers;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyModel;
    using Serilog;
    using Serilog.Events;

    /// <summary>
    /// A DI container extensions.
    /// </summary>
    [PublicAPI]
    public static class ServiceCollectionsExtensions
    {
        /// <summary>
        /// Adds logs in a <paramref name="services"/>.
        /// </summary>
        /// <param name="services">A DI container.</param>
        /// <param name="cfg">A configuration.</param>
        /// <param name="useDefaultEnrichers">Indicates, that default enrichers will be used.</param>
        /// <param name="addEnricher">An action for additional logs configuration.</param>
        public static void AddLogs(
            this IServiceCollection services,
            IConfiguration? cfg = null,
            bool useDefaultEnrichers = true,
            Action<IServiceProvider, LoggerConfiguration>? addEnricher = null)
        {
            RegisterLogger(services, cfg, useDefaultEnrichers, addEnricher);
            services.Decorate(typeof(IMethodCaller<>), typeof(LoggedMethodCaller<>));
        }

        private static void RegisterLogger(
            IServiceCollection services,
            IConfiguration? cfg,
            bool useDefaultEnrichers = true,
            Action<IServiceProvider, LoggerConfiguration>? addEnricher = null)
        {
            services.AddSingleton(
                sp =>
                {
                    if (cfg == null)
                    {
                        TryGetConfigurationFromContainer(sp, ref cfg);
                    }

                    return CreateLogger(cfg, sp, useDefaultEnrichers, addEnricher);
                });
        }

        private static void TryGetConfigurationFromContainer(IServiceProvider serviceProvider, ref IConfiguration? cfg)
        {
            try
            {
                cfg = serviceProvider.GetService<IConfiguration>();
            }
            catch
            {
                // ignore
            }
        }

        private static ILogger CreateLogger(
            IConfiguration? cfg,
            IServiceProvider serviceProvider,
            bool useDefaultEnrichers = true,
            Action<IServiceProvider, LoggerConfiguration>? addEnricher = null)
        {
            var config = new LoggerConfiguration();
            if (cfg != null)
            {
                var dependencyContext = DependencyContext.Load(typeof(LoggedMethodCaller<>).Assembly);
                config.ReadFrom.Configuration(cfg, dependencyContext);
            }
            else
            {
                config
                    .WriteTo.Debug()
                    .WriteTo.File("log.txt", LogEventLevel.Information, fileSizeLimitBytes: 1024 * 10);
            }

            AddLogEnrichers(serviceProvider, config, useDefaultEnrichers, addEnricher);

            return config.CreateLogger();
        }

        private static void AddLogEnrichers(
            IServiceProvider serviceProvider,
            LoggerConfiguration config,
            bool useDefaultEnrichers = true,
            Action<IServiceProvider, LoggerConfiguration>? addEnricher = null)
        {
            config.Enrich.FromLogContext();
            if (useDefaultEnrichers)
            {
                config.Enrich.WithMachineName()
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.With<ExceptionEnricher>()
                    .Enrich.With<OsEnricher>();
            }

            addEnricher?.Invoke(serviceProvider, config);
        }
    }
}