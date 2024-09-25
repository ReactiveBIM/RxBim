namespace RxBim.Application.Ribbon
{
    using System;
    using System.Reflection;
    using Abstractions;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Services;

    /// <summary>
    /// Extensions for <see cref="IContainer"/>.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds ribbon menu from action.
        /// </summary>
        /// <param name="container">DI container.</param>
        /// <param name="builder">The ribbon builder.</param>
        /// <param name="menuAssembly">Menu assembly.</param>
        /// <param name="lazyRibbonCreation">
        /// True - create ribbon after CAD fully initialized, some of the functionality may be lost due to the late registration of the ribbon commands;
        /// False - create ribbon panel due application startup - some of the CAD services can be unavailable!</param>
        public static void AddRevitMenu(
            this IContainer container,
            Action<IRibbonBuilder> builder,
            Assembly? menuAssembly = null,
            bool lazyRibbonCreation = true)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            container
                .AddServices()
                .AddMenu<RevitRibbonMenuBuilder>(builder, menuAssembly, lazyRibbonCreation);
        }

        /// <summary>
        /// Adds ribbon menu from config.
        /// </summary>
        /// <param name="container">DI container.</param>
        /// <param name="cfg">Configuration.</param>
        /// <param name="menuAssembly">Menu assembly.</param>
        /// <param name="lazyRibbonCreation">
        /// True - create ribbon after CAD fully initialized, some of the functionality may be lost due to the late registration of the ribbon commands;
        /// False - create ribbon panel due application startup - some of the CAD services can be unavailable!</param>
        public static void AddRevitMenu(
            this IContainer container,
            IConfiguration? cfg = null,
            Assembly? menuAssembly = null,
            bool lazyRibbonCreation = true)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            container
                .AddServices()
                .AddMenu<RevitRibbonMenuBuilder>(cfg, menuAssembly, lazyRibbonCreation);
        }

        private static IContainer AddServices(this IContainer container)
        {
            return container
                .RegisterTypes<IItemStrategy>(Lifetime.Singleton, Assembly.GetExecutingAssembly())
                .AddSingleton<IRibbonPanelItemService, RibbonPanelItemService>();
        }
    }
}