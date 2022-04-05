namespace RxBim.Di
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The implementation of <see cref="IContainer"/> based on <see cref="ServiceProvider"/>.
    /// </summary>
    public class ServiceProviderContainer : IContainer
    {
        private readonly Lazy<IServiceProvider> _serviceProvider;
        private readonly IServiceCollection _serviceCollection;

        /// <summary>
        /// ctor.
        /// </summary>
        public ServiceProviderContainer()
        {
            _serviceCollection = new ServiceCollection();
            _serviceProvider = new Lazy<IServiceProvider>(() => ServiceCollection.BuildServiceProvider(false));
        }

        /// <summary>
        /// Internal ctor.
        /// </summary>
        /// <param name="serviceProvider">The scoped service provider.</param>
        private ServiceProviderContainer(IServiceProvider serviceProvider)
        {
            _serviceProvider = new Lazy<IServiceProvider>(() => serviceProvider);

            // lazy value initializing
            _ = _serviceProvider.Value;
        }

        private IServiceCollection ServiceCollection
        {
            get
            {
                if (_serviceProvider.IsValueCreated)
                    throw new InvalidOperationException("Container is already built");

                return _serviceCollection;
            }
        }

        private IServiceProvider ServiceProvider => _serviceProvider.Value;

        /// <inheritdoc />
        public IContainer Add(Type serviceType, Type implementationType, Lifetime lifetime)
        {
            Add(ServiceCollection, serviceType, implementationType, lifetime);
            return this;
        }

        /// <inheritdoc />
        public IContainer Add(Type serviceType, Func<object> implementationFactory, Lifetime lifetime)
        {
            Add(ServiceCollection, serviceType, _ => implementationFactory(), lifetime);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddSingleton(Type serviceType, object implementationInstance)
        {
            ServiceCollection.AddSingleton(serviceType, implementationInstance);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddInstance(Type serviceType, object implementationInstance)
        {
            ServiceCollection.AddTransient(serviceType, _ => implementationInstance);
            return this;
        }

        /// <inheritdoc />
        public IContainer Decorate(Type serviceType, Type decoratorType)
        {
            ServiceCollection.Decorate(serviceType, decoratorType);
            return this;
        }

        /// <inheritdoc />
        public IEnumerable<Registration> GetCurrentRegistrations()
        {
            return ServiceCollection.Select(x => new Registration(x.ServiceType));
        }

        /// <inheritdoc />
        public object GetService(Type serviceType) => ServiceProvider.GetService(serviceType);

        /// <inheritdoc />
        public IContainerScope CreateScope()
        {
            var scope = ServiceProvider.CreateScope();
            var container = new ServiceProviderContainer(scope.ServiceProvider);
            return new ServiceProviderScope(scope, container);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_serviceProvider.IsValueCreated && ServiceProvider is IDisposable disposable)
                disposable.Dispose();
        }

        private static IServiceCollection Add(
            IServiceCollection collection,
            Type serviceType,
            Type implementationType,
            Lifetime lifetime)
        {
            var descriptor = new ServiceDescriptor(serviceType, implementationType, GetServiceLifetime(lifetime));
            collection.Add(descriptor);
            return collection;
        }

        private static IServiceCollection Add(
            IServiceCollection collection,
            Type serviceType,
            Func<IServiceProvider, object> implementationFactory,
            Lifetime lifetime)
        {
            var descriptor = new ServiceDescriptor(serviceType, implementationFactory, GetServiceLifetime(lifetime));
            collection.Add(descriptor);
            return collection;
        }

        private static ServiceLifetime GetServiceLifetime(Lifetime lifetime) => lifetime switch
        {
            Lifetime.Transient => ServiceLifetime.Transient,
            Lifetime.Scoped => ServiceLifetime.Scoped,
            Lifetime.Singleton => ServiceLifetime.Singleton,
            _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
        };
    }
}