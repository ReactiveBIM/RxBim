namespace RxBim.Application.Ribbon.ItemFromConfigStrategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The strategy for getting a <see cref="StackedItems"/> from a configuration section.
    /// </summary>
    public class StackedItemsFromConfigStrategy : IItemFromConfigStrategy
    {
        private readonly Lazy<List<IItemFromConfigStrategy>> _itemFromConfigStrategies;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedItemsFromConfigStrategy"/> class.
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> object.</param>
        public StackedItemsFromConfigStrategy(IServiceProvider serviceProvider)
        {
            _itemFromConfigStrategies = new Lazy<List<IItemFromConfigStrategy>>(() =>
                serviceProvider.GetServices<IItemFromConfigStrategy>().ToList());
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

            foreach (var child in itemsSection.GetChildren())
            {
                var strategy = _itemFromConfigStrategies.Value.FirstOrDefault(x => x.IsApplicable(child));
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