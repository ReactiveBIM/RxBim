namespace PikTools.Logs.Revit
{
    using Autodesk.Revit.UI;
    using Serilog.Core;
    using Serilog.Events;

    /// <summary>
    /// Наполняет логи данными о Revit
    /// </summary>
    public class RevitEnricher : ILogEventEnricher
    {
        private readonly UIApplication _application;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="application">revit ui application</param>
        public RevitEnricher(UIApplication application)
        {
            _application = application;
        }

        /// <inheritdoc />
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            AddRevitVersion(logEvent, propertyFactory);
            AddDocument(logEvent, propertyFactory);
        }

        private void AddDocument(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_application.ActiveUIDocument != null)
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("DocName",
                    _application.ActiveUIDocument.Document.Title));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("DocPath",
                    _application.ActiveUIDocument.Document.PathName));
            }
        }

        private void AddRevitVersion(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var version =
                $"{_application.Application.VersionNumber}-{_application.Application.VersionBuild}";
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RevitVersion", version));
        }
    }
}