namespace RxBim.Di.Extensions;

using System;
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
}