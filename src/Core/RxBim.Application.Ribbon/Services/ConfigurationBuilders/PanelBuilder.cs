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
        /// Building panel.
        /// </summary>
        private readonly Panel _panel = new();

        /// <summary>
        /// Initializes a new instance of the PanelBuilder class.
        /// </summary>
        /// <param name="name">Panel name.</param>
        public PanelBuilder(string name)
        {
            _panel.Name = name;
        }

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
            _panel.Items.Add(stackedItems.StackedItems);
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
            _panel.Items.Add(buttonBuilder.Build());
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder PullDownButton(
            string name,
            Action<IPulldownButtonBuilder> builder)
        {
            var pulldownButton = new PulldownButtonBuilder(name);
            builder.Invoke(pulldownButton);
            _panel.Items.Add(pulldownButton.Build());
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder ComboBox(string name, Action<IComboBoxBuilder> builder)
        {
            var combobox = new ComboBoxBuilder(name);
            builder.Invoke(combobox);
            _panel.Items.Add(combobox.Build());
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder Separator()
        {
            _panel.Items.Add(new PanelLayoutItem
            {
                LayoutItemType = PanelLayoutItemType.Separator
            });
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder SlideOut()
        {
            if (_panel.Items.Any(
                    e => e is PanelLayoutItem { LayoutItemType: PanelLayoutItemType.SlideOut }))
                throw new InvalidOperationException("The panel already contains SlideOut!");
            _panel.Items.Add(new PanelLayoutItem { LayoutItemType = PanelLayoutItemType.SlideOut });
            return this;
        }

        /// <inheritdoc />
        public void AddItem(IRibbonPanelItem item)
        {
            _panel.Items.Add(item);
        }

        /// <summary>
        /// Returns panel.
        /// </summary>
        internal Panel Build()
        {
            return _panel;
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