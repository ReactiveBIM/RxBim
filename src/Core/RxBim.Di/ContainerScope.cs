namespace RxBim.Di
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The ServiceProvider implementation of <see cref="IContainerScope"/>.
    /// </summary>
    internal class ContainerScope : IContainerScope
    {
        private readonly IServiceScope _scope;
        private readonly DiContainer _container;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="scope">A ServiceProvider scope.</param>
        /// <param name="container">The container that the scope belongs to.</param>
        public ContainerScope(IServiceScope scope, DiContainer container)
        {
            _scope = scope;
            _container = container;
        }

        /// <inheritdoc/>
        public IContainer GetContainer() => _container;

        /// <inheritdoc/>
        public void Dispose() => _scope.Dispose();
    }
}