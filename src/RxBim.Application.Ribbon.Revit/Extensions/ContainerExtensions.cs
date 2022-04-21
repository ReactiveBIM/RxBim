namespace RxBim.Application.Ribbon
{
    using System;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using RxBim.Di;
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
        public static void AddRevitMenu(
            this IContainer container,
            Action<IRibbonBuilder> builder,
            Assembly? menuAssembly = null)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            container.AddMenu<RevitRibbonMenuBuilderFactory>(builder, menuAssembly);
        }

        /// <summary>
        /// Adds ribbon menu from config.
        /// </summary>
        /// <param name="container">DI container.</param>
        /// <param name="cfg">Configuration.</param>
        /// <param name="menuAssembly">Menu assembly.</param>
        public static void AddRevitMenu(
            this IContainer container,
            IConfiguration? cfg = null,
            Assembly? menuAssembly = null)
        {
            menuAssembly ??= Assembly.GetCallingAssembly();
            container.AddMenu<RevitRibbonMenuBuilderFactory>(cfg, menuAssembly);
        }
    }
}