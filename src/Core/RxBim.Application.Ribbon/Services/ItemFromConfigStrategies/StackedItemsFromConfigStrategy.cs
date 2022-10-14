namespace RxBim.Application.Ribbon.ItemFromConfigStrategies
{
    using System;
    using System.Linq;
    using ConfigurationBuilders;
    using Di;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The strategy for getting a <see cref="StackedItems"/> from a configuration section.
    /// </summary>
    public class StackedItemsFromConfigStrategy : IItemFromConfigStrategy
    {
        private readonly IServiceLocator _serviceLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedItemsFromConfigStrategy"/> class.
        /// </summary>
        /// <param name="serviceLocator"><see cref="IServiceLocator"/>.</param>
        public StackedItemsFromConfigStrategy(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
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

            var fromConfigStrategies = _serviceLocator.GetServicesAssignableTo<IItemFromConfigStrategy>().ToList();
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