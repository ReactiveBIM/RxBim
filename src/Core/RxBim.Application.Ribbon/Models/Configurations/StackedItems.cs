namespace RxBim.Application.Ribbon
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a vertical stack of buttons on a ribbon.
    /// </summary>
    public class StackedItems : IRibbonPanelElement
    {
        /// <summary>
        /// The button list.
        /// </summary>
        public List<Button> StackedButtons { get; set; } = new();
    }
}