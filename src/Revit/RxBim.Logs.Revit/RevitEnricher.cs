namespace RxBim.Logs.Revit
{
    using System.Reflection;
    using Autodesk.Revit.UI;
    using Serilog.Core;
    using Serilog.Events;

    /// <summary>
    /// Enricher for Revit logs. Extends logs data.
    /// </summary>
    public class RevitEnricher : PluginEnricher
    {
        private readonly UIApplication _application;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="application">Revit ui application.</param>
        /// <param name="pluginAssembly">The plugin assembly.</param>
        public RevitEnricher(UIApplication application, Assembly pluginAssembly)
            : base(pluginAssembly)
        {
            _application = application;
        }

        /// <inheritdoc />
        protected override void AddProperties(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            AddRevitVersion(logEvent, propertyFactory);
            AddDocument(logEvent, propertyFactory);
        }

        private void AddRevitVersion(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var version =
                $"{_application.Application.VersionNumber}-{_application.Application.VersionBuild}";
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RevitVersion", version));
        }

        private void AddDocument(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_application.ActiveUIDocument is null)
                return;
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("DocName",
                _application.ActiveUIDocument.Document.Title));
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("DocPath",
                _application.ActiveUIDocument.Document.PathName));
        }
    }
}