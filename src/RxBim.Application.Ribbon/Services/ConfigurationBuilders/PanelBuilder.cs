namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Models.Configurations;

    /// <inheritdoc />
    public class PanelBuilder : IPanelBuilder
    {
        private readonly ITabBuilder _tabBuilder;

        /// <summary>
        /// Initializes a new instance of the PanelBuilder class.
        /// </summary>
        /// <param name="name">Panel name</param>
        /// <param name="tabBuilder">Tab builder</param>
        public PanelBuilder(string name, ITabBuilder tabBuilder)
        {
            _tabBuilder = tabBuilder;
            BuildingPanel.Name = name;
        }

        /// <summary>
        /// Building panel
        /// </summary>
        public Panel BuildingPanel { get; } = new();

        /// <summary>
        /// Create new stacked items at the panel
        /// </summary>
        /// <param name="action">Action where you must add items to the stacked panel</param>
        /// <returns>Panel where stacked items were created</returns>
        public IPanelBuilder AddStackedItems(Action<IStackedItemsBuilder> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var stackedItems = new StackedItemsBuilder();
            action.Invoke(stackedItems);
            BuildingPanel.Elements.Add(stackedItems.StackedItems);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddCommandButton(
            string name,
            Type commandType,
            Action<IButtonBuilder>? action = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, commandType);
            action?.Invoke(buttonBuilder);
            BuildingPanel.Elements.Add(buttonBuilder.BuildingButton);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddPullDownButton(
            string name,
            Action<IPulldownButtonBuilder> action)
        {
            var builder = new PulldownButtonBuilder(name);
            action.Invoke(builder);
            BuildingPanel.Elements.Add(builder.BuildingButton);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddSeparator()
        {
            BuildingPanel.Elements.Add(new PanelLayoutElement
            {
                LayoutElementType = PanelLayoutElementType.Separator
            });
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddSlideOut()
        {
            if (BuildingPanel.Elements.Any(
                    e => e is PanelLayoutElement { LayoutElementType: PanelLayoutElementType.SlideOut }))
                throw new InvalidOperationException("The panel already contains SlideOut!");
            BuildingPanel.Elements.Add(new PanelLayoutElement { LayoutElementType = PanelLayoutElementType.SlideOut });
            return this;
        }

        /// <inheritdoc />
        public ITabBuilder ReturnToTab()
        {
            return _tabBuilder;
        }

        /// <inheritdoc />
        public IPanelBuilder AddElement(IRibbonPanelElement element)
        {
            BuildingPanel.Elements.Add(element);
            return this;
        }

        /// <inheritdoc />
        public IRibbonBuilder ReturnToRibbon()
        {
            return _tabBuilder.RibbonBuilder;
        }

        /// <summary>
        /// Load from config
        /// </summary>
        /// <param name="panelSection">Config section</param>
        /// <param name="fromConfigStrategies">Collection of <see cref="IElementFromConfigStrategy"/>.</param>
        internal void LoadFromConfig(
            IConfigurationSection panelSection,
            IReadOnlyCollection<IElementFromConfigStrategy> fromConfigStrategies)
        {
            var elementsSection = panelSection.GetSection(nameof(Panel.Elements));
            if (!elementsSection.Exists())
                return;

            foreach (var elementSection in elementsSection.GetChildren())
            {
                var strategy = fromConfigStrategies.FirstOrDefault(x => x.IsApplicable(elementSection));
                strategy?.CreateFromConfigAndAdd(elementSection, this);
            }
        }
    }
}