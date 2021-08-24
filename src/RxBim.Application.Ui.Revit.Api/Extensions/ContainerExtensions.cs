namespace RxBim.Application.Ui.Revit.Api.Extensions
{
    using System;
    using System.Reflection;
    using Di;
    using Microsoft.Extensions.Configuration;
    using RxBim.Application.Ui.Api;
    using RxBim.Application.Ui.Api.Abstractions;
    using Services;

    /// <summary>
    /// Расширения для контейнера
    /// </summary>
    public static class ContainerExtensions
    {
        private const bool CreateOnlyOnce = true;

        /// <summary>
        /// Добавляет меню приложения
        /// </summary>
        /// <param name="container">контейнер</param>
        /// <param name="action">метод создания меню</param>
        public static void AddMenu(this IContainer container, Action<IRibbon> action)
        {
            container.AddMenu<RevitRibbonFactory, RevitMenuBuildService>(action, CreateOnlyOnce);
        }

        /// <summary>
        /// Добавляет меню приложения
        /// </summary>
        /// <param name="container">контейнер</param>
        /// <param name="cfg">конфигурация</param>
        /// <param name="assembly">сборка</param>
        public static void AddMenu(this IContainer container, IConfiguration cfg = null, Assembly assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            container.AddMenu<RevitRibbonFactory, RevitMenuBuildService>(assembly, cfg, CreateOnlyOnce);
        }
    }
}