namespace RxBim.Application.Ribbon.ItemFromConfigStrategies
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The strategy for getting a simple item from a configuration section.
    /// </summary>
    public abstract class SimpleItemFromConfigStrategyBase<T> : IItemFromConfigStrategy
        where T : IRibbonPanelItem
    {
        /// <inheritdoc />
        public abstract bool IsApplicable(IConfigurationSection itemSection);

        /// <inheritdoc />
        public void CreateAndAddToPanelConfig(
            IConfigurationSection itemSection,
            IPanelBuilder panelBuilder)
        {
            panelBuilder.AddItem(CreateForStack(itemSection));
        }

        /// <inheritdoc />
        public IRibbonPanelItem CreateForStack(IConfigurationSection itemSection)
        {
            return itemSection.Get<T>() is { } item
                ? item
                : throw new InvalidOperationException($"Could not read ribbon item from section '{itemSection.Path}'.");
        }
    }
}