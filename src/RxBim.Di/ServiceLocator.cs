namespace RxBim.Di
{
    using System;

    /// <inheritdoc />
    internal class ServiceLocator : IServiceLocator
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        /// <param name="container">The DI container</param>
        public ServiceLocator(IContainer container)
        {
            _container = container;
        }

        /// <inheritdoc/>
        public T GetService<T>() => _container.GetService<T>();

        /// <inheritdoc/>
        public object GetService(Type type) => _container.GetService(type);
    }
}