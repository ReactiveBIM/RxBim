namespace RxBim.Application.Ribbon.Models.Configurations
{
    using System.Collections.Generic;
    using Abstractions.ConfigurationBuilders;

    /// <summary>
    /// Ribbon panel configuration
    /// </summary>
    public class Panel
    {
        /// <summary>
        /// Panel name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Elements on the panel
        /// </summary>
        public List<IRibbonPanelElement> Elements { get; set; } = new ();
    }
}