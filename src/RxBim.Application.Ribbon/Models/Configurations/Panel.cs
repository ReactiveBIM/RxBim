namespace RxBim.Application.Ribbon.Models.Configurations
{
    using System.Collections.Generic;
    using Abstractions.ConfigurationBuilders;

    /// <summary>
    /// Represents a ribbon panel configuration.
    /// </summary>
    public class Panel
    {
        /// <summary>
        /// The panel name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The panel element list.
        /// </summary>
        public List<IRibbonPanelElement> Elements { get; set; } = new();
    }
}