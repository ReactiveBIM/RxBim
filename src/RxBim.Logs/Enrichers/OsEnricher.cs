namespace RxBim.Logs.Enrichers
{
    using System;
    using Serilog.Core;
    using Serilog.Events;

    /// <summary>
    /// Logs enricher. Extends logs with operating system information.
    /// </summary>
    public class OsEnricher : ILogEventEnricher
    {
        /// <inheritdoc />
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("OperatingSystem", Environment.OSVersion));
        }
    }
}