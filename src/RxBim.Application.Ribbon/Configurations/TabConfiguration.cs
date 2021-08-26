namespace RxBim.Application.Ribbon.Configurations
{
    using System.Collections.Generic;

    /// <summary>
    /// Конфигурация вкладки
    /// </summary>
    public class TabConfiguration
    {
        /// <summary>
        /// Имя вкладки
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Панели
        /// </summary>
        public List<PanelConfiguration> Panels { get; set; }
    }
}