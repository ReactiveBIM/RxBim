namespace RxBim.Di;

using System;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// DI container abstraction.
/// </summary>
public interface IContainer
{
    /// <summary>
    /// Services collection.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Services provider.
    /// </summary>
    IServiceProvider ServiceProvider { get; }
}