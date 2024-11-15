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
        /// <param name="lazyRibbonCreation">
        /// True - create ribbon after CAD fully initialized, some of the functionality may be lost due to the late registration of the ribbon commands;
        /// False - create ribbon panel due application startup - some of the CAD services can be unavailable!</param>
        public static void AddRevitMenu(
            this IServiceCollection services,
            Action<IRibbonBuilder> builder,
            Assembly? menuAssembly = null,
            bool lazyRibbonCreation = true)
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
        /// <param name="lazyRibbonCreation">
        /// True - create ribbon after CAD fully initialized, some of the functionality may be lost due to the late registration of the ribbon commands;
        /// False - create ribbon panel due application startup - some of the CAD services can be unavailable!</param>
        public static void AddRevitMenu(
            this IServiceCollection services,
            IConfiguration? cfg = null,
            Assembly? menuAssembly = null,
            bool lazyRibbonCreation = true)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            services
                .AddServices()
                .AddMenu<RevitRibbonMenuBuilder>(cfg, menuAssembly);
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .Scan(scan => scan
                    .FromExecutingAssembly()
                    .AddClasses(classes => classes.AssignableTo<IItemStrategy>())
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime())
                .AddSingleton<IRibbonPanelItemService, RibbonPanelItemService>();
        }
    }
}