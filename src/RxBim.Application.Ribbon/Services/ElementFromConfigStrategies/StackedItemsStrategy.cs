namespace RxBim.Application.Ribbon.Services.ElementFromConfigStrategies
{
    using System;
    using System.Linq;
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;
    using Models.Configurations;
    using Shared.Abstractions;

    /// <summary>
    /// The strategy for getting a <see cref="StackedItems"/> from a configuration section.
    /// </summary>
    public class StackedItemsStrategy : IElementFromConfigStrategy
    {
        private readonly IDiCollectionService<IElementFromConfigStrategy> _strategiesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedItemsStrategy"/> class.
        /// </summary>
        /// <param name="strategiesService">Strategies service.</param>
        public StackedItemsStrategy(IDiCollectionService<IElementFromConfigStrategy> strategiesService)
        {
            _strategiesService = strategiesService;
        }

        /// <inheritdoc />
        public bool IsApplicable(IConfigurationSection elementSection)
        {
            return elementSection.GetSection(nameof(StackedItems.StackedElements)).Exists();
        }

        /// <inheritdoc />
        public void CreateAndAddToPanelConfig(IConfigurationSection elementSection, IPanelBuilder panelBuilder)
        {
            var stackedElements = elementSection.GetSection(nameof(StackedItems.StackedElements));
            if (!stackedElements.Exists())
                return;

            var stackedItems = new StackedItemsBuilder();

            var fromConfigStrategies = _strategiesService.GetItems().ToList();
            foreach (var child in stackedElements.GetChildren())
            {
                var strategy = fromConfigStrategies.FirstOrDefault(x => x.IsApplicable(child));
                if (strategy is null)
                    throw new InvalidOperationException($"Not found strategy for: {child.Value}");
                stackedItems.AddElement(strategy.CreateForStack(child));
            }

            panelBuilder.AddElement(stackedItems.StackedItems);
        }

        /// <inheritdoc />
        public IRibbonPanelElement CreateForStack(IConfigurationSection elementSection)
        {
            throw new InvalidOperationException("Can't be stacked!");
        }
    }
}