namespace RxBim.Application.Ribbon.ItemFromConfigStrategies
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The strategy for getting a simple item from a configuration section.
    /// </summary>
    public abstract class SimpleItemStrategyBase<T> : IItemFromConfigStrategy
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
            return itemSection.Get<T>();
        }
    }
}