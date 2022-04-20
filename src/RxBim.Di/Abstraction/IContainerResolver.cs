namespace RxBim.Di
{
    /// <summary>
    /// A DI container resolver abstraction.
    /// </summary>
    public interface IContainerResolver
    {
        /// <summary>
        /// Resolves a container.
        /// </summary>
        /// <returns>The DI container.</returns>
        IContainer Resolve();
    }
}