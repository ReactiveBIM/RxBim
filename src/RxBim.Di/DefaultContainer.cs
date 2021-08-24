#pragma warning disable
namespace RxBim.Di
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SimpleInjector;

    public class DefaultContainer : IContainer
    {
        private readonly Container _container;

        public DefaultContainer()
        {
            _container = new Container();
            _container.Options.EnableAutoVerification = false;
        }

        public IContainer AddTransient(Type service, Type implementation)
        {
            _container.Register(service, implementation);
            return this;
        }

        public IContainer AddTransient(Type service)
        {
            _container.Register(service);
            return this;
        }

        public IContainer AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.Register<TService, TImplementation>();
            return this;
        }

        public IContainer AddTransient<TService>()
            where TService : class
        {
            _container.Register<TService>();
            return this;
        }

        public IContainer AddTransient<TService>(TService service)
            where TService : class
        {
            _container.Register(() => service);
            return this;
        }

        public IContainer AddTransient<TService>(Func<TService> factory)
            where TService : class
        {
            _container.Register(factory);
            return this;
        }

        public IContainer AddScoped(Type service, Type implementation)
        {
            _container.Register(service, implementation, Lifestyle.Scoped);
            return this;
        }

        public IContainer AddScoped(Type service)
        {
            _container.Register(service, service, Lifestyle.Scoped);
            return this;
        }

        public IContainer AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.Register<TService, TImplementation>(Lifestyle.Scoped);
            return this;
        }

        public IContainer AddScoped<TService>()
            where TService : class
        {
            _container.Register<TService>(Lifestyle.Scoped);
            return this;
        }

        public IContainer AddScoped<TService>(TService service)
            where TService : class
        {
            _container.Register(() => service, Lifestyle.Scoped);
            return this;
        }

        public IContainer AddScoped<TService>(Func<TService> factory)
            where TService : class
        {
            _container.Register(factory, Lifestyle.Scoped);
            return this;
        }

        public IContainer AddSingleton(Type service, Type implementation)
        {
            _container.Register(service, implementation, Lifestyle.Singleton);
            return this;
        }

        public IContainer AddSingleton(Type service)
        {
            _container.Register(service, service, Lifestyle.Singleton);
            return this;
        }

        public IContainer AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.Register<TService, TImplementation>(Lifestyle.Singleton);
            return this;
        }

        public IContainer AddSingleton<TService>()
            where TService : class
        {
            _container.Register<TService>(Lifestyle.Singleton);
            return this;
        }

        public IContainer AddSingleton<TService>(TService service)
            where TService : class
        {
            _container.Register(() => service, Lifestyle.Singleton);
            return this;
        }

        public IContainer AddSingleton<TService>(Func<TService> factory)
            where TService : class
        {
            _container.Register(factory, Lifestyle.Singleton);
            return this;
        }

        public IContainer AddInstance<TService>(TService service)
            where TService : class
        {
            _container.RegisterInstance(service);
            return this;
        }

        public IContainer AddInstance(Type type, object instance)
        {
            _container.RegisterInstance(type, instance);
            return this;
        }

        public IContainer Decorate(Type service, Type decorator)
        {
            _container.RegisterDecorator(service, decorator);
            return this;
        }

        public IContainer Decorate<TService, TDecorator>(TService service, TDecorator decorator)
            where TService : class
            where TDecorator : class, TService
        {
            _container.RegisterDecorator<TService, TDecorator>();
            return this;
        }

        public IEnumerable<Registration> GetCurrentRegistrations()
        {
            return _container.GetCurrentRegistrations().Select(x => new Registration(x.ServiceType));
        }

        public object GetService(Type type)
        {
            return _container.GetInstance(type);
        }

        public T GetService<T>()
            where T : class
        {
            return _container.GetInstance<T>();
        }
    }
}