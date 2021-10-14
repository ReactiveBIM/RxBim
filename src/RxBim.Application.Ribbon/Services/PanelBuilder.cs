namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Linq;
    using Abstractions;
    using Models;
    using Shared;

    /// <summary>
    /// Ribbon panel implementation
    /// </summary>
    public class PanelBuilder : RibbonControlBuilder<Panel>, IPanelBuilder
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
            Control.Name = name;
        }

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

            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddCommandButton(
            string name,
            string text,
            Type commandType,
            Action<ICommandButtonBuilder>? action = null)
        {
            var buttonBuilder = new CommandButtonBuilder(name, text, commandType);
            action?.Invoke(buttonBuilder);
            Control.Elements.Add(buttonBuilder.Control);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddPullDownButton(
            string name,
            string text,
            Action<IPulldownButtonBuilder> action)
        {
            var builder = new PulldownButtonBuilder(name, text);
            action.Invoke(builder);
            Control.Elements.Add(builder.Control);
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddSeparator()
        {
            Control.Elements.Add(new Separator());
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddSlideOut()
        {
            if (Control.Elements.Any(e => e is SlideOut))
                throw new InvalidOperationException("The panel already contains SlideOut!");
            Control.Elements.Add(new SlideOut());
            return this;
        }

        /// <inheritdoc />
        public IPanelBuilder AddAboutButton(
            string name,
            string text,
            AboutBoxContent content,
            Action<IButtonBuilder>? action = null)
        {
            var builder = new AboutButtonBuilder(name, text, content);
            action?.Invoke(builder);
            Control.Elements.Add(builder.Control);
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