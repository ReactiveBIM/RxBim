namespace RxBim.Logs.Settings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Serilog.Sinks.Http;
    using Serilog.Sinks.Http.BatchFormatters;

    /// <inheritdoc />
    public class OneLineBatchFormatter : IBatchFormatter
    {
        private readonly ArrayBatchFormatter _arrayBatchFormatter = new();

        /// <inheritdoc />
        public void Format(IEnumerable<string> logEvents, TextWriter output)
        {
            if (logEvents == null)
                throw new ArgumentNullException(nameof(logEvents));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            logEvents = logEvents.Select(x => x.Replace(Environment.NewLine, string.Empty));
            _arrayBatchFormatter.Format(logEvents, output);
        }
    }
}