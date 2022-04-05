namespace RxBim.Logs.Revit
{
    using Autodesk.Revit.UI;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    /// <summary>
    /// DI container extensions.
    /// </summary>
    public static class RevitContainerExtensions
    {
        /// <summary>
        /// Adds logs into a DI container.
        /// </summary>
        /// <param name="container">The DI container.</param>
        /// <param name="cfg">The configuration.</param>
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