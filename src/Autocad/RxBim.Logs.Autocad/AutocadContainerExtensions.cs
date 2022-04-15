namespace RxBim.Logs.Autocad
{
    using Di;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    /// <summary>
    /// The DI container extensions. 
    /// </summary>
    public static class AutocadContainerExtensions
    {
        /// <summary>
        /// Adds logger into a container.
        /// </summary>
        /// <param name="container">The DI container.</param>
        /// <param name="cfg">The configuration.</param>
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