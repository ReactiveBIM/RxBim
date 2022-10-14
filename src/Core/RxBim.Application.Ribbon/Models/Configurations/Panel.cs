namespace RxBim.Application.Ribbon
{
    using System.Collections.Generic;

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
        /// The panel item list.
        /// </summary>
        public List<IRibbonPanelItem> Items { get; set; } = new();
    }
}