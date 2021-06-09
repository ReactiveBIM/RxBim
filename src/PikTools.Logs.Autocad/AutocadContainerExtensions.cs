namespace PikTools.Logs.Autocad
{
    using Di;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    /// <summary>
    /// Расширения для контейнера
    /// </summary>
    public static class AutocadContainerExtensions
    {
        /// <summary>
        /// Добавляет логгер в контейнер
        /// </summary>
        /// <param name="container">контейнер</param>
        /// <param name="cfg">Конфигурация</param>
        public static void AddLogs(this IContainer container, IConfiguration cfg = null)
        {
            container.AddLogs(cfg, EnrichWithAutocadData);
        }

        private static void EnrichWithAutocadData(IContainer container, LoggerConfiguration config)
        {
            try
            {
                config.Enrich.With(new AutocadEnricher());
            }
            catch
            {
                // ignore
            }
        }
    }
}