namespace PikTools.Logs
{
    using System.Linq;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Serilog;
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
            var config = new LoggerConfiguration();
            if (cfg != null)
            {
            }
            else
            {
                config
                    .WriteTo.Debug();
            }

            container.Register<ILogger>(() => config.CreateLogger());

            container.RegisterDecorator(
                typeof(IMethodCaller<>),
                typeof(LoggedMethodCaller<>));
        }
    }
}