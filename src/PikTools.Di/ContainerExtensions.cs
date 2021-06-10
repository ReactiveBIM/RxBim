namespace PikTools.Di
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Расширения контейнера для добавления конфигураций
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Добавляет конфигурацию в контейнер
        /// </summary>
        /// <param name="container">контейнер</param>
        /// <param name="action">функция настройки конфигурации</param>
        public static void AddConfiguration(
            this IContainer container,
            Func<ConfigurationBuilder, IConfiguration> action)
        {
            if (action != null)
            {
                container.AddInstance(action);
            }
        }
    }
}