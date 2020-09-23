namespace PikTools.Logs.Settings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Serilog.Sinks.Http.BatchFormatters;

    /// <inheritdoc />
    public class OneLineBatchFormatter : ArrayBatchFormatter
    {
        /// <inheritdoc />
        public OneLineBatchFormatter(long? eventBodyLimitBytes = 256 * 1024)
            : base(eventBodyLimitBytes)
        {
        }

        /// <inheritdoc />
        public override void Format(IEnumerable<string> logEvents, TextWriter output)
        {
            if (logEvents == null)
                throw new ArgumentNullException(nameof(logEvents));
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            logEvents = logEvents.Select(x => x.Replace(Environment.NewLine, string.Empty));
            base.Format(logEvents, output);
        }
    }
}