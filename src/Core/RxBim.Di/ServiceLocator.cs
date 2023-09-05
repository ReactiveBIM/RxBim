namespace RxBim.Di
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    internal class ServiceLocator : IServiceLocator
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocator"/> class.
        /// </summary>
        /// <param name="container">The DI container.</param>
        public ServiceLocator(IContainer container)
        {
            _container = container;
        }

        /// <inheritdoc/>
        public T GetService<T>() => _container.GetService<T>();

        /// <inheritdoc/>
        public object GetService(Type type) => _container.GetService(type);

        /// <inheritdoc/>
        public IEnumerable<T> GetServices<T>()
        {
            return _container.ServiceProvider.GetServices<T>();
        }

        /// <inheritdoc/>
        public IEnumerable<object> GetServices(Type type)
        {
            return _container.ServiceProvider.GetServices(type);
        }

        /// <inheritdoc />
        public IEnumerable<T> GetServicesAssignableTo<T>()
        {
            return GetServices<T>();
        }

        /// <inheritdoc />
        public IEnumerable<object> GetServicesAssignableTo(Type type)
        {
            return GetServices(type);
        }
    }
}