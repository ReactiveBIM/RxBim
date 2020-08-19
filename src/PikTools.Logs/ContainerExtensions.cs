namespace PikTools.Logs
{
    using Di;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using Serilog.Core;
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
            var logger = CreateLogger(cfg);
            container.RegisterInstance<ILogger>(logger);
        }

        private static Logger CreateLogger(IConfiguration cfg)
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
                    .WriteTo.File("log.txt");
            }

            var logger = config.CreateLogger();
            return logger;
        }
    }
}