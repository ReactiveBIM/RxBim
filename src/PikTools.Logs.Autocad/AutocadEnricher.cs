namespace PikTools.Logs.Autocad
{
    using System.IO;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Serilog.Core;
    using Serilog.Events;

    /// <summary>
    /// Наполняет логи данными об AutoCAD
    /// </summary>
    public class AutocadEnricher : ILogEventEnricher
    {
        /// <inheritdoc />
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            AddVersion(logEvent, propertyFactory);
            AddDocument(logEvent, propertyFactory);
        }

        private void AddDocument(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var document = Application.DocumentManager.MdiActiveDocument;

            if (document != null)
            {
                var path = document.Name;
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("DocName", Path.GetFileName(path)));
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("DocPath", path));
            }
        }

        private void AddVersion(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var version = Application.Version;
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("AutoCAD_Version", version));
        }
    }
}