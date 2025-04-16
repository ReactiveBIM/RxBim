namespace RxBim.Application.Ribbon
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a ribbon tab configuration.
    /// </summary>
    public class Tab
    {
        /// <summary>
        /// The tab name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The tab panel list.
        /// </summary>
        public List<IRibbonPanelItem> Items { get; set; } = new();
    }
}