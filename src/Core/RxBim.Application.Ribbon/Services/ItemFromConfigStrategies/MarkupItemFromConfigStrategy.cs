namespace RxBim.Application.Ribbon.ItemFromConfigStrategies
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The strategy for getting a markup item from a configuration section.
    /// </summary>
    public abstract class MarkupItemFromConfigStrategy : IItemFromConfigStrategy
    {
        /// <summary>
        /// Item type.
        /// </summary>
        protected abstract PanelLayoutItemType ItemType { get; }

        /// <inheritdoc />
        public bool IsApplicable(IConfigurationSection itemSection)
        {
            var typeSection = itemSection.GetSection(nameof(PanelLayoutItem.LayoutItemType));
            if (!typeSection.Exists())
                return false;

            var type = typeSection.Get<PanelLayoutItemType>();
            return type == ItemType;
        }

        /// <inheritdoc />
        public void CreateAndAddToPanelConfig(IConfigurationSection itemSection, IPanelBuilder panelBuilder)
        {
            AddItem(panelBuilder);
        }

        /// <inheritdoc />
        public IRibbonPanelItem CreateForStack(IConfigurationSection itemSection)
        {
            throw new InvalidOperationException("Can't be stacked!");
        }

        /// <summary>
        /// Action for the item adding.
        /// </summary>
        /// <param name="panelBuilder">Panel builder.</param>
        protected abstract void AddItem(IPanelBuilder panelBuilder);
    }
}