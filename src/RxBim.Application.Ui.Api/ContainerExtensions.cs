namespace RxBim.Application.Ui.Api
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Abstractions;
    using Configurations;
    using Di;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Configuration;
    using Services;

    /// <summary>
    /// Расширения контейнера
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Добавляет меню приложения
        /// </summary>
        /// <param name="container">контейнер</param>
        /// <param name="action">метод создания меню</param>
        /// <param name="createOnlyOnce">Создавать только единожды</param>
        public static void AddMenu<TFactory, TBuildService>(
            this IContainer container,
            Action<IRibbon> action,
            bool createOnlyOnce)
            where TFactory : class, IRibbonFactory
            where TBuildService : class, IMenuBuildService
        {
            container.AddRibbonBuildTypes<TFactory, TBuildService>();

            var menuCreated = false;
            container.AddInstance<Action<IRibbon>>(ribbon =>
            {
                if (!menuCreated || !createOnlyOnce)
                {
                    action(ribbon);
                    menuCreated = true;
                }
            });

            container.DecorateContainer();
        }

        /// <summary>
        /// Добавляет меню приложения
        /// </summary>
        /// <param name="container">контейнер</param>
        /// <param name="assembly">сборка</param>
        /// <param name="cfg">конфигурация</param>
        /// <param name="createOnlyOnce">Создавать только единожды</param>
        public static void AddMenu<TFactory, TBuildService>(
            this IContainer container,
            Assembly assembly,
            IConfiguration cfg,
            bool createOnlyOnce)
            where TFactory : class, IRibbonFactory
            where TBuildService : class, IMenuBuildService
        {
            container.AddRibbonBuildTypes<TFactory, TBuildService>();

            var menuCreated = false;
            container.AddTransient<Action<IRibbon>>(() =>
            {
                var menuConfiguration = GetMenuConfiguration(container, cfg);

                return ribbon =>
                {
                    if (menuCreated && createOnlyOnce)
                        return;

                    menuConfiguration.Tabs
                        .ForEach(t =>
                        {
                            var tab = ribbon.Tab(t.Name);
                            t.Panels.ForEach(p =>
                            {
                                var panel = tab.Panel(p.Name);
                                //// todo доделать обработку других типов кнопок
                                p.Buttons.ForEach(b =>
                                {
                                    var commandType = GetType(b.CommandType, assembly);
                                    panel.Button(b.Name,
                                        b.Title,
                                        commandType,
                                        button => SetupButton(assembly, button, b));
                                });
                            });
                        });

                    menuCreated = true;
                };
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

        private static Type GetType(string commandType, [NotNull] Assembly assembly)
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

        private static void SetupButton(Assembly assembly, IButton button, ButtonConfiguration b)
        {
            button.SetLongDescription(b.Description);
            button.SetToolTip(b.ToolTip);

            if (TryGetImagePath(assembly, b.LargeImage, out var largeImagePath))
            {
                button.SetLargeImage(new Uri(largeImagePath, UriKind.RelativeOrAbsolute));
            }

            if (TryGetImagePath(assembly, b.SmallImage, out var smallImagePath))
            {
                button.SetSmallImage(new Uri(smallImagePath, UriKind.RelativeOrAbsolute));
            }
        }

        private static MenuConfiguration GetMenuConfiguration(IContainer container, IConfiguration cfg)
        {
            cfg ??= container.GetService<IConfiguration>();
            var menuConfiguration = cfg.GetSection("Menu").Get<MenuConfiguration>();
            return menuConfiguration;
        }

        private static bool TryGetImagePath(Assembly assembly, string imagePathFromConfig, out string path)
        {
            path = imagePathFromConfig;
            if (File.Exists(path))
                return true;

            path = Path.Combine(GetAssemblyDirectory(assembly), imagePathFromConfig);
            return File.Exists(path);
        }

        private static string GetAssemblyDirectory(Assembly assembly)
        {
            return Path.GetDirectoryName(assembly.Location);
        }
    }
}