namespace RxBim.Di
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The ServiceProvider implementation of <see cref="IContainerScope"/>.
    /// </summary>
    internal class ServiceProviderScope : IContainerScope
    {
        private readonly IServiceScope _scope;
        private readonly ServiceProviderContainer _container;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="scope">A ServiceProvider scope.</param>
        /// <param name="container">The container that the scope belongs to.</param>
        public ServiceProviderScope(IServiceScope scope, ServiceProviderContainer container)
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