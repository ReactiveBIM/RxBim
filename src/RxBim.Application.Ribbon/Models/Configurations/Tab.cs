namespace RxBim.Application.Ribbon.Models.Configurations
{
    using System.Collections.Generic;

    /// <summary>
    /// Ribbon tab configuration.
    /// </summary>
    public class Tab
    {
        /// <summary>
        /// Tab name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Panels on the tab.
        /// </summary>
        public List<Panel> Panels { get; set; } = new();
    }
}