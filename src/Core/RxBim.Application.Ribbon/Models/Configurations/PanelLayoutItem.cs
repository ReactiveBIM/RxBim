namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Represents a ribbon panel layout item.
    /// </summary>
    public class PanelLayoutItem : IRibbonPanelItem
    {
        /// <summary>
        /// The layout item type.
        /// </summary>
        public PanelLayoutItemType LayoutItemType { get; set; }
    }
}