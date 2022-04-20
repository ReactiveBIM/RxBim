namespace RxBim.Application.Ribbon.Models.Configurations
{
    using System.Collections.Generic;
    using Abstractions.ConfigurationBuilders;

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