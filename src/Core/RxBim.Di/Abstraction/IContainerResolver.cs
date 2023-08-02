namespace RxBim.Di
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// A DI container resolver abstraction.
    /// </summary>
    public interface IContainerResolver
    {
        /// <summary>
        /// Resolves a container.
        /// </summary>
        /// <returns>The DI container.</returns>
        IServiceCollection Resolve();
    }
}