namespace RxBim.Application.Ribbon
{
    using System;
    using System.Reflection;
    using Di;
    using Di.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    /// <summary>
    /// Extensions for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds ribbon menu from action.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="builder">The ribbon builder.</param>
        /// <param name="menuAssembly">Menu assembly.</param>
        public static void AddAutocadMenu(
            this IServiceCollection services,
            Action<IRibbonBuilder> builder,
            Assembly? menuAssembly = null)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            services.AddServices();
            services.AddMenu<AutocadRibbonMenuBuilder>(builder, menuAssembly);
        }

        /// <summary>
        /// Adds ribbon menu from config.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="cfg">Configuration.</param>
        /// <param name="menuAssembly">Menu assembly.</param>
        public static void AddAutocadMenu(
            this IServiceCollection services,
            IConfiguration? cfg = null,
            Assembly? menuAssembly = null)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            services.AddServices();
            services.AddMenu<AutocadRibbonMenuBuilder>(cfg, menuAssembly);
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.RegisterTypes<IItemStrategy>(Assembly.GetExecutingAssembly(), ServiceLifetime.Singleton)
                .AddSingleton<IOnlineHelpService, OnlineHelpService>()
                .AddSingleton<IRibbonEventsService, RibbonEventsService>()
                .AddSingleton<IColorThemeService, ColorThemeService>()
                .AddSingleton<ITabService, TabService>()
                .AddSingleton<IPanelService, PanelService>()
                .AddSingleton<IButtonService, ButtonService>()
                .AddSingleton<IRibbonComponentStorageService, RibbonComponentStorageService>();
        }
    }
}