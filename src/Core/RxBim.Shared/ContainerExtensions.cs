namespace RxBim.Shared
{
    using System.Linq;
    using System.Reflection;
    using Abstractions;
    using Di;

    /// <summary>
    /// <see cref="IContainer"/> extensions for shared services.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds strategy implementations to container.
        /// </summary>
        /// <param name="container">DI container.</param>
        /// <param name="assembly">Assembly with strategy implementations.</param>
        /// <typeparam name="T">Strategy abstract type.</typeparam>
        public static IContainer AddStrategies<T>(this IContainer container, Assembly? assembly = null)
        {
            var registered = container.GetCurrentRegistrations().Any(x => x.ServiceType == typeof(IStrategyFactory<T>));
            assembly ??= Assembly.GetCallingAssembly();

            if (!registered)
            {
                container.AddSingleton<IStrategyFactory<T>>(() =>
                {
                    var factory = new StrategiesFactory<T>(container);
                    factory.AddStrategies(assembly);
                    return factory;
                });
            }
            else
            {
                var strategyFactory = container.GetService<IStrategyFactory<T>>();
                strategyFactory.AddStrategies(assembly);
            }

            return container;
        }
    }
}