namespace RxBim.Logs
{
    using System;
    using Di;
    using Enrichers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyModel;
    using Serilog;
    using Serilog.Events;

    /// <summary>
    /// A DI container extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds logs in a <paramref name="services"/>.
        /// </summary>
        /// <param name="services">A DI container.</param>
        /// <param name="cfg">A configuration.</param>
        /// <param name="addEnricher">An action for additional logs configuration.</param>
        public static void AddLogs(
            this IServiceCollection services,
            IConfiguration? cfg = null,
            Action<IServiceProvider, LoggerConfiguration>? addEnricher = null)
        {
            RegisterLogger(services, cfg, addEnricher);
            services.Decorate(typeof(IMethodCaller<>), typeof(LoggedMethodCaller<>));
        }

        private static void RegisterLogger(
            IServiceCollection services,
            IConfiguration? cfg,
            Action<IServiceProvider, LoggerConfiguration>? addEnricher)
        {
            services.AddSingleton(
                provider =>
                {
                    if (cfg == null)
                    {
                        TryGetConfigurationFromContainer(provider, ref cfg);
                    }

                    return CreateLogger(cfg, provider, addEnricher);
                });
        }

        private static void TryGetConfigurationFromContainer(IServiceProvider provider, ref IConfiguration? cfg)
        {
            try
            {
                cfg = provider.GetService<IConfiguration>();
            }
            catch
            {
                // ignore
            }
        }

        private static ILogger CreateLogger(
            IConfiguration? cfg,
            IServiceProvider provider,
            Action<IServiceProvider, LoggerConfiguration>? addEnricher)
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

            AddLogEnrichers(provider, config, addEnricher);

            return config.CreateLogger();
        }

        private static void AddLogEnrichers(
            IServiceProvider provider,
            LoggerConfiguration config,
            Action<IServiceProvider, LoggerConfiguration>? addEnricher)
        {
            config
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.With<ExceptionEnricher>()
                .Enrich.With<OsEnricher>();

            addEnricher?.Invoke(provider, config);
        }
    }
}