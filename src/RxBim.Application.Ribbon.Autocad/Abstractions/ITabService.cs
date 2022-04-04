namespace RxBim.Application.Ribbon.Autocad.Abstractions
{
    using Autodesk.Windows;

    /// <summary>
    /// Ribbon tab service.
    /// </summary>
    public interface ITabService
    {
        /// <summary>
        /// Returns an existing tab with the specified name.
        /// </summary>
        /// <param name="tabName">Tab name.</param>
        RibbonTab? GetTab(string tabName);

        /// <summary>
        /// Creates and returns a new tab.
        /// </summary>
        /// <param name="tabName">Tab name.</param>
        RibbonTab CreateTab(string tabName);
    }
}