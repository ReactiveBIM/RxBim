namespace RxBim.Di.Extensions;

using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a transient service of the type specified in <typeparamref name="T"/> with a factory specified in
    /// <paramref name="implementationFactory"/> to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <typeparam name="T">The type of the service to add.</typeparam>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddTransient<T>(this IServiceCollection services, Func<T> implementationFactory)
        where T : class
    {
        return services.AddTransient<T>(_ => implementationFactory());
    }

    /// <summary>
    /// Adds a single instance that will be returned when an instance of type
    /// <paramref name="serviceType" /> is requested. This <paramref name="implementationInstance" />
    /// must be thread-safe when working in a multi-threaded environment.
    /// <b>NOTE:</b> Do note that instances supplied by this method <b>NEVER</b> get disposed by the
    /// container, since the instance is assumed to outlive this container instance.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
    /// <param name="implementationInstance">The instance of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddInstance(
        this IServiceCollection serviceCollection,
        Type serviceType,
        object implementationInstance)
    {
        serviceCollection.AddTransient(serviceType, _ => implementationInstance);
        return serviceCollection;
    }

    /// <summary>
    /// Adds a single instance that will be returned when an instance of type
    /// <typeparamref name="TService" /> is requested. This <paramref name="implementationInstance" /> must be thread-safe
    /// when working in a multi-threaded environment.
    /// <b>NOTE:</b> Do note that instances supplied by this method <b>NEVER</b> get disposed by the
    /// container, since the instance is assumed to outlive this container instance.
    /// </summary>
    /// <param name="container">The instance of <see cref="IServiceCollection"/>.</param>
    /// <param name="implementationInstance">The instance of the service.</param>
    /// <returns>A reference to the <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
    public static IServiceCollection AddInstance<TService>(
        this IServiceCollection container,
        TService implementationInstance)
        where TService : class
    {
        container.AddInstance(typeof(TService), implementationInstance);
        return container;
    }

    /// <summary>
    /// Adds a configuration via factory method <paramref name="action"/>
    /// </summary>
    /// <param name="services">The instance of <see cref="IServiceCollection"/>.</param>
    /// <param name="action">The function for creating a configuration.</param>
    public static void AddConfiguration(
        this IServiceCollection services,
        Action<IServiceProvider, IConfigurationBuilder> action)
    {
        services.AddTransient(() => action);
    }

    /// <summary>
    /// Adds type implementations to the container.
    /// </summary>
    /// <param name="services">DI container.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <param name="assembly">An assembly with type implementations.</param>
    /// <typeparam name="T">Base type.</typeparam>
    public static IServiceCollection RegisterTypes<T>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient,
        Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        return services.Scan(selector =>
            selector
                .FromAssemblies(assembly)
                .AddClasses(x => x.AssignableTo<T>())
                .AsSelfWithInterfaces()
                .WithLifetime(lifetime));
    }
}