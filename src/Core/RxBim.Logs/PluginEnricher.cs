namespace RxBim.Logs;

using System.Reflection;
using Serilog.Core;
using Serilog.Events;

/// <summary>
/// Base enricher for plugin logs. Extends logs data.
/// </summary>
public abstract class PluginEnricher : ILogEventEnricher
{
    private readonly Assembly _pluginAssembly;

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginEnricher"/> class.
    /// </summary>
    /// <param name="pluginAssembly">The plugin main assembly.</param>
    protected PluginEnricher(Assembly pluginAssembly)
    {
        _pluginAssembly = pluginAssembly;
    }

    /// <inheritdoc />
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        AddPluginVersion(logEvent, propertyFactory);
    }

    /// <summary>
    /// Adds properties to logs.
    /// </summary>
    /// <param name="logEvent"><see cref="LogEvent"/> object.</param>
    /// <param name="propertyFactory"><see cref="ILogEventPropertyFactory"/> object.</param>
    protected abstract void AddProperties(LogEvent logEvent, ILogEventPropertyFactory propertyFactory);

    private void AddPluginVersion(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var version = _pluginAssembly.GetName().Version;
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Plugin_Version", version));
    }
}