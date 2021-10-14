namespace RxBim.Application.Ribbon.Abstractions
{
    using System;
    using Shared;

    /// <summary>
    /// Ribbon panel
    /// </summary>
    public interface IPanelBuilder : IRibbonItemsContainerBuilder, IRibbonControlBuilder
    {
        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="commandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        IPanelBuilder AddCommandButton(
            string name,
            string text,
            Type commandType,
            Action<ICommandButtonBuilder>? action = null);

        /// <summary>
        /// Create new Stacked items at the panel
        /// </summary>
        /// <param name="action">Action where you must add items to the stacked panel</param>
        /// <returns>Panel where stacked items were created</returns>
        IPanelBuilder AddStackedItems(Action<IStackedItemsBuilder> action);

        /// <summary>
        /// Create pull down button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        IPanelBuilder AddPullDownButton(string name, string text, Action<IPulldownButtonBuilder> action);

        /// <summary>
        /// Creates and adds a separator to the panel
        /// </summary>
        IPanelBuilder AddSeparator();

        /// <summary>
        /// Creates and adds a switch for the sliding part of the panel
        /// </summary>
        IPanelBuilder AddSlideOut();

        /// <summary>
        /// Adds button for displaying About window
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="text">Button label text</param>
        /// <param name="content">About button content</param>
        /// <param name="action">Additional actions for the button</param>
        IPanelBuilder AddAboutButton(
            string name,
            string text,
            AboutBoxContent content,
            Action<IButtonBuilder>? action = null);

        /// <summary>
        /// Returns tab builder object
        /// </summary>
        ITabBuilder ToTabBuilder();
    }
}