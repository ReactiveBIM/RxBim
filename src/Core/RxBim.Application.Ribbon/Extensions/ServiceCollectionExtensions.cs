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
    public static class ServiceCollectionExtensions
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
        /// <param name="lazyRibbonCreation">
        /// True - create ribbon after CAD fully initialized, some of the functionality may be lost due to the late registration of the ribbon commands;
        /// False - create ribbon panel due application startup - some of the CAD services can be unavailable!</param>
        public static void AddMenu<TBuilder>(
            this IContainer container,
            Action<IRibbonBuilder> builder,
            Assembly assembly,
            bool lazyRibbonCreation = true)
            where TBuilder : class, IRibbonMenuBuilder
        {
            container.AddBuilder<TBuilder>(assembly);
            container.AddSingleton(() =>
            {
                var ribbon = new RibbonBuilder();
                builder(ribbon);
                return ribbon.Build();
            });
            container.AddMenuBuilder(lazyRibbonCreation);
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
        /// <param name="lazyRibbonCreation">
        /// True - create ribbon after CAD fully initialized, some of the functionality may be lost due to the late registration of the ribbon commands;
        /// False - create ribbon panel due application startup - some of the CAD services can be unavailable!</param>
        public static void AddMenu<TBuilder>(
            this IContainer container,
            IConfiguration? config,
            Assembly assembly,
            bool lazyRibbonCreation = true)
            where TBuilder : class, IRibbonMenuBuilder
        {
            container.AddBuilder<TBuilder>(assembly);
            container.AddSingleton(() => GetMenuConfiguration(container, config));
            container.AddMenuBuilder(lazyRibbonCreation);
        }

        /// <summary>
        /// Implementation of building plugin ribbon.
        /// </summary>
        /// <param name="serviceLocator">Service locator.</param>
        internal static void BuildRibbonMenu(this IServiceLocator serviceLocator)
        {
            try
            {
                var builder = serviceLocator.GetService<IRibbonMenuBuilder>();
                var ribbonConfiguration = serviceLocator.GetService<Ribbon>();
                builder.BuildRibbonMenu(ribbonConfiguration);
            }
            catch (Exception e)
            {
                throw new MethodCallerException("Failed to build ribbon", e);
            }
        }

        private static void AddBuilder<T>(this IContainer container, Assembly assembly)
            where T : class, IRibbonMenuBuilder
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            container
                .AddSingleton(() => new MenuData { MenuAssembly = assembly })
                .RegisterTypes<IItemFromConfigStrategy>(Lifetime.Singleton, thisAssembly)
                .RegisterTypes<IItemStrategy>(Lifetime.Singleton, thisAssembly)
                .AddSingleton<IRibbonMenuBuilder, T>();
        }

        private static void AddMenuBuilder(this IContainer container, bool lazyRibbonCreation)
        {
            if (lazyRibbonCreation)
                container.Decorate(typeof(IMethodCaller<>), typeof(MenuBuilderMethodCaller<>));
            else
                container.AddSingleton<ICriticalInitializationService, StartUpMenuBuilder>();
        }

        private static Ribbon GetMenuConfiguration(IContainer container, IConfiguration? cfg)
        {
            cfg ??= container.GetService<IConfiguration>();
            var serviceLocator = container.GetService<IServiceLocator>();
            var strategies = serviceLocator.GetServices<IItemFromConfigStrategy>().ToList();

            var builder = new RibbonBuilder();
            builder.LoadFromConfig(cfg, strategies);

            return builder.Build();
        }
    }
}