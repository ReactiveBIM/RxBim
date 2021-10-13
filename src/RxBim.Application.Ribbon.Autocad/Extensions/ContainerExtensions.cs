namespace RxBim.Application.Ribbon.Autocad.Extensions
{
    using System;
    using System.Reflection;
    using Abstractions;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Ribbon;
    using Ribbon.Abstractions;
    using Services;

    /// <summary>
    /// Расширения для контейнера
    /// </summary>
    public static class ContainerExtensions
    {
        private const bool CreateMenuOnlyOnce = false;

        /// <summary>
        /// Добавляет меню приложения
        /// </summary>
        /// <param name="container">контейнер</param>
        /// <param name="action">метод создания меню</param>
        public static void AddMenu(this IContainer container, Action<IRibbonBuilder> action)
        {
            container.AddInternalObjects();
            container.AddMenu<AutocadRibbonFactory, AutocadMenuBuildService>(action, CreateMenuOnlyOnce);
        }

        /// <summary>
        /// Добавляет меню приложения
        /// </summary>
        /// <param name="container">контейнер</param>
        /// <param name="cfg">конфигурация</param>
        /// <param name="assembly">сборка</param>
        public static void AddMenu(this IContainer container, IConfiguration? cfg = null, Assembly? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            container.AddInternalObjects();
            container.AddMenu<AutocadRibbonFactory, AutocadMenuBuildService>(assembly, cfg, CreateMenuOnlyOnce);
        }

        private static void AddInternalObjects(this IContainer container)
        {
            container.AddSingleton<IOnlineHelpService, OnlineHelpService>();
            container.AddSingleton<IRibbonEvents, RibbonEventService>();
        }
    }
}