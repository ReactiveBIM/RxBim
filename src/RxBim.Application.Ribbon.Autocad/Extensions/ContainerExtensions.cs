namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Reflection;
    using Abstractions;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Ribbon.Abstractions.ConfigurationBuilders;
    using Ribbon.Extensions;
    using Services;

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
        /// <param name="assembly">Menu assembly</param>
        public static void AddAutocadMenu(
            this IContainer container,
            Action<IRibbonBuilder> action,
            Assembly? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            container.AddInternalObjects();
            container.AddMenu<AutocadRibbonMenuBuilderFactory>(action, assembly);
        }

        /// <summary>
        /// Adds ribbon menu from config
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="cfg">Configuration</param>
        /// <param name="assembly">Menu assembly</param>
        public static void AddAutocadMenu(
            this IContainer container,
            IConfiguration? cfg = null,
            Assembly? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            container.AddInternalObjects();
            container.AddMenu<AutocadRibbonMenuBuilderFactory>(cfg, assembly);
        }

        private static void AddInternalObjects(this IContainer container)
        {
            container.AddSingleton<IOnlineHelpService, OnlineHelpService>();
            container.AddSingleton<IRibbonEvents, RibbonEventService>();
        }
    }
}