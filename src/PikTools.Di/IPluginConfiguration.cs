namespace PikTools.Di
{
    using SimpleInjector;

    /// <summary>
    /// Конфигурация плагина
    /// </summary>
    public interface IPluginConfiguration
    {
        /// <summary>
        /// Дополнительные конфигурации контейнера
        /// </summary>
        /// <param name="container">Контейнер</param>
        void Configure(Container container);
    }
}