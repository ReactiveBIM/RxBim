namespace RxBim.Application.Ribbon;
using RxBim.Di;
using RxBim.Di.Abstraction;

/// <summary>
/// Service for building ribbon menu at plugin startup.
/// </summary>
internal class StartUpMenuBuilder : ICriticalInitializationService
{
    /// <inheritdoc cref="StartUpMenuBuilder"/>
    public StartUpMenuBuilder(IContainer container)
    {
        container.BuildRibbonMenu();
    }
}