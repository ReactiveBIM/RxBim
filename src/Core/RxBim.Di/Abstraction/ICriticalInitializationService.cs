namespace RxBim.Di;

/// <summary>
/// Service with a critical startup time that must be initialized without any delays.
/// Examples: assembly resolvers, domain level exceptions handlers, CAD startup initializations, etc.
/// </summary>
public interface ICriticalInitializationService
{
}