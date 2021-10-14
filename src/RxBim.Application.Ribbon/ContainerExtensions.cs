namespace RxBim.Application.Ribbon
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Abstractions;
    using Di;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Services;

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
        /// <param name="createOnlyOnce">
        /// If true, the menu for the plugin on the ribbon is
        /// created only once when the plugin is loaded
        /// </param>
        public static void AddMenu<TFactory, TBuildService>(
            this IContainer container,
            Action<IRibbonBuilder> action,
            bool createOnlyOnce)
            where TFactory : class, IRibbonFactory
            where TBuildService : class, IMenuBuildService
        {
            container.AddRibbonBuildTypes<TFactory, TBuildService>();

            var menuWasCreated = false;
            container.AddInstance<Action<IRibbonBuilder>>(ribbon =>
            {
                if (!menuWasCreated || !createOnlyOnce)
                {
                    action(ribbon);
                    menuWasCreated = true;
                }
            });

            container.DecorateContainer();
        }

        /// <summary>
        /// Adds a plugin ribbon menu from config
        /// </summary>
        /// <param name="container">DI container</param>
        /// <param name="assembly">Plugin main assembly</param>
        /// <param name="config">Plugin config</param>
        /// <param name="createOnlyOnce">
        /// If true, the menu for the plugin on the ribbon is
        /// created only once when the plugin is loaded
        /// </param>
        public static void AddMenu<TFactory, TBuildService>(
            this IContainer container,
            Assembly assembly,
            IConfiguration config,
            bool createOnlyOnce)
            where TFactory : class, IRibbonFactory
            where TBuildService : class, IMenuBuildService
        {
            container.AddRibbonBuildTypes<TFactory, TBuildService>();
            container.AddTransient<Action<IRibbonBuilder>>(() =>
            {
                var menuConfiguration = GetMenuConfiguration(container, config);
            });

            container.DecorateContainer();
        }

        private static void AddRibbonBuildTypes<TFactory, TBuildService>(this IContainer container)
            where TFactory : class, IRibbonFactory
            where TBuildService : class, IMenuBuildService
        {
            container.AddTransient<IRibbonFactory, TFactory>();
            container.AddTransient<IMenuBuildService, TBuildService>();
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
                1 => assembly.GetType(commandType),
                2 => Assembly
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
            return Path.GetDirectoryName(assembly.Location);
        }
    }
}