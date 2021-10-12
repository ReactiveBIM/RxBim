namespace RxBim.Application.Ribbon.Abstractions
{
    using System;
    using Di;

    /// <summary>
    /// Ribbon panel
    /// </summary>
    public interface IPanel : IRibbonBuilder
    {
        /// <summary>
        /// The tab on which the panel is located
        /// </summary>
        ITab Tab { get; }

        /// <summary>
        /// Create new Stacked items at the panel
        /// </summary>
        /// <param name="action">Action where you must add items to the stacked panel</param>
        /// <returns>Panel where stacked items were created</returns>
        IPanel StackedItems(Action<IStackedItems> action);

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface.
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        IPanel Button(string name, string text, Type externalCommandType, Action<IButton> action = null);

        /// <summary>
        /// Create pull down button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        IPanel PullDownButton(string name, string text, Action<IPulldownButton> action);

        /// <summary>
        /// Create separator on the panel
        /// </summary>
        /// <returns></returns>
        IPanel Separator();

        /// <summary>
        /// Adds button for displaying About window
        /// </summary>
        /// <param name="name">Button name</param>
        /// <param name="text">Button label text</param>
        /// <param name="tabName">Name of the ribbon tab for the plugin</param>
        /// <param name="panelName">Panel name</param>
        /// <param name="container">DI container</param>
        /// <param name="action">Additional actions for the button</param>
        void AddAboutButton(
            string name,
            string text,
            string tabName,
            string panelName,
            IContainer container,
            Action<IAboutButton> action);
    }
}