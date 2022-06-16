namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Represents a ribbon panel builder.
    /// </summary>
    public class PanelBuilder : IPanelBuilder
    {
        /// <summary>
        /// Initializes a new instance of the PanelBuilder class.
        /// </summary>
        /// <param name="name">Panel name.</param>
        public PanelBuilder(string name)
        {
            BuildingPanel.Name = name;
        }

        /// <summary>
        /// Building panel.
        /// </summary>
        public Panel BuildingPanel { get; } = new();

        /// <summary>
        /// Adds a new stacked items to the panel.
        /// </summary>
        /// <param name="builder">The stacked items builder.</param>
        public IPanelBuilder StackedItems(Action<IStackedItemsBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var stackedItems = new StackedItemsBuilder();
            builder.Invoke(stackedItems);
            BuildingPanel.Items.Add(stackedItems.StackedItems);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder CommandButton(
            string name,
            Type commandType,
            Action<ICommandButtonBuilder>? builder = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, commandType);
            builder?.Invoke(buttonBuilder);
            BuildingPanel.Items.Add(buttonBuilder.BuildingButton);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder PullDownButton(
            string name,
            Action<IPulldownButtonBuilder> builder)
        {
            var pulldownButton = new PulldownButtonBuilder(name);
            builder.Invoke(pulldownButton);
            BuildingPanel.Items.Add(pulldownButton.BuildingButton);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder Separator()
        {
            BuildingPanel.Items.Add(new PanelLayoutItem
            {
                LayoutItemType = PanelLayoutItemType.Separator
            });
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder SlideOut()
        {
            if (BuildingPanel.Items.Any(
                    e => e is PanelLayoutItem { LayoutItemType: PanelLayoutItemType.SlideOut }))
                throw new InvalidOperationException("The panel already contains SlideOut!");
            BuildingPanel.Items.Add(new PanelLayoutItem { LayoutItemType = PanelLayoutItemType.SlideOut });
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddItem(IRibbonPanelItem item)
        {
            BuildingPanel.Items.Add(item);
            return this;
        }

        /// <summary>
        /// Load from config.
        /// </summary>
        /// <param name="section">Config section.</param>
        /// <param name="fromConfigStrategies">Collection of <see cref="IItemFromConfigStrategy"/>.</param>
        internal void LoadFromConfig(
            IConfigurationSection section,
            IReadOnlyCollection<IItemFromConfigStrategy> fromConfigStrategies)
        {
            var itemsSection = section.GetSection(nameof(Panel.Items));
            if (!itemsSection.Exists())
                return;

            foreach (var itemSection in itemsSection.GetChildren())
            {
                var strategy = fromConfigStrategies.FirstOrDefault(x => x.IsApplicable(itemSection));
                strategy?.CreateAndAddToPanelConfig(itemSection, this);
            }
        }
    }
}