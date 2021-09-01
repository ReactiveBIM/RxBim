#pragma warning disable SA1600
namespace RxBim.Logs.Settings.Configuration
{
    using Serilog.Configuration;

    internal interface IConfigurationReader : ILoggerSettings
    {
        void ApplySinks(LoggerSinkConfiguration loggerSinkConfiguration);
        void ApplyEnrichment(LoggerEnrichmentConfiguration loggerEnrichmentConfiguration);
    }
}
