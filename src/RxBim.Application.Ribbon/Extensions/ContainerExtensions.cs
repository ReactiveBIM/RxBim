namespace RxBim.Application.Ribbon.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Models.Configurations;
    using Services;
    using Services.ConfigurationBuilders;
    using Shared;
    using Shared.Abstractions;

    /// <summary>
    /// DI Container Extensions for Ribbon Menu
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds a plugin ribbon menu from an action
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="action">Action to create a menu</param>
        /// <param name="assembly">
        /// Menu definition assembly.
        /// Used to get the command type from the command type name
        /// and to define the root directory for relative icon paths
        /// </param>
        public static void AddMenu<TBuilder>(
            this IContainer container,
            Action<IRibbonBuilder> action,
            Assembly assembly)
            where TBuilder : class, IRibbonMenuBuilder
        {
            container.AddBuilder<TBuilder>(assembly);
            container.AddSingleton(() =>
            {
                var builder = new RibbonBuilder();
                action(builder);
                return builder.Ribbon;
            });
            container.DecorateContainer();
        }

        /// <summary>
        /// Adds a plugin ribbon menu from configuration
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="config">Plugin configuration</param>
        /// <param name="assembly">
        /// Menu definition assembly.
        /// Used to get the command type from the command type name
        /// and to define the root directory for relative icon paths
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

        private static void AddBuilder<TBuilder>(this IContainer container, Assembly assembly)
            where TBuilder : class, IRibbonMenuBuilder
        {
            container.AddSingleton(() => new MenuData { MenuAssembly = assembly });
            container.AddStrategies<IElementFromConfigStrategy>();
            container.AddSingleton<IRibbonMenuBuilder, TBuilder>();
        }

        private static void DecorateContainer(this IContainer container)
        {
            container.Decorate(typeof(IMethodCaller<>), typeof(MenuBuilderMethodCaller<>));
        }

        private static Ribbon GetMenuConfiguration(IContainer container, IConfiguration? cfg)
        {
            cfg ??= container.GetService<IConfiguration>();
            var strategyFactory = container.GetService<IStrategyFactory<IElementFromConfigStrategy>>();
            var strategies = strategyFactory.GetStrategies().ToList();

            var builder = new RibbonBuilder();
            builder.LoadFromConfig(cfg, strategies);

            return builder.Ribbon;
        }
    }
}