namespace RxBim.Logs.Enrichers
{
    using System;
    using Serilog.Core;
    using Serilog.Events;

    /// <inheritdoc />
    public class ExceptionEnricher : ILogEventEnricher
    {
        /// <inheritdoc />
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Exception != null)
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(nameof(logEvent.Exception),
                    new
                    {
                        Name = logEvent.Exception.GetType().FullName,
                        Message = GetMessage(logEvent.Exception),
                    },
                    true));
            }
        }

        private string GetMessage(Exception e)
        {
            var result = e.Message;
            if (e.InnerException != null)
            {
                result += $" ----> {GetMessage(e.InnerException)}";
            }

            return result;
        }
    }
}