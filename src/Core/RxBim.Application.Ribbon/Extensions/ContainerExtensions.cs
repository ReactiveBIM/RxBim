namespace RxBim.Application.Ribbon
{
    using System;
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
        public static void AddMenu<TBuilder, TFactory>(
            this IContainer container,
            Action<IRibbonBuilder> builder,
            Assembly assembly)
            where TBuilder : class, IRibbonMenuBuilder
            where TFactory : class, IAddElementsStrategiesFactory
        {
            container.AddBuilder<TBuilder>(assembly);
            container.AddElementsStrategiesFactory<TFactory>();
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
        public static void AddMenu<TBuilder, TFactory>(
            this IContainer container,
            IConfiguration? config,
            Assembly assembly)
            where TBuilder : class, IRibbonMenuBuilder
            where TFactory : class, IAddElementsStrategiesFactory
        {
            container.AddBuilder<TBuilder>(assembly);
            container.AddElementsStrategiesFactory<TFactory>();
            container.AddSingleton(() => GetMenuConfiguration(container, config));
            container.DecorateContainer();
        }

        private static void AddBuilder<T>(this IContainer container, Assembly assembly)
            where T : class, IRibbonMenuBuilder
        {
            container.AddSingleton<IRibbonMenuBuilder, T>();
            container.AddSingleton<Action<Ribbon>>(() =>
            {
                var menuBuilder = container.GetService<IRibbonMenuBuilder>();
                menuBuilder.Initialize(assembly);
                return menuBuilder.BuildRibbonMenu;
            });
        }

        private static void AddElementsStrategiesFactory<TFactory>(this IContainer container)
            where TFactory : class, IAddElementsStrategiesFactory
        {
            container.AddSingleton<IAddElementsStrategiesFactory, TFactory>();
        }

        private static void DecorateContainer(this IContainer container)
        {
            container.Decorate(typeof(IMethodCaller<>), typeof(MenuBuilderMethodCaller<>));
        }

        private static Ribbon GetMenuConfiguration(IContainer container, IConfiguration? cfg)
        {
            cfg ??= container.GetService<IConfiguration>();
            var builder = new RibbonBuilder();
            builder.LoadFromConfig(cfg);
            return builder.Ribbon;
        }
    }
}