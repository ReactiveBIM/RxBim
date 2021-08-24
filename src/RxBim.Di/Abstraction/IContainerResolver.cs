namespace RxBim.Di
{
    /// <summary>
    /// Container resolver
    /// </summary>
    public interface IContainerResolver
    {
        /// <summary>
        /// Resolve container 
        /// </summary>
        IContainer Resolve();
    }
}