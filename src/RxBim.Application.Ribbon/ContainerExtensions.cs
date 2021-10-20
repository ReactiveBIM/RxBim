namespace RxBim.Application.Ribbon
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Models.Configurations;
    using Services;
    using Services.ConfigurationBuilders;

    /// <summary>
    /// DI Container Extensions for Ribbon Menu
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds a plugin ribbon menu from an action
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="action">Action to create a menu</param>
        /// <param name="assembly">
        /// Menu definition assembly.
        /// Used to get the command type from the command type name
        /// and to define the root directory for relative icon paths
        /// </param>
        public static void AddMenu<T>(
            this IContainer container,
            Action<IRibbonBuilder> action,
            Assembly assembly)
            where T : class, IRibbonMenuBuilderFactory
        {
            container.AddBuilder<T>(assembly);
            container.AddSingleton(() =>
            {
                var builder = new RibbonBuilder();
                action(builder);
                return builder.Ribbon;
            });
            container.DecorateContainer();
        }

        /// <summary>
        /// Adds a plugin ribbon menu from configuration
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="config">Plugin configuration</param>
        /// <param name="assembly">
        /// Menu definition assembly.
        /// Used to get the command type from the command type name
        /// and to define the root directory for relative icon paths
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

        private static Type GetType(string commandType, Assembly assembly)
        {
            var strings = commandType.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();

            return strings.Length switch
            {
                1 => assembly.GetType(commandType), 2 => Assembly
                    .LoadFrom(Path.Combine(GetAssemblyDirectory(assembly), strings[1] + ".dll"))
                    .GetType(strings[0]),
                _ => throw new ArgumentException()
            };
        }

        private static Ribbon GetMenuConfiguration(IContainer container, IConfiguration? cfg)
        {
            cfg ??= container.GetService<IConfiguration>();
            var menuConfiguration = cfg.GetSection("Menu").Get<Ribbon>();
            return menuConfiguration;
        }

        private static string GetAssemblyDirectory(Assembly assembly)
        {
            return Path.GetDirectoryName(assembly.Location) !;
        }
    }
}