namespace RxBim.Application.Ribbon
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a vertical stack of buttons on a ribbon.
    /// </summary>
    public class StackedItems : IRibbonPanelItem
    {
        /// <summary>
        /// The item list.
        /// </summary>
        public List<IRibbonPanelItem> Items { get; set; } = new();
    }
}