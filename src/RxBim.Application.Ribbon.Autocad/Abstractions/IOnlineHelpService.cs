namespace RxBim.Application.Ribbon.Autocad.Abstractions
{
    using Autodesk.Windows;

    /// <summary>
    /// Online help processing service.
    /// </summary>
    public interface IOnlineHelpService
    {
        /// <summary>
        /// Starts the service.
        /// </summary>
        void Run();

        /// <summary>
        /// Adds a tracked tooltip.
        /// </summary>
        /// <param name="toolTip"><see cref="RibbonToolTip"/> object for tracking.</param>
        void AddToolTip(RibbonToolTip toolTip);

        /// <summary>
        /// Clears the tooltip cache.
        /// </summary>
        void ClearToolTipsCache();
    }
}