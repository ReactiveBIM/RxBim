#pragma warning disable
namespace PikTools.Di
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// DI container
    /// </summary>
    public interface IContainer
    {
        IContainer AddTransient(Type service, Type implementation);
        IContainer AddTransient(Type service);

        IContainer AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        IContainer AddTransient<TService>()
            where TService : class;

        IContainer AddTransient<TService>(Func<TService> factory)
            where TService : class;

        IContainer AddScoped(Type service, Type implementation);
        IContainer AddScoped(Type service);

        IContainer AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        IContainer AddScoped<TService>()
            where TService : class;

        IContainer AddScoped<TService>(Func<TService> factory)
            where TService : class;

        IContainer AddSingleton(Type service, Type implementation);
        IContainer AddSingleton(Type service);

        IContainer AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        IContainer AddSingleton<TService>()
            where TService : class;

        IContainer AddSingleton<TService>(Func<TService> factory)
            where TService : class;

        IContainer AddInstance<TService>(TService service)
            where TService : class;

        IContainer Decorate(Type service, Type decorator);

        IContainer Decorate<TService, TDecorator>(TService service, TDecorator decorator)
            where TService : class
            where TDecorator : class, TService;

        IEnumerable<Registration> GetCurrentRegistrations();

        object GetService(Type type);

        T GetService<T>()
            where T : class;
    }
}