namespace PikTools.Logs
{
    using System;
    using System.Reflection;
    using Autodesk.Revit.UI;
    using Di;
    using Enrichers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyModel;
    using Serilog;
    using Serilog.Core;
    using Serilog.Events;
    using Settings;
    using SimpleInjector;

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
        public static void AddLogs(this Container container, IConfiguration cfg = null)
        {
            RegisterLogger(container, cfg);

            container.RegisterDecorator(
                typeof(IMethodCaller<>),
                typeof(LoggedMethodCaller<>));
        }

        private static void RegisterLogger(Container container, IConfiguration cfg)
        {
            container.Register(() =>
            {
                if (cfg == null)
                {
                    TryGetConfigurationFromContainer(container, ref cfg);
                }

                return CreateLogger(cfg, container);
            });
        }

        private static void TryGetConfigurationFromContainer(Container container, ref IConfiguration cfg)
        {
            try
            {
                cfg = container.GetInstance<IConfiguration>();
            }
            catch
            {
                // ignore
            }
        }

        private static ILogger CreateLogger(IConfiguration cfg, Container container)
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

        private static void AddLogEnrichers(Container container, LoggerConfiguration config)
        {
            config
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.With<StackTraceEnricher>()
                .Enrich.With<OsEnricher>();
            EnrichWithRevitData(container, config);
        }

        private static void EnrichWithRevitData(Container container, LoggerConfiguration config)
        {
            try
            {
                var uiApp = container.GetInstance<UIApplication>();
                config.Enrich.With(new RevitEnricher(uiApp));
            }
            catch
            {
                // ignore
            }
        }
    }
}