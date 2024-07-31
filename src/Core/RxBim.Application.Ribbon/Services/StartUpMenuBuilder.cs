namespace RxBim.Application.Ribbon;
using RxBim.Di;

/// <summary>
/// Service for building ribbon menu at plugin startup.
/// </summary>
internal class StartUpMenuBuilder : ICriticalInitializationService
{
    /// <inheritdoc cref="StartUpMenuBuilder"/>
    public StartUpMenuBuilder(IServiceLocator container)
    {
        container.BuildRibbonMenu();
    }
}