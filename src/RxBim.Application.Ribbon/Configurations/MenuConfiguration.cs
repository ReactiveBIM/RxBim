namespace RxBim.Application.Ribbon.Configurations
{
    using System.Collections.Generic;

    /// <summary>
    /// Конфигурация меню плагина
    /// </summary>
    public class MenuConfiguration
    {
        /// <summary>
        /// Вкладки
        /// </summary>
        public List<TabConfiguration> Tabs { get; set; }
    }
}