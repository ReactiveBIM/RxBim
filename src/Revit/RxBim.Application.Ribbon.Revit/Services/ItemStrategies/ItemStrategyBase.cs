namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using System;
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using RibbonItem = Autodesk.Revit.UI.RibbonItem;
    using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

    /// <summary>
    /// Basic implementation of <see cref="IItemStrategy"/> for Revit menu item.
    /// </summary>
    public abstract class ItemStrategyBase<TItem> : IItemStrategy
    {
        /// <inheritdoc />
        public virtual bool IsApplicable(IRibbonPanelItem item)
        {
            return item is TItem;
        }

        /// <inheritdoc />
        public void AddItem(object tab, object panel, IRibbonPanelItem item)
        {
            if (tab is not RibbonTab ribbonTab || panel is not RibbonPanel ribbonPanel || item is not TItem itemConfig)
                return;

            AddItem(ribbonTab, ribbonPanel, itemConfig);
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
        /// <param name="tab">Ribbon tab.</param>
        /// <param name="ribbonPanel">Ribbon panel.</param>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        // ReSharper disable once UnusedParameter.Global
        protected abstract void AddItem(RibbonTab tab, RibbonPanel ribbonPanel, TItem itemConfig);

        /// <summary>
        /// Creates and returns an item for a stack.
        /// </summary>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        protected abstract RibbonItemData GetItemForStack(TItem itemConfig);

        /// <summary>
        /// Stub for GetItemForStack, if item can't be stacked.
        /// </summary>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        protected RibbonItemData CantBeStackedStub(TItem itemConfig)
        {
            throw new InvalidOperationException($"Can't be stacked: {itemConfig.GetType().FullName}");
        }
    }
}