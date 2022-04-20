namespace RxBim.Di
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Extension methods for <see cref="IContainer"/>.
    /// </summary>
    [PublicAPI]
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType"/> with an
        /// implementation of the type specified in <paramref name="implementationType"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddTransient(
            this IContainer container,
            Type serviceType,
            Type implementationType)
        {
            container.Add(serviceType, implementationType, Lifetime.Transient);
            return container;
        }

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddTransient(
            this IContainer container,
            Type serviceType)
        {
            container.AddTransient(serviceType, serviceType);
            return container;
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService"/> with an
        /// implementation type specified in <typeparamref name="TImplementation"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddTransient<TService, TImplementation>(
            this IContainer container)
            where TService : class
            where TImplementation : class, TService
        {
            container.AddTransient(typeof(TService), typeof(TImplementation));
            return container;
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddTransient<TService>(
            this IContainer container)
            where TService : class
        {
            container.AddTransient(typeof(TService), typeof(TService));
            return container;
        }

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService"/> using the
        /// factory specified in <paramref name="implementationFactory"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddTransient<TService>(
            this IContainer container,
            Func<TService> implementationFactory)
            where TService : class
        {
            container.Add(typeof(TService), implementationFactory, Lifetime.Transient);
            return container;
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType"/> with an
        /// implementation of the type specified in <paramref name="implementationType"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddScoped(
            this IContainer container,
            Type serviceType,
            Type implementationType)
        {
            container.Add(serviceType, implementationType, Lifetime.Scoped);
            return container;
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddScoped(
            this IContainer container,
            Type serviceType)
        {
            container.AddScoped(serviceType, serviceType);
            return container;
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> with an
        /// implementation type specified in <typeparamref name="TImplementation"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddScoped<TService, TImplementation>(
            this IContainer container)
            where TService : class
            where TImplementation : class, TService
        {
            container.AddScoped(typeof(TService), typeof(TImplementation));
            return container;
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddScoped<TService>(
            this IContainer container)
            where TService : class
        {
            container.AddScoped(typeof(TService), typeof(TService));
            return container;
        }

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> using the
        /// factory specified in <paramref name="implementationFactory"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddScoped<TService>(
            this IContainer container,
            Func<TService> implementationFactory)
            where TService : class
        {
            container.Add(typeof(TService), implementationFactory, Lifetime.Scoped);
            return container;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType"/> with an
        /// implementation of the type specified in <paramref name="implementationType"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddSingleton(
            this IContainer container,
            Type serviceType,
            Type implementationType)
        {
            container.Add(serviceType, implementationType, Lifetime.Singleton);
            return container;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddSingleton(
            this IContainer container,
            Type serviceType)
        {
            container.Add(serviceType, serviceType, Lifetime.Singleton);
            return container;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> with an
        /// implementation type specified in <typeparamref name="TImplementation"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddSingleton<TService, TImplementation>(
            this IContainer container)
            where TService : class
            where TImplementation : class, TService
        {
            container.Add(typeof(TService), typeof(TImplementation), Lifetime.Singleton);
            return container;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddSingleton<TService>(
            this IContainer container)
            where TService : class
        {
            container.Add(typeof(TService), typeof(TService), Lifetime.Singleton);
            return container;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> using the
        /// factory specified in <paramref name="implementationFactory"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddSingleton<TService>(
            this IContainer container,
            Func<TService> implementationFactory)
            where TService : class
        {
            container.Add(typeof(TService), implementationFactory, Lifetime.Singleton);
            return container;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        /// instance specified in <paramref name="implementationInstance"/> to the
        /// DI container.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddSingleton<TService>(
            this IContainer container,
            TService implementationInstance)
            where TService : class
        {
            container.AddSingleton(() => implementationInstance);
            return container;
        }

        /// <summary>
        /// Adds a single instance that will be returned when an instance of type
        /// <typeparamref name="TService" /> is requested. This <paramref name="implementationInstance" /> must be thread-safe
        /// when working in a multi-threaded environment.
        /// <b>NOTE:</b> Do note that instances supplied by this method <b>NEVER</b> get disposed by the
        /// container, since the instance is assumed to outlive this container instance.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        public static IContainer AddInstance<TService>(
            this IContainer container,
            TService implementationInstance)
            where TService : class
        {
            container.AddInstance(typeof(TService), implementationInstance);
            return container;
        }

        /// <summary>
        /// Adds a transient decorator of type <typeparamref name="TDecorator" /> for the service
        /// of type <typeparamref name="TService"/>.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <returns>A reference to the <see cref="IContainer"/> instance after the operation has completed.</returns>
        /// <typeparam name="TService">The type of the service to decorate.</typeparam>
        /// <typeparam name="TDecorator">The type of the decorator.</typeparam>
        public static IContainer Decorate<TService, TDecorator>(
            this IContainer container)
            where TService : class
            where TDecorator : class, TService
        {
            container.Decorate(typeof(TService), typeof(TDecorator));
            return container;
        }

        /// <summary>
        /// Adds a configuration via factory method <param name="action">action</param>.
        /// </summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <param name="action">The function for creating a configuration.</param>
        public static void AddConfiguration(
            this IContainer container,
            Func<ConfigurationBuilder, IConfiguration> action)
        {
            if (action != null)
            {
                container.AddInstance(action);
            }
        }

        /// <summary>Gets an instance of type <typeparamref name="TService"/>.</summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <typeparam name="TService">The type of the requested service.</typeparam>
        /// <returns>The requested service instance.</returns>
        public static TService GetService<TService>(
            this IContainer container)
        {
            return (TService)container.GetService(typeof(TService));
        }

        /// <summary>Gets an instance of the given <paramref name="serviceType" />.</summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <param name="serviceType">Type of object requested.</param>
        /// <returns>The requested service instance.</returns>
        /// <exception cref="System.InvalidOperationException">There is no service of type <paramref name="serviceType"/>.</exception>
        public static object GetRequiredService(
            this IContainer container,
            Type serviceType)
        {
            return container.GetService(serviceType) ?? throw new InvalidOperationException(nameof(container));
        }

        /// <summary>Gets an instance of type <typeparamref name="TService"/>.</summary>
        /// <param name="container">The instance of <see cref="IContainer"/>.</param>
        /// <typeparam name="TService">The type of the requested service.</typeparam>
        /// <returns>The requested service instance.</returns>
        /// <exception cref="System.InvalidOperationException">There is no service of type <typeparamref name="TService"/>.</exception>
        public static TService GetRequiredService<TService>(
            this IContainer container)
            where TService : class
        {
            return container.GetService<TService>() ?? throw new InvalidOperationException(nameof(container));
        }
    }
}