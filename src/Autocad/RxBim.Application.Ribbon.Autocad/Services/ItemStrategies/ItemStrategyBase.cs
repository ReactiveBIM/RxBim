namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using System;
    using Autodesk.Windows;

    /// <summary>
    /// Basic implementation of <see cref="IItemStrategy"/> for AutoCAD menu item.
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
            if (tab is not RibbonTab ribbonTab || panel is not RibbonPanel ribbonPanel || item is not TItem itemConfig)
                return;

            AddItem(ribbonTab, ribbonPanel, itemConfig);
        }

        /// <inheritdoc />
        public object GetItemForStack(IRibbonPanelItem item, bool small = false)
        {
            if (item is not TItem itemConfig)
                throw new InvalidOperationException($"Invalid item type: {item.GetType().FullName}");
            var size = small ? RibbonItemSize.Standard : RibbonItemSize.Large;

            return GetItemForStack(itemConfig, size);
        }

        /// <summary>
        /// Creates and adds to ribbon an item.
        /// </summary>
        /// <param name="ribbonTab">Ribbon tab.</param>
        /// <param name="ribbonPanel">Ribbon panel.</param>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        // ReSharper disable once UnusedParameter.Global
        protected abstract void AddItem(RibbonTab ribbonTab, RibbonPanel ribbonPanel, TItem itemConfig);

        /// <summary>
        /// Creates and returns an item for a stack.
        /// </summary>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        /// <param name="size">Item size.</param>
        protected abstract RibbonItem GetItemForStack(TItem itemConfig, RibbonItemSize size);

        /// <summary>
        /// Stub for GetItemForStack, if item can't be stacked.
        /// </summary>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        protected RibbonItem CantBeStackedStub(TItem itemConfig)
        {
            throw new InvalidOperationException($"Can't be stacked: {itemConfig.GetType().FullName}");
        }
    }
}