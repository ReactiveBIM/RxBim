namespace RxBim.Application.Ribbon.Services.ElementFromConfigStrategies
{
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;
    using Models.Configurations;

    /// <summary>
    /// The strategy for getting a <see cref="StackedItems"/> from a configuration section.
    /// </summary>
    public class StackedItemsStrategy : IElementFromConfigStrategy
    {
        /// <inheritdoc />
        public bool IsApplicable(IConfigurationSection elementSection)
        {
            return elementSection.GetSection(nameof(StackedItems.StackedButtons)).Exists();
        }

        /// <inheritdoc />
        public void CreateFromConfigAndAdd(IConfigurationSection elementSection, IPanelBuilder panelBuilder)
        {
            var stackedButtons = elementSection.GetSection(nameof(StackedItems.StackedButtons));
            var stackedItems = new StackedItemsBuilder();
            stackedItems.LoadButtonsFromConfig(stackedButtons);
            panelBuilder.AddElement(stackedItems.StackedItems);
        }
    }
}