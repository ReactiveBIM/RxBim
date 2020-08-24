namespace PikTools.Application.Ui.Api
{
    using System;
    using System.IO;
    using System.Reflection;
    using Builder;
    using Configurations;
    using Di;
    using Microsoft.Extensions.Configuration;
    using SimpleInjector;

    /// <summary>
    /// Расширения контейнера
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Добавляет меню риожения
        /// </summary>
        /// <param name="container">контейнер</param>
        /// <param name="action">метод создания меню</param>
        public static void AddMenu(this Container container, Action<Ribbon> action)
        {
            container.RegisterInstance(action);
            container.RegisterDecorator(typeof(IMethodCaller<>), typeof(MenuBuilderMethodCaller<>));
        }

        /// <summary>
        /// Добавляет меню риожения
        /// </summary>
        /// <param name="container">контейнер</param>
        /// <param name="cfg">конфигурация</param>
        /// <param name="assembly">сборка</param>
        public static void AddMenu(this Container container, IConfiguration cfg = null, Assembly assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();

            container.Register<Action<Ribbon>>(() =>
            {
                var menuConfiguration = GetMenuConfiguration(container, cfg);

                return ribbon =>
                {
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
                                    var commandType = assembly.GetType(b.CommandType);
                                    panel.Button(b.Name,
                                        b.Title,
                                        commandType,
                                        button => SetupButton(assembly, button, b));
                                });
                            });
                        });
                };
            });
            container.RegisterDecorator(typeof(IMethodCaller<>), typeof(MenuBuilderMethodCaller<>));
        }

        private static void SetupButton(Assembly assembly, Button button, ButtonConfiguration b)
        {
            button.SetLongDescription(b.Description);

            if (TryGetImagePath(assembly, b.LargeImage, out var largeImagePath))
            {
                button.SetLargeImage(largeImagePath);
            }

            if (TryGetImagePath(assembly, b.SmallImage, out var smallImagePath))
            {
                button.SetSmallImage(smallImagePath);
            }
        }

        private static MenuConfiguration GetMenuConfiguration(Container container, IConfiguration cfg)
        {
            cfg ??= container.GetInstance<IConfiguration>();
            var menuConfiguration = cfg.GetSection("Menu").Get<MenuConfiguration>();
            return menuConfiguration;
        }

        private static bool TryGetImagePath(Assembly assembly, string imagePathFromConfig, out string path)
        {
            path = imagePathFromConfig;
            if (File.Exists(path))
                return true;

            path = Path.Combine(Path.GetDirectoryName(assembly.Location), imagePathFromConfig);
            return File.Exists(path);
        }
    }
}