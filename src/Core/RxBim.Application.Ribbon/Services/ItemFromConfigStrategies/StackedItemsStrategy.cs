namespace RxBim.Application.Ribbon.ItemFromConfigStrategies
{
    using System;
    using System.Linq;
    using ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;
    using Shared.Abstractions;

    /// <summary>
    /// The strategy for getting a <see cref="StackedItems"/> from a configuration section.
    /// </summary>
    public class StackedItemsStrategy : IItemFromConfigStrategy
    {
        private readonly IDiCollectionService<IItemFromConfigStrategy> _strategiesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedItemsStrategy"/> class.
        /// </summary>
        /// <param name="strategiesService">Strategies service.</param>
        public StackedItemsStrategy(IDiCollectionService<IItemFromConfigStrategy> strategiesService)
        {
            _strategiesService = strategiesService;
        }

        /// <inheritdoc />
        public bool IsApplicable(IConfigurationSection itemSection)
        {
            return itemSection.GetSection(nameof(StackedItems.Items)).Exists();
        }

        /// <inheritdoc />
        public void CreateAndAddToPanelConfig(IConfigurationSection itemSection, IPanelBuilder panelBuilder)
        {
            var itemsSection = itemSection.GetSection(nameof(StackedItems.Items));
            if (!itemsSection.Exists())
                return;

            var stackedItems = new StackedItemsBuilder();

            var fromConfigStrategies = _strategiesService.GetItems().ToList();
            foreach (var child in itemsSection.GetChildren())
            {
                var strategy = fromConfigStrategies.FirstOrDefault(x => x.IsApplicable(child));
                if (strategy is null)
                    throw new InvalidOperationException($"Not found strategy for: {child.Value}");
                stackedItems.AddItem(strategy.CreateForStack(child));
            }

            panelBuilder.AddItem(stackedItems.StackedItems);
        }

        /// <inheritdoc />
        public IRibbonPanelItem CreateForStack(IConfigurationSection itemSection)
        {
            throw new InvalidOperationException("Can't be stacked!");
        }
    }
}