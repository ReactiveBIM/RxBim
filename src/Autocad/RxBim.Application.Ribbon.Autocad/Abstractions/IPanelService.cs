namespace RxBim.Application.Ribbon
{
    using Autodesk.Windows;

    /// <summary>
    /// Ribbon panel service.
    /// </summary>
    public interface IPanelService
    {
        /// <summary>
        /// Creates and adds separator.
        /// </summary>
        /// <param name="panel">Panel.</param>
        void AddSeparator(RibbonPanel panel);

        /// <summary>
        /// Creates and adds slide-out.
        /// </summary>
        /// <param name="panel">Panel.</param>
        void AddSlideOut(RibbonPanel panel);

        /// <summary>
        /// Adds ribbon item to the panel
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <param name="item">Ribbon item</param>
        void AddItem(RibbonPanel panel, RibbonItem item);

        /// <summary>
        /// Returns a ribbon panel with the specified name on the tab. If the panel does not exist, it will be created.
        /// </summary>
        /// <param name="acRibbonTab">Ribbon tab.</param>
        /// <param name="panelName">Panel name.</param>
        RibbonPanel GetOrCreatePanel(RibbonTab acRibbonTab, string panelName);

        /// <summary>
        /// Deletes a panel.
        /// </summary>
        /// <param name="panel">The panel to be removed.</param>
        void DeletePanel(RibbonPanel panel);
    }
}