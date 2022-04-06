namespace RxBim.Application.Ribbon.Revit.Extensions
{
    using System;
    using System.Reflection;
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Ribbon.Extensions;
    using Services;
    using Shared;

    /// <summary>
    /// Extensions for <see cref="IContainer"/>
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds ribbon menu from action
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="action">Menu building action</param>
        /// <param name="menuAssembly">Menu assembly</param>
        public static void AddRevitMenu(
            this IContainer container,
            Action<IRibbonBuilder> action,
            Assembly menuAssembly = null)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            container.RegisterTypes<IAddElementStrategy>();
            container.AddMenu<RevitRibbonMenuBuilder>(action, menuAssembly);
        }

        /// <summary>
        /// Adds ribbon menu from config
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="cfg">Configuration</param>
        /// <param name="menuAssembly">Menu assembly</param>
        public static void AddRevitMenu(
            this IContainer container,
            IConfiguration cfg = null,
            Assembly menuAssembly = null)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            container.RegisterTypes<IAddElementStrategy>();
            container.AddMenu<RevitRibbonMenuBuilder>(cfg, menuAssembly);
        }
    }
}