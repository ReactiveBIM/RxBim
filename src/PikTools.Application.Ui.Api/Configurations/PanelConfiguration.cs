namespace PikTools.Application.Ui.Api.Configurations
{
    using System.Collections.Generic;

    /// <summary>
    /// Конфигурация панели
    /// </summary>
    public class PanelConfiguration
    {
        /// <summary>
        /// Имя панели
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Кнопки
        /// </summary>
        public List<ButtonConfiguration> Buttons { get; set; }
    }
}