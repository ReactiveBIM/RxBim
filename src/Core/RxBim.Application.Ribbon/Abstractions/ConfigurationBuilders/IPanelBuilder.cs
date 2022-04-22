namespace RxBim.Application.Ribbon
{
    using System;
    using RxBim.Shared;

    /// <summary>
    /// Defines a ribbon panel.
    /// </summary>
    public interface IPanelBuilder : IRibbonItemsContainerBuilder
    {
        /// <summary>
        /// Adds a new push button to the panel.
        /// </summary>
        /// <param name="name">Internal name of the button.</param>
        /// <param name="commandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button.</param>
        /// <param name="builder">The button builder.</param>
        /// <returns>Panel where button were created.</returns>
        IPanelBuilder AddCommandButton(
            string name,
            Type commandType,
            Action<IButtonBuilder>? builder = null);

        /// <summary>
        /// Adds a new Stacked items on the panel.
        /// </summary>
        /// <param name="builder">The stacke items builder.</param>
        /// <returns>Panel where stacked items were created.</returns>
        IPanelBuilder AddStackedItems(Action<IStackedItemsBuilder> builder);

        /// <summary>
        /// Adds a new pull down button on the panel.
        /// </summary>
        /// <param name="name">Internal name of the button.</param>
        /// <param name="builder">A pull-down button builder.</param>
        /// <returns>Panel where button were created.</returns>
        IPanelBuilder AddPullDownButton(string name, Action<IPulldownButtonBuilder> builder);

        /// <summary>
        /// Adds a new separator to the panel.
        /// </summary>
        IPanelBuilder AddSeparator();

        /// <summary>
        /// Adds a new switch for the sliding part of the panel.
        /// </summary>
        IPanelBuilder AddSlideOut();

        /// <summary>
        /// Adds a new button for displaying the About window.
        /// </summary>
        /// <param name="name">Button name.</param>
        /// <param name="content">About button content.</param>
        /// <param name="builder">The button builder.</param>
        IPanelBuilder AddAboutButton(
            string name,
            AboutBoxContent content,
            Action<IButtonBuilder>? builder = null);

        /// <summary>
        /// Finishes the ribbon building.
        /// </summary>
        ITabBuilder ReturnToTab();
    }
}