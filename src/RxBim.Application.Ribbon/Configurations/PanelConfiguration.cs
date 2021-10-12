namespace RxBim.Application.Ribbon.Configurations
{
    using System.Collections.Generic;

    /// <summary>
    /// Ribbon panel configuration
    /// </summary>
    public class PanelConfiguration
    {
        /// <summary>
        /// Panel name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Buttons on the panel
        /// </summary>
        public List<ButtonConfiguration> Buttons { get; set; }
    }
}