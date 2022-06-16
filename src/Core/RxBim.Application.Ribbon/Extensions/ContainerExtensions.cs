namespace RxBim.Application.Ribbon
{
    using System;
    using System.Linq;
    using System.Reflection;
    using ConfigurationBuilders;
    using Di;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Contains DI Container Extensions for Ribbon Menu.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds a plugin ribbon menu from an action.
        /// </summary>
        /// <param name="container">DI container.</param>
        /// <param name="builder">The ribbon menu builder.</param>
        /// <param name="assembly">
        /// Menu definition assembly.
        /// Used to get the command type from the command type name
        /// and to define the root directory for relative icon paths.
        /// </param>
        public static void AddMenu<TBuilder>(
            this IContainer container,
            Action<IRibbonBuilder> builder,
            Assembly assembly)
            where TBuilder : class, IRibbonMenuBuilder
        {
            container.AddBuilder<TBuilder>(assembly);
            container.AddSingleton(() =>
            {
                var ribbon = new RibbonBuilder();
                builder(ribbon);
                return ribbon.Ribbon;
            });
            container.DecorateContainer();
        }

        /// <summary>
        /// Adds a plugin ribbon menu from configuration.
        /// </summary>
        /// <param name="container">DI container.</param>
        /// <param name="config">Plugin configuration.</param>
        /// <param name="assembly">
        /// Menu definition assembly.
        /// Used to get the command type from the command type name
        /// and to define the root directory for relative icon paths.
        /// </param>
        public static void AddMenu<TBuilder>(
            this IContainer container,
            IConfiguration? config,
            Assembly assembly)
            where TBuilder : class, IRibbonMenuBuilder
        {
            container.AddBuilder<TBuilder>(assembly);
            container.AddSingleton(() => GetMenuConfiguration(container, config));
            container.DecorateContainer();
        }

        /// <summary>
        /// Adds strategy implementations to container.
        /// </summary>
        /// <param name="container">DI container.</param>
        /// <param name="assembly">Assembly with strategy implementations.</param>
        /// <typeparam name="T">Strategy interface type.</typeparam>
        public static IContainer RegisterStrategies<T>(this IContainer container, Assembly? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            var interfaceType = typeof(T);
            var types = assembly.GetTypes()
                .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface)
                .Except(container.GetCurrentRegistrations().Select(x => x.ServiceType))
                .ToList();

            foreach (var type in types)
            {
                container.AddTransient(type);
            }

            return container;
        }

        private static IContainer AddStrategies<T>(this IContainer container)
        {
            return container
                .RegisterStrategies<T>()
                .AddSingleton<IStrategiesFactory<T>>(() => new StrategiesFactory<T>(container));
        }

        private static void AddBuilder<T>(this IContainer container, Assembly assembly)
            where T : class, IRibbonMenuBuilder
        {
            container
                .AddSingleton(() => new MenuData { MenuAssembly = assembly })
                .AddStrategies<IElementFromConfigStrategy>()
                .AddStrategies<IAddElementStrategy>()
                .AddSingleton<IRibbonMenuBuilder, T>();
        }

        private static void DecorateContainer(this IContainer container)
        {
            container.Decorate(typeof(IMethodCaller<>), typeof(MenuBuilderMethodCaller<>));
        }

        private static Ribbon GetMenuConfiguration(IContainer container, IConfiguration? cfg)
        {
            cfg ??= container.GetService<IConfiguration>();
            var strategyFactory = container.GetService<IStrategiesFactory<IElementFromConfigStrategy>>();
            var strategies = strategyFactory.GetStrategies().ToList();

            var builder = new RibbonBuilder();
            builder.LoadFromConfig(cfg, strategies);

            return builder.Ribbon;
        }
    }
}