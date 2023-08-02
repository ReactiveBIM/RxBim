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
            Action<IServiceCollection, LoggerConfiguration>? addEnricher = null)
        {
            RegisterLogger(services, cfg, addEnricher);
            services.Decorate(typeof(IMethodCaller<>), typeof(LoggedMethodCaller<>));
        }

        private static void RegisterLogger(
            IServiceCollection container,
            IConfiguration? cfg,
            Action<IServiceCollection, LoggerConfiguration>? addEnricher)
        {
            container.AddSingleton(
                provider =>
                {
                    if (cfg == null)
                    {
                        TryGetConfigurationFromContainer(provider, ref cfg);
                    }

                    return CreateLogger(cfg, container, addEnricher);
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
            IServiceCollection container,
            Action<IServiceCollection, LoggerConfiguration>? addEnricher)
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

            AddLogEnrichers(container, config, addEnricher);

            return config.CreateLogger();
        }

        private static void AddLogEnrichers(
            IServiceCollection container,
            LoggerConfiguration config,
            Action<IServiceCollection, LoggerConfiguration>? addEnricher)
        {
            config
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.With<ExceptionEnricher>()
                .Enrich.With<OsEnricher>();

            addEnricher?.Invoke(container, config);
        }
    }
}