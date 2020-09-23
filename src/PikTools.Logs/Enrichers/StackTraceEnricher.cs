namespace PikTools.Logs.Enrichers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Serilog.Core;
    using Serilog.Events;

    /// <inheritdoc />
    public class StackTraceEnricher : ILogEventEnricher
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
                        StackTrace = GetStackTrace(logEvent.Exception),
                    }, true));
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

        private IEnumerable<string> GetStackTrace(Exception e)
        {
            var result = Result(e);

            if (e.InnerException != null)
            {
                var innerStackTrace = Result(e.InnerException);
                innerStackTrace.AddRange(result);
                result = innerStackTrace;
            }

            return result;

            List<string> Result(Exception exception)
            {
                return exception.StackTrace.Split(
                        new[] { Environment.NewLine },
                        StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
            }
        }
    }
}