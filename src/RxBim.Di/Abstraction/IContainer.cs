namespace RxBim.Di
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// DI container.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType"/> with an
        /// implementation of the type specified in <paramref name="implementationType"/> to this
        /// DI container.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddTransient(Type serviceType, Type implementationType);
        
        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType"/> to this
        /// DI container.
        /// </summary>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddTransient(Type serviceType);

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService"/> with an
        /// implementation type specified in <typeparamref name="TImplementation"/> to this
        /// DI container.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService"/> to this
        /// DI container.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddTransient<TService>()
            where TService : class;

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService"/> using the
        /// factory specified in <paramref name="implementationFactory"/> to this
        /// DI container.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddTransient<TService>(Func<TService> implementationFactory)
            where TService : class;

        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType"/> with an
        /// implementation of the type specified in <paramref name="implementationType"/> to this
        /// DI container.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddScoped(Type serviceType, Type implementationType);
        
        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType"/> to this
        /// DI container.
        /// </summary>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddScoped(Type serviceType);

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> with an
        /// implementation type specified in <typeparamref name="TImplementation"/> to this
        /// DI container.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> to this
        /// DI container.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddScoped<TService>()
            where TService : class;

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> using the
        /// factory specified in <paramref name="implementationFactory"/> to this
        /// DI container.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddScoped<TService>(Func<TService> implementationFactory)
            where TService : class;

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType"/> with an
        /// implementation of the type specified in <paramref name="implementationType"/> to this
        /// DI container.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddSingleton(Type serviceType, Type implementationType);
        
        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType"/> to this
        /// DI container.
        /// </summary>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddSingleton(Type serviceType);

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> with an
        /// implementation type specified in <typeparamref name="TImplementation"/> to this
        /// DI container.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> to this
        /// DI container.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddSingleton<TService>()
            where TService : class;

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        /// instance specified in <paramref name="implementationInstance"/> to this
        /// DI container.
        /// </summary>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddSingleton<TService>(TService implementationInstance)
            where TService : class;

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> using the
        /// factory specified in <paramref name="implementationFactory"/> to this
        /// DI container.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddSingleton<TService>(Func<TService> implementationFactory)
            where TService : class;

        /// <summary>
        /// Adds a single instance that will be returned when an instance of type
        /// <typeparamref name="TService" /> is requested. This <paramref name="implementationInstance" /> must be thread-safe
        /// when working in a multi-threaded environment.
        /// <b>NOTE:</b> Do note that instances supplied by this method <b>NEVER</b> get disposed by the
        /// container, since the instance is assumed to outlive this container instance.
        /// </summary>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        IContainer AddInstance<TService>(TService implementationInstance)
            where TService : class;
        
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
        /// Adds a transient decorator of type <typeparamref name="TDecorator" /> for the service
        /// of type <typeparamref name="TService"/>.
        /// </summary>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <typeparam name="TService">The type of the service to decorate.</typeparam>
        /// <typeparam name="TDecorator">The type of the decorator.</typeparam>
        IContainer Decorate<TService, TDecorator>()
            where TService : class
            where TDecorator : class, TService;

        /// <summary>
        /// Returns all type registrations.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Registration> GetCurrentRegistrations();

        /// <summary>Gets an instance of the given <paramref name="serviceType" />.</summary>
        /// <param name="serviceType">Type of object requested.</param>
        /// <returns>The requested service instance.</returns>
        object GetService(Type serviceType);

        /// <summary>Gets an instance of the given <typeparamref name="TService" />.</summary>
        /// <typeparam name="TService">Type of object requested.</typeparam>
        /// <returns>The requested service instance.</returns>
        TService GetService<TService>()
            where TService : class;
    }
}