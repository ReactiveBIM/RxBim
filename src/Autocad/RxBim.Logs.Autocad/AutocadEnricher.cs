namespace RxBim.Logs.Autocad
{
    using System.IO;
    using System.Reflection;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Serilog.Core;
    using Serilog.Events;

    /// <summary>
    /// Enricher for logs. Extends logs data.
    /// </summary>
    public class AutocadEnricher : PluginEnricher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutocadEnricher"/> class.
        /// </summary>
        /// <param name="pluginAssembly">The plugin assembly.</param>
        public AutocadEnricher(Assembly pluginAssembly)
            : base(pluginAssembly)
        {
        }

        /// <inheritdoc />
        protected override void AddProperties(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            AddAutocadVersion(logEvent, propertyFactory);
            AddDocument(logEvent, propertyFactory);
        }

        private void AddAutocadVersion(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var version = Application.Version;
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("AutoCAD_Version", version));
        }

        private void AddDocument(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var document = Application.DocumentManager.MdiActiveDocument;

            if (document is null)
                return;

            var path = document.Name;
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("DocName", Path.GetFileName(path)));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("DocPath", path));
        }
    }
}