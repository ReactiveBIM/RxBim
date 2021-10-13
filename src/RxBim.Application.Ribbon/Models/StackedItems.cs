namespace RxBim.Application.Ribbon.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Vertical stack of buttons on the ribbon
    /// </summary>
    public class StackedItems
    {
        /// <summary>
        /// Stacked buttons
        /// </summary>
        public List<Button> Buttons { get; set; }
    }
}