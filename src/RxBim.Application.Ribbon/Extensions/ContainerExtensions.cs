namespace RxBim.Application.Ribbon.Extensions
{
    using System;
    using System.Reflection;
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Models.Configurations;
    using Services;
    using Services.ConfigurationBuilders;
    using Shared.Abstractions;

    /// <summary>
    /// Contqins DI Container Extensions for Ribbon Menu.
    /// </summary>
    public static class ContainerExtensions
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
        public static void AddMenu<T>(
            this IContainer container,
            Action<IRibbonBuilder> builder,
            Assembly assembly)
            where T : class, IRibbonMenuBuilderFactory
        {
            container.AddBuilder<T>(assembly);
            container.AddSingleton(() =>
            {
                var ribbon = new RibbonBuilder();
                builder(ribbon);
                return ribbon.Ribbon;
            });
            container.DecorateContainer();
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
        public static void AddMenu<T>(
            this IContainer container,
            IConfiguration? config,
            Assembly assembly)
            where T : class, IRibbonMenuBuilderFactory
        {
            container.AddBuilder<T>(assembly);
            container.AddSingleton(() => GetMenuConfiguration(container, config));
            container.DecorateContainer();
        }

        /// <summary>
        /// Returns AboutShowService.
        /// </summary>
        /// <param name="container">DI container.</param>
        internal static IAboutShowService? TryGetAboutShowService(this IContainer container)
        {
            try
            {
                return container.GetService<IAboutShowService>();
            }
            catch
            {
                return null;
            }
        }

        private static void AddBuilder<T>(
            this IContainer container,
            Assembly assembly)
            where T : class, IRibbonMenuBuilderFactory
        {
            container.AddSingleton<IRibbonMenuBuilderFactory, T>();
            container.AddSingleton(() =>
            {
                var builderFactory = container.GetService<IRibbonMenuBuilderFactory>();
                return builderFactory.CreateMenuBuilder(assembly);
            });
        }

        private static void DecorateContainer(this IContainer container)
        {
            container.Decorate(typeof(IMethodCaller<>), typeof(MenuBuilderMethodCaller<>));
        }

        private static Ribbon GetMenuConfiguration(IContainer container, IConfiguration? cfg)
        {
            cfg ??= container.GetService<IConfiguration>();
            var builder = new RibbonBuilder();
            builder.LoadFromConfig(cfg);
            return builder.Ribbon;
        }
    }
}