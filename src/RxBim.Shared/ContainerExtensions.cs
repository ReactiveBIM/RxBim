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
        /// Adds type implementations to the container.
        /// </summary>
        /// <param name="container">DI container.</param>
        /// <param name="assembly">An assembly with type implementations.</param>
        /// <typeparam name="T">Base type.</typeparam>
        public static IContainer RegisterTypes<T>(this IContainer container, Assembly assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            var interfaceType = typeof(T);
            var types = assembly.GetTypes()
                .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface)
                .Except(container.GetCurrentRegistrations().Select(x => x.ServiceType))
                .ToList();

            foreach (var type in types)
                container.AddTransient(type);

            return container;
        }

        /// <summary>
        /// Adds <see cref="IDiCollectionService{T}"/> to the container.
        /// </summary>
        /// <param name="container">DI container.</param>
        /// <typeparam name="T">Collection member type.</typeparam>
        public static IContainer AddDiCollectionService<T>(this IContainer container)
        {
            return container.AddSingleton<IDiCollectionService<T>>(() => new DiCollectionService<T>(container));
        }
    }
}