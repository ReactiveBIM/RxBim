namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using System;
    using System.Linq;
    using Abstractions.ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Models.Configurations;
    using Shared;

    /// <summary>
    /// Represents a ribbon panel builder.
    /// </summary>
    public class PanelBuilder : IPanelBuilder
    {
        private readonly IRibbonBuilder _ribbonBuilder;
        private readonly ITabBuilder _tabBuilder;

        /// <summary>
        /// Initializes a new instance of the PanelBuilder class.
        /// </summary>
        /// <param name="name">Panel name.</param>
        /// <param name="ribbonBuilder">Ribbon builder.</param>
        /// <param name="tabBuilder">Tab builder.</param>
        public PanelBuilder(string name, IRibbonBuilder ribbonBuilder, ITabBuilder tabBuilder)
        {
            _ribbonBuilder = ribbonBuilder;
            _tabBuilder = tabBuilder;
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
        public IPanelBuilder AddStackedItems(Action<IStackedItemsBuilder> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var stackedItems = new StackedItemsBuilder();
            builder.Invoke(stackedItems);
            BuildingPanel.Elements.Add(stackedItems.StackedItems);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddCommandButton(
            string name,
            Type commandType,
            Action<IButtonBuilder>? builder = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, commandType);
            builder?.Invoke(buttonBuilder);
            BuildingPanel.Elements.Add(buttonBuilder.BuildingButton);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddPullDownButton(
            string name,
            Action<IPulldownButtonBuilder> builder)
        {
            var pulldownButton = new PulldownButtonBuilder(name);
            builder.Invoke(pulldownButton);
            BuildingPanel.Elements.Add(pulldownButton.BuildingButton);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddSeparator()
        {
            BuildingPanel.Elements.Add(new PanelLayoutElement { LayoutElementType = PanelLayoutElementType.Separator });
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
        public IPanelBuilder AddAboutButton(
            string name,
            AboutBoxContent content,
            Action<IButtonBuilder>? builder = null)
        {
            var aboutButton = new AboutButtonBuilder(name, content);
            builder?.Invoke(aboutButton);
            BuildingPanel.Elements.Add(aboutButton.BuildingButton);
            return this;
        }

        /// <inheritdoc />
        public ITabBuilder ReturnToTab()
        {
            return _tabBuilder;
        }

        /// <inheritdoc />
        public IRibbonBuilder ReturnToRibbon()
        {
            return _ribbonBuilder;
        }

        /// <summary>
        /// Load from config.
        /// </summary>
        /// <param name="section">Config section.</param>
        internal void LoadFromConfig(IConfigurationSection section)
        {
            var elementsSection = section.GetSection(nameof(Panel.Elements));
            if (!elementsSection.Exists())
                return;

            foreach (var elementSection in elementsSection.GetChildren())
            {
                var stackedButtons = elementSection.GetSection(nameof(StackedItems.StackedButtons));
                if (stackedButtons.Exists())
                {
                    var stackedItems = new StackedItemsBuilder();
                    stackedItems.LoadButtonsFromConfig(stackedButtons);
                    BuildingPanel.Elements.Add(stackedItems.StackedItems);
                }
                else if (elementSection.GetSection(nameof(CommandButton.CommandType)).Exists())
                {
                    CreateFromConfigAndAdd<CommandButton>(elementSection);
                }
                else if (elementSection.GetSection(nameof(PullDownButton.CommandButtonsList)).Exists())
                {
                    CreateFromConfigAndAdd<PullDownButton>(elementSection);
                }
                else if (elementSection.GetSection(nameof(AboutButton.Content)).Exists())
                {
                    CreateFromConfigAndAdd<AboutButton>(elementSection);
                }
                else
                {
                    var typeSection = elementSection.GetSection(nameof(PanelLayoutElement.LayoutElementType));
                    if (typeSection.Exists())
                    {
                        var type = typeSection.Get<PanelLayoutElementType>();
                        switch (type)
                        {
                            case PanelLayoutElementType.Separator:
                                AddSeparator();
                                break;
                            case PanelLayoutElementType.SlideOut:
                                AddSlideOut();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException($"Unknown panel layout element type: {type}");
                        }
                    }
                }
            }
        }

        private void CreateFromConfigAndAdd<T>(IConfigurationSection elementSection)
            where T : IRibbonPanelElement
        {
            var button = elementSection.Get<T>();
            BuildingPanel.Elements.Add(button);
        }
    }
}