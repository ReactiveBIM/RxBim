namespace RxBim.Application.Ribbon
{
    using System;
    using System.Reflection;
    using Abstractions;
    using Di;
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
        public static void AddRevitMenu(
            this IServiceCollection services,
            Action<IRibbonBuilder> builder,
            Assembly? menuAssembly = null)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            services
                .AddServices()
                .AddMenu<RevitRibbonMenuBuilder>(builder, menuAssembly);
        }

        /// <summary>
        /// Adds ribbon menu from config.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="cfg">Configuration.</param>
        /// <param name="menuAssembly">Menu assembly.</param>
        public static void AddRevitMenu(
            this IServiceCollection services,
            IConfiguration? cfg = null,
            Assembly? menuAssembly = null)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            services
                .AddServices()
                .AddMenu<RevitRibbonMenuBuilder>(cfg, menuAssembly);
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .RegisterItemStrategies(Assembly.GetExecutingAssembly())
                .AddSingleton<IRibbonPanelItemService, RibbonPanelItemService>();
        }
    }
}