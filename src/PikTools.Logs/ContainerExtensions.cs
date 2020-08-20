namespace PikTools.Logs
{
    using System;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using Serilog.Core;
    using Serilog.Events;
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

                return CreateLogger(cfg);
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

        private static ILogger CreateLogger(IConfiguration cfg)
        {
            var config = new LoggerConfiguration();
            if (cfg != null)
            {
                config.ReadFrom.Configuration(cfg);
            }
            else
            {
                config
                    .WriteTo.Debug()
                    .WriteTo.File("log.txt", LogEventLevel.Information, fileSizeLimitBytes: 1024 * 10);
            }

            return config.CreateLogger();
        }
    }
}