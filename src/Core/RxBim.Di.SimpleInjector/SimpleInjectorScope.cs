namespace RxBim.Di
{
    using SimpleInjector;

    /// <summary>
    /// The SimpleInjector implementation of <see cref="IContainerScope"/>.
    /// </summary>
    internal sealed class SimpleInjectorScope : IContainerScope
    {
        private readonly Scope _scope;
        private readonly SimpleInjectorContainer _container;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="scope">A SimpleInjector scope.</param>
        /// <param name="container">The container that the scope belongs to.</param>
        public SimpleInjectorScope(Scope scope, SimpleInjectorContainer container)
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