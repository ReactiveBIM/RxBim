namespace RxBim.Application.Ribbon.Models
{
    using System.Collections.Generic;
    using Abstractions;

    /// <summary>
    /// Ribbon panel configuration
    /// </summary>
    public class Panel : RibbonControl
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