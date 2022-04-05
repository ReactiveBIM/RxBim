namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    using System;
    using Models.Configurations;

    /// <summary>
    /// Ribbon panel builder.
    /// </summary>
    public interface IPanelBuilder : IRibbonItemsContainerBuilder
    {
        /// <summary>
        /// Building panel
        /// </summary>
        Panel BuildingPanel { get; }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="commandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        IPanelBuilder AddCommandButton(
            string name,
            Type commandType,
            Action<IButtonBuilder>? action = null);

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
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        IPanelBuilder AddPullDownButton(string name, Action<IPulldownButtonBuilder> action);

        /// <summary>
        /// Creates and adds a separator to the panel
        /// </summary>
        IPanelBuilder AddSeparator();

        /// <summary>
        /// Creates and adds a switch for the sliding part of the panel
        /// </summary>
        IPanelBuilder AddSlideOut();

        /// <summary>
        /// Returns tab builder object
        /// </summary>
        ITabBuilder ReturnToTab();

        /// <summary>
        /// Adds a element to the panel.
        /// </summary>
        /// <param name="element">Element.</param>
        IPanelBuilder AddElement(IRibbonPanelElement element);
    }
}