namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Strategy for adding item to the ribbon.
    /// </summary>
    public interface IItemStrategy
    {
        /// <summary>
        /// Returns true if the strategy is applicable for specified item configuration. Otherwise returns false.
        /// </summary>
        /// <param name="item">Ribbon item configuration.</param>
        bool IsApplicable(IRibbonPanelItem item);

        /// <summary>
        /// Creates an item.
        /// </summary>
        /// <param name="tab">Ribbon tab.</param>
        /// <param name="panel">Ribbon panel.</param>
        /// <param name="item">Ribbon item configuration.</param>
        void AddItem(object tab, object panel, IRibbonPanelItem item);

        /// <summary>
        /// Creates and returns an item for a stack.
        /// </summary>
        /// <param name="item">Ribbon item configuration.</param>
        /// <param name="small">Item is small.</param>
        object GetItemForStack(IRibbonPanelItem item, bool small = false);
    }
}