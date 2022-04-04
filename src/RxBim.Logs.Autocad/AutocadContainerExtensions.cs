namespace RxBim.Logs.Autocad
{
    using Di;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    /// <summary>
    /// DI conatainer extensions 
    /// </summary>
    public static class AutocadContainerExtensions
    {
        /// <summary>
        /// Adds logger into DI container
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="cfg">configuration</param>
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