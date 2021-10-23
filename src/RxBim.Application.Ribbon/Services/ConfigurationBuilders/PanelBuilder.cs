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
    /// Ribbon panel implementation
    /// </summary>
    public class PanelBuilder : IPanelBuilder
    {
        private readonly IRibbonBuilder _ribbonBuilder;
        private readonly ITabBuilder _tabBuilder;

        /// <summary>
        /// Initializes a new instance of the PanelBuilder class.
        /// </summary>
        /// <param name="name">Panel name</param>
        /// <param name="ribbonBuilder">Ribbon builder</param>
        /// <param name="tabBuilder">Tab builder</param>
        public PanelBuilder(string name, IRibbonBuilder ribbonBuilder, ITabBuilder tabBuilder)
        {
            _ribbonBuilder = ribbonBuilder;
            _tabBuilder = tabBuilder;
            BuildingPanel.Name = name;
        }

        /// <summary>
        /// Building panel
        /// </summary>
        public Panel BuildingPanel { get; } = new ();

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
            Action<ICommandButtonBuilder>? action = null)
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
            Action<IButtonBuilder>? action = null)
        {
            var builder = new AboutButtonBuilder(name, content);
            action?.Invoke(builder);
            BuildingPanel.Elements.Add(builder.BuildingButton);
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
        /// Load from config
        /// </summary>
        /// <param name="panelSection">Config section</param>
        internal void LoadFromConfig(IConfigurationSection panelSection)
        {
            var elementsSection = panelSection.GetSection(nameof(Panel.Elements));
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