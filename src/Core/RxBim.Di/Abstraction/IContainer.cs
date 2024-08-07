namespace RxBim.Di;

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// DI container abstraction.
/// </summary>
public interface IContainer : IDisposable
{
    /// <summary>
    /// Occurs right after the container is built.
    /// </summary>
    event EventHandler? ContainerBuilt;

    /// <summary>
    /// Services collection.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Services provider.
    /// </summary>
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Adds a service of the type specified in <paramref name="serviceType"/>
    /// and the lifetime specified in <paramref name="lifetime"/> with an
    /// implementation of the type specified in <paramref name="implementationType"/> to this
    /// DI container.
    /// </summary>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IContainer Add(Type serviceType, Type implementationType, Lifetime lifetime);

    /// <summary>
    /// Adds a service of the type specified in <paramref name="serviceType"/>
    /// and the lifetime specified in <paramref name="lifetime"/> using the
    /// factory specified in <paramref name="implementationFactory"/> to this
    /// DI container.
    /// </summary>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationFactory">The factory that creates the service.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IContainer Add(Type serviceType, Func<object> implementationFactory, Lifetime lifetime);

    /// <summary>
    /// Adds a singleton service of the type specified in <paramref name="serviceType"/> with an
    /// instance specified in <paramref name="implementationInstance"/> to this
    /// DI container.
    /// </summary>
    /// <param name="serviceType">The type of the service to register.</param>
    /// <param name="implementationInstance">The instance of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IContainer AddSingleton(Type serviceType, object implementationInstance);

    /// <summary>
    /// Adds a single instance that will be returned when an instance of type
    /// <paramref name="serviceType" /> is requested. This <paramref name="implementationInstance" />
    /// must be thread-safe when working in a multi-threaded environment.
    /// <b>NOTE:</b> Do note that instances supplied by this method <b>NEVER</b> get disposed by the
    /// container, since the instance is assumed to outlive this container instance.
    /// </summary>
    /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
    /// <param name="implementationInstance">The instance of the service.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IContainer AddInstance(Type serviceType, object implementationInstance);

    /// <summary>
    /// Adds a transient decorator of type <paramref name="decoratorType" /> for the service
    /// of the type specified in <paramref name="serviceType"/>.
    /// </summary>
    /// <param name="serviceType">The type of the service to decorate.</param>
    /// <param name="decoratorType">The type of the decorator.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IContainer Decorate(Type serviceType, Type decoratorType);

    /// <summary>
    /// Returns all type registrations.
    /// </summary>
    /// <returns></returns>
    IEnumerable<Registration> GetCurrentRegistrations();

    /// <summary>Gets an instance of the given <paramref name="serviceType" />.</summary>
    /// <param name="serviceType">Type of object requested.</param>
    /// <returns>The requested service instance.</returns>
    object GetService(Type serviceType);

    /// <summary>
    /// Creates a DI container scope.
    /// </summary>
    /// <returns>The container scope instance.</returns>
    IContainerScope CreateScope();
}