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
            BuildingPanel.Elements.Add(stackedItems.StackedItems);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder CommandButton(
            string name,
            Type commandType,
            Action<ICommnadButtonBuilder>? builder = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, commandType);
            builder?.Invoke(buttonBuilder);
            BuildingPanel.Elements.Add(buttonBuilder.BuildingButton);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder PullDownButton(
            string name,
            Action<IPulldownButtonBuilder> builder)
        {
            var pulldownButton = new PulldownButtonBuilder(name);
            builder.Invoke(pulldownButton);
            BuildingPanel.Elements.Add(pulldownButton.BuildingButton);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder Separator()
        {
            BuildingPanel.Elements.Add(new PanelLayoutElement
            {
                LayoutElementType = PanelLayoutElementType.Separator
            });
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder SlideOut()
        {
            if (BuildingPanel.Elements.Any(
                    e => e is PanelLayoutElement { LayoutElementType: PanelLayoutElementType.SlideOut }))
                throw new InvalidOperationException("The panel already contains SlideOut!");
            BuildingPanel.Elements.Add(new PanelLayoutElement { LayoutElementType = PanelLayoutElementType.SlideOut });
            return this;
        }

        /// <summary>
        /// Load from config.
        /// </summary>
        /// <param name="section">Config section.</param>
        /// <param name="fromConfigStrategies">Collection of <see cref="IElementFromConfigStrategy"/>.</param>
        internal void LoadFromConfig(
            IConfigurationSection section,
            IEnumerable<IElementFromConfigStrategy> fromConfigStrategies)
        {
            var elementsSection = section.GetSection(nameof(Panel.Elements));
            if (!elementsSection.Exists())
                return;

            foreach (var elementSection in elementsSection.GetChildren())
            {
                // Stacked Items
                var stackedItemsSection =
                    elementSection.GetSection(nameof(Application.Ribbon.StackedItems.StackedButtons));
                if (stackedItemsSection.Exists())
                {
                    var stackedItems = new StackedItemsBuilder();
                    stackedItems.LoadFromConfig(stackedItemsSection);
                    BuildingPanel.Elements.Add(stackedItems.StackedItems);
                }
                else if (elementSection.GetSection(nameof(Application.Ribbon.CommandButton.CommandType)).Exists())
                {
                    // Command Button
                    var button = LoadFromConfig<CommandButton>(elementSection);
                }
                else if (elementSection.GetSection(nameof(Application.Ribbon.PullDownButton.CommandButtonsList))
                         .Exists())
                {
                    // Pulldown
                    LoadFromConfig<PullDownButton>(elementSection);
                }
                else if (elementSection.GetSection(nameof(AboutButton.Content)).Exists())
                {
                    // About
                    LoadFromConfig<AboutButton>(elementSection);
                }
                else
                {
                    // Layout element
                    var typeSection = elementSection.GetSection(nameof(PanelLayoutElement.LayoutElementType));
                    if (typeSection.Exists())
                    {
                        var type = typeSection.Get<PanelLayoutElementType>();
                        switch (type)
                        {
                            case PanelLayoutElementType.Separator:
                                Separator();
                                break;
                            case PanelLayoutElementType.SlideOut:
                                SlideOut();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException($"Unknown panel layout element type: {type}");
                        }
                    }
                }
            }
        }

        private T LoadFromConfig<T>(IConfigurationSection elementSection)
            where T : IRibbonPanelElement
        {
            var element = elementSection.Get<T>();
            BuildingPanel.Elements.Add(element);
            return element;
        }
    }
}