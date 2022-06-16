namespace RxBim.Application.Ribbon.Services.AddItemStrategies
{
    using System;
    using Autodesk.Windows;

    /// <summary>
    /// Basic implementation of <see cref="IAddItemStrategy"/> for AutoCAD menu item.
    /// </summary>
    public abstract class ItemStrategyBase<TItem> : IAddItemStrategy
        where TItem : IRibbonPanelItem
    {
        /// <inheritdoc />
        public virtual bool IsApplicable(IRibbonPanelItem config)
        {
            return config is TItem;
        }

        /// <inheritdoc />
        public void CreateAndAddItem(object panel, IRibbonPanelItem config)
        {
            if (panel is not RibbonPanel ribbonPanel || config is not TItem itemConfig)
                return;

            AddItem(ribbonPanel, itemConfig);
        }

        /// <inheritdoc />
        public object CreateItemForStack(IRibbonPanelItem config, bool small = false)
        {
            if (config is not TItem itemConfig)
                throw new InvalidOperationException($"Invalid config type: {config.GetType().FullName}");
            var size = small ? RibbonItemSize.Standard : RibbonItemSize.Large;

            return GetItemForStack(itemConfig, size);
        }

        /// <summary>
        /// Creates and adds to ribbon an item.
        /// </summary>
        /// <param name="ribbonPanel">Ribbon panel.</param>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        protected abstract void AddItem(RibbonPanel ribbonPanel, TItem itemConfig);

        /// <summary>
        /// Creates and returns an item for a stack.
        /// </summary>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        /// <param name="size">Item size.</param>
        protected abstract RibbonItem GetItemForStack(TItem itemConfig, RibbonItemSize size);

        /// <summary>
        /// Stub for CreateItemForStack, if item can't be stacked.
        /// </summary>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        protected RibbonItem CantBeStackedStub(TItem itemConfig)
        {
            throw new InvalidOperationException($"Can't be stacked: {itemConfig.GetType().FullName}");
        }
    }
}