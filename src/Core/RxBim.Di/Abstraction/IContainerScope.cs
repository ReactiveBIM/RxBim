namespace RxBim.Di
{
    using System;

    /// <summary>
    /// The DI container scope.
    /// </summary>
    public interface IContainerScope : IDisposable
    {
        /// <summary>
        /// Returns the container that this scope belongs to.
        /// </summary>
        IContainer GetContainer();
    }
}