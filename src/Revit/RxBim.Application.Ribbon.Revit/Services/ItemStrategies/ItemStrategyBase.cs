namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using System;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Basic implementation of <see cref="IItemStrategy"/> for Revit menu item.
    /// </summary>
    public abstract class ItemStrategyBase<TItem> : IItemStrategy
        where TItem : IRibbonPanelItem
    {
        /// <inheritdoc />
        public virtual bool IsApplicable(IRibbonPanelItem item)
        {
            return item is TItem;
        }

        /// <inheritdoc />
        public void AddItem(object tab, object panel, IRibbonPanelItem item)
        {
            if (tab is not string tabName || panel is not RibbonPanel ribbonPanel || item is not TItem panelItem)
                return;

            AddItem(tabName, ribbonPanel, panelItem);
        }

        /// <inheritdoc />
        public object GetItemForStack(IRibbonPanelItem item, bool small = false)
        {
            if (item is not TItem panelItem)
                throw new InvalidOperationException($"Invalid item type: {item.GetType().FullName}");

            return GetItemForStack(panelItem);
        }

        /// <summary>
        /// Creates and adds to ribbon an item.
        /// </summary>
        /// <param name="tabName">Ribbon tab name.</param>
        /// <param name="ribbonPanel">Ribbon panel.</param>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        // ReSharper disable once UnusedParameter.Global
        protected abstract void AddItem(string tabName, RibbonPanel ribbonPanel, TItem itemConfig);

        /// <summary>
        /// Creates and returns an item for a stack.
        /// </summary>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        protected abstract RibbonItemData GetItemForStack(TItem itemConfig);
    }
}