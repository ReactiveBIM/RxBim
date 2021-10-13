namespace RxBim.Application.Ribbon.Abstractions
{
    /// <summary>
    /// Ribbon
    /// </summary>
    public interface IRibbonBuilder
    {
        /// <summary>
        /// Ribbon control is enabled
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Creates or returns exists tab
        /// </summary>
        /// <param name="tabTitle">Tab title</param>
        ITabBuilder Tab(string tabTitle = null);
    }
}