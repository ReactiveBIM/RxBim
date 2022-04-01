namespace RxBim.Application.Ribbon.Autocad.Abstractions
{
    using Autodesk.Windows;

    /// <summary>
    /// Ribbon component storage service.
    /// </summary>
    public interface IRibbonComponentStorageService
    {
        /// <summary>
        /// Adds a tab to the storage.
        /// </summary>
        /// <param name="tab">Tab to add.</param>
        void AddTab(RibbonTab tab);

        /// <summary>
        /// Adds a panel to the storage.
        /// </summary>
        /// <param name="panel">Panel to add.</param>
        void AddPanel(RibbonPanel panel);

        /// <summary>
        /// Adds a item to the storage.
        /// </summary>
        /// <param name="item">Item to add.</param>
        /// <param name="collection">The collection to which the item is added.</param>
        void AddItem(RibbonItem item, RibbonItemCollection collection);

        /// <summary>
        /// Destroys and deletes all components in the storage.
        /// </summary>
        void DeleteComponents();
    }
}