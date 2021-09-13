namespace RxBim.Di
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SimpleInjector;
    using SimpleInjector.Lifestyles;

    /// <summary>
    /// The implementation of <see cref="IContainer"/> based on <see cref="SimpleInjector"/>
    /// </summary>
    public class SimpleInjectorContainer : IContainer
    {
        private readonly Container _container;

        /// <summary>
        /// ctor
        /// </summary>
        public SimpleInjectorContainer()
        {
            _container = new Container();
            _container.Options.EnableAutoVerification = false;
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        }

        /// <inheritdoc />
        public IContainer AddTransient(Type serviceType, Type implementationType)
        {
            _container.Register(serviceType, implementationType);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddTransient(Type serviceType)
        {
            _container.Register(serviceType);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.Register<TService, TImplementation>();
            return this;
        }

        /// <inheritdoc />
        public IContainer AddTransient<TService>()
            where TService : class
        {
            _container.Register<TService>();
            return this;
        }

        /// <inheritdoc />
        public IContainer AddTransient<TService>(Func<TService> implementationFactory)
            where TService : class
        {
            _container.Register(implementationFactory);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddScoped(Type serviceType, Type implementationType)
        {
            _container.Register(serviceType, implementationType, Lifestyle.Scoped);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddScoped(Type serviceType)
        {
            _container.Register(serviceType, serviceType, Lifestyle.Scoped);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.Register<TService, TImplementation>(Lifestyle.Scoped);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddScoped<TService>()
            where TService : class
        {
            _container.Register<TService>(Lifestyle.Scoped);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddScoped<TService>(Func<TService> implementationFactory)
            where TService : class
        {
            _container.Register(implementationFactory, Lifestyle.Scoped);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddSingleton(Type serviceType, Type implementationType)
        {
            _container.Register(serviceType, implementationType, Lifestyle.Singleton);
            return this;
        }

        /// <inheritdoc />
        public IContainer Add(Type serviceType, Type implementationType, Lifetime lifetime)
        {
            _container.Register(serviceType, implementationType, GetLifestyle(lifetime));
            return this;
        }

        /// <inheritdoc />
        public IContainer Add(Type serviceType, Func<object> implementationFactory, Lifetime lifetime)
        {
            _container.Register(serviceType, implementationFactory, GetLifestyle(lifetime));
            return this;
        }

        /// <inheritdoc />
        public IContainer AddSingleton(Type serviceType,  object implementationInstance)
        {
            _container.Register(serviceType, () => implementationInstance, Lifestyle.Singleton);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddInstance(Type serviceType, object implementationInstance)
        {
            _container.RegisterInstance(serviceType, implementationInstance);
            return this;
        }

        /// <inheritdoc />
        public IContainer Decorate(Type serviceType, Type decoratorType)
        {
            _container.RegisterDecorator(serviceType, decoratorType);
            return this;
        }

        /// <inheritdoc />
        public IEnumerable<Registration> GetCurrentRegistrations()
        {
            return _container.GetCurrentRegistrations().Select(x => new Registration(x.ServiceType));
        }

        /// <inheritdoc />
        public object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }

        /// <inheritdoc />
        public IContainerScope CreateScope()
        {
            var scope = AsyncScopedLifestyle.BeginScope(_container);
            return new SimpleInjectorScope(scope, this);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _container.Dispose();
        }

        private Lifestyle GetLifestyle(Lifetime lifetime) => lifetime switch
        {
            Lifetime.Transient => Lifestyle.Transient,
            Lifetime.Scoped => Lifestyle.Scoped,
            Lifetime.Singleton => Lifestyle.Singleton,
            _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
        };
    }
}