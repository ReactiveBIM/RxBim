namespace RxBim.Application.Ribbon
{
    using System;
    using System.Linq;
    using System.Reflection;
    using ConfigurationBuilders;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Shared;
    using Shared.Abstractions;

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

        private static IContainer AddStrategies<T>(this IContainer container)
        {
            return container
                .RegisterTypes<T>()
                .AddDiCollectionService<T>();
        }

        private static void AddBuilder<T>(this IContainer container, Assembly assembly)
            where T : class, IRibbonMenuBuilder
        {
            container
                .AddSingleton(() => new MenuData { MenuAssembly = assembly })
                .AddStrategies<IItemFromConfigStrategy>()
                .AddStrategies<IAddItemStrategy>()
                .AddSingleton<IRibbonMenuBuilder, T>();
        }

        private static void DecorateContainer(this IContainer container)
        {
            container.Decorate(typeof(IMethodCaller<>), typeof(MenuBuilderMethodCaller<>));
        }

        private static Ribbon GetMenuConfiguration(IContainer container, IConfiguration? cfg)
        {
            cfg ??= container.GetService<IConfiguration>();
            var strategyFactory = container.GetService<IDiCollectionService<IItemFromConfigStrategy>>();
            var strategies = strategyFactory.GetItems().ToList();

            var builder = new RibbonBuilder();
            builder.LoadFromConfig(cfg, strategies);

            return builder.Ribbon;
        }
    }
}