﻿namespace RxBim.Application.Ribbon
{
    using System;
    using System.Reflection;
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
        public static void AddAutocadMenu(
            this IContainer container,
            Action<IRibbonBuilder> builder,
            Assembly? menuAssembly = null)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            container.AddServices();
            container.AddMenu<AutocadRibbonMenuBuilder>(builder, menuAssembly);
        }

        /// <summary>
        /// Adds ribbon menu from config.
        /// </summary>
        /// <param name="container">DI container.</param>
        /// <param name="cfg">Configuration.</param>
        /// <param name="menuAssembly">Menu assembly.</param>
        public static void AddAutocadMenu(
            this IContainer container,
            IConfiguration? cfg = null,
            Assembly? menuAssembly = null)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            container.AddServices();
            container.AddMenu<AutocadRibbonMenuBuilder>(cfg, menuAssembly);
        }

        private static void AddServices(this IContainer container)
        {
            container.RegisterTypes<IItemStrategy>(Lifetime.Singleton, Assembly.GetExecutingAssembly());
            container.AddSingleton<IOnlineHelpService, OnlineHelpService>();
            container.AddSingleton<IRibbonEventsService, RibbonEventsService>();
            container.AddSingleton<IColorThemeService, ColorThemeService>();
            container.AddSingleton<ITabService, TabService>();
            container.AddSingleton<IPanelService, PanelService>();
            container.AddSingleton<IButtonService, ButtonService>();
            container.AddSingleton<IRibbonComponentStorageService, RibbonComponentStorageService>();
        }
    }
}