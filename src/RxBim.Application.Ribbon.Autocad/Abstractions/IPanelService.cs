namespace RxBim.Application.Ribbon.Autocad.Abstractions
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
        /// Returns a ribbon panel with the specified name on the tab.
        /// If the panel does not exist, it will be created.
        /// </summary>
        /// <param name="tab">Ribbon tab.</param>
        /// <param name="panelName">Panel name.</param>
        RibbonPanel GetOrCreatePanel(RibbonTab tab, string panelName);
    }
}