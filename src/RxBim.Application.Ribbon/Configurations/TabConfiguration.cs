namespace RxBim.Application.Ribbon.Configurations
{
    using System.Collections.Generic;

    /// <summary>
    /// Ribbon tab configuration
    /// </summary>
    public class TabConfiguration
    {
        /// <summary>
        /// Tab name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Panels on the tab
        /// </summary>
        public List<PanelConfiguration> Panels { get; set; }
    }
}