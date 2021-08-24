namespace RxBim.Logs.Enrichers
{
    using System;
    using Serilog.Core;
    using Serilog.Events;

    /// <summary>
    /// Наполняет логи данными о ОС
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