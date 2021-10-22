namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using System;
    using System.Linq;
    using Abstractions.ConfigurationBuilders;
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
            {
                throw new ArgumentNullException(nameof(action));
            }

            var stackedItems = new StackedItemsBuilder();
            action.Invoke(stackedItems);

            if (!stackedItems.HasCorrectItemsCount)
            {
                throw new InvalidOperationException("StackedItems has incorrect items count!");
            }

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
            BuildingPanel.Elements.Add(new Separator());
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddSlideOut()
        {
            if (BuildingPanel.Elements.Any(e => e is SlideOut))
                throw new InvalidOperationException("The panel already contains SlideOut!");
            BuildingPanel.Elements.Add(new SlideOut());
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
        public ITabBuilder ToTabBuilder()
        {
            return _tabBuilder;
        }

        /// <inheritdoc />
        public IRibbonBuilder ToRibbonBuilder()
        {
            return _ribbonBuilder;
        }
    }
}