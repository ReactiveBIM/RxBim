namespace PikTools.Logs.Revit
{
    using Autodesk.Revit.UI;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    /// <summary>
    /// Расширения для контейнера
    /// </summary>
    public static class RevitContainerExtensions
    {
        /// <summary>
        /// Добавляет логгер в контейнер
        /// </summary>
        /// <param name="container">контейнер</param>
        /// <param name="cfg">Конфигурация</param>
        public static void AddLogs(this IContainer container, IConfiguration cfg = null)
        {
            container.AddLogs(cfg, EnrichWithRevitData);
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