namespace RxBim.Di
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SimpleInjector;

    /// <summary>
    /// The default implementation of <see cref="IContainer"/>
    /// </summary>
    public class DefaultContainer : IContainer
    {
        private readonly Container _container;

        /// <summary>
        /// ctor
        /// </summary>
        public DefaultContainer()
        {
            _container = new Container();
            _container.Options.EnableAutoVerification = false;
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
        public IContainer AddSingleton(Type serviceType)
        {
            _container.Register(serviceType, serviceType, Lifestyle.Singleton);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.Register<TService, TImplementation>(Lifestyle.Singleton);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddSingleton<TService>()
            where TService : class
        {
            _container.Register<TService>(Lifestyle.Singleton);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddSingleton<TService>(TService implementationInstance)
            where TService : class
        {
            _container.Register(() => implementationInstance, Lifestyle.Singleton);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddSingleton<TService>(Func<TService> implementationFactory)
            where TService : class
        {
            _container.Register(implementationFactory, Lifestyle.Singleton);
            return this;
        }

        /// <inheritdoc />
        public IContainer AddInstance<TService>(TService implementationInstance)
            where TService : class
        {
            _container.RegisterInstance(implementationInstance);
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
        public IContainer Decorate<TService, TDecorator>()
            where TService : class
            where TDecorator : class, TService
        {
            _container.RegisterDecorator<TService, TDecorator>();
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
        public T GetService<T>()
            where T : class
        {
            return _container.GetInstance<T>();
        }
    }
}