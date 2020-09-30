#pragma warning disable

namespace PikTools.Logs.Settings.Configuration
{
    using Serilog.Configuration;

    interface IConfigurationReader : ILoggerSettings
    {
        void ApplySinks(LoggerSinkConfiguration loggerSinkConfiguration);
        void ApplyEnrichment(LoggerEnrichmentConfiguration loggerEnrichmentConfiguration);
    }
}
