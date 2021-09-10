namespace RxBim.Di
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SimpleInjector;

    /// <summary>
    /// The default implementation of <see cref="IContainer"/>.
    /// </summary>
    public sealed class DefaultContainer : IContainer
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

        private Lifestyle GetLifestyle(Lifetime lifetime) => lifetime switch
        {
            Lifetime.Transient => Lifestyle.Transient,
            Lifetime.Scoped => Lifestyle.Scoped,
            Lifetime.Singleton => Lifestyle.Singleton,
            _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
        };
    }
}