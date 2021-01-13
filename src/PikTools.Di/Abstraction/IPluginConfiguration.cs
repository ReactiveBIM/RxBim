namespace PikTools.Di
{
    /// <summary>
    /// Конфигурация плагина
    /// </summary>
    public interface IPluginConfiguration
    {
        /// <summary>
        /// Дополнительные конфигурации контейнера
        /// </summary>
        /// <param name="container">Контейнер</param>
        void Configure(IContainer container);
    }
}