namespace RxBim.Di.Extensions;

using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
[PublicAPI]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a configuration via factory method <paramref name="action"/>
    /// </summary>
    /// <param name="services">The instance of <see cref="IServiceCollection"/>.</param>
    /// <param name="action">The function for creating a configuration.</param>
    public static void AddConfiguration(
        this IServiceCollection services,
        Action<IServiceCollection, IConfigurationBuilder>? action)
    {
        if (action != null)
            services.AddSingleton(action);
    }

    /// <summary>
    /// Adds all services assignable to TService.
    /// </summary>
    /// <param name="services">DI container.</param>
    /// <param name="assembly">An assembly to load </param>
    /// <param name="lifetime">Service lifetime.</param>
    /// <typeparam name="TService">The type of services.</typeparam>
    public static IServiceCollection RegisterTypes<TService>(
        this IServiceCollection services,
        Assembly assembly,
        ServiceLifetime lifetime)
    {
        return services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo<TService>())
            .AsImplementedInterfaces()
            .WithLifetime(lifetime));
    }
}