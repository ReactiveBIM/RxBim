namespace RxBim.Application.Ribbon.Models
{
    using System.Collections.Generic;
    using Abstractions;

    /// <summary>
    /// Vertical stack of buttons on the ribbon
    /// </summary>
    public class StackedItems : IRibbonPanelElement
    {
        /// <summary>
        /// Stacked buttons
        /// </summary>
        public List<Button> Buttons { get; set; } = new ();
    }
}