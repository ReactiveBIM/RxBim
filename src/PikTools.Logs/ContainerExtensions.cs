namespace PikTools.Logs
{
    using Autodesk.Revit.UI;
    using Di;
    using Enrichers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyModel;
    using Serilog;
    using Serilog.Events;

    /// <summary>
    /// Расширения для контейнера
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Добавляет логгер в контейнер
        /// </summary>
        /// <param name="container">контейнер</param>
        /// <param name="cfg">Конфигурация</param>
        public static void AddLogs(this IContainer container, IConfiguration cfg = null)
        {
            RegisterLogger(container, cfg);

            container.Decorate(typeof(IMethodCaller<>), typeof(LoggedMethodCaller<>));
        }

        private static void RegisterLogger(IContainer container, IConfiguration cfg)
        {
            container.AddTransient(() =>
            {
                if (cfg == null)
                {
                    TryGetConfigurationFromContainer(container, ref cfg);
                }

                return CreateLogger(cfg, container);
            });
        }

        private static void TryGetConfigurationFromContainer(IContainer container, ref IConfiguration cfg)
        {
            try
            {
                cfg = container.GetService<IConfiguration>();
            }
            catch
            {
                // ignore
            }
        }

        private static ILogger CreateLogger(IConfiguration cfg, IContainer container)
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

            AddLogEnrichers(container, config);

            return config.CreateLogger();
        }

        private static void AddLogEnrichers(IContainer container, LoggerConfiguration config)
        {
            config
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.With<ExceptionEnricher>()
                .Enrich.With<OsEnricher>();
            EnrichWithRevitData(container, config);
        }

        private static void EnrichWithRevitData(IContainer container, LoggerConfiguration config)
        {
            try
            {
                var uiApp = container.GetService<UIApplication>();
                config.Enrich.With(new RevitEnricher(uiApp));
            }
            catch
            {
                // ignore
            }
        }
    }
}