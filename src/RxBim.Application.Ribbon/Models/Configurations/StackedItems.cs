namespace RxBim.Application.Ribbon.Models.Configurations
{
    using System.Collections.Generic;
    using Abstractions.ConfigurationBuilders;

    /// <summary>
    /// Vertical stack of buttons on the ribbon
    /// </summary>
    public class StackedItems : IRibbonPanelElement
    {
        /// <summary>
        /// Stacked buttons
        /// </summary>
        public List<Button> StackedButtons { get; set; } = new ();
    }
}