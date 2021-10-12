namespace RxBim.Application.Ribbon.Abstractions
{
    /// <summary>
    /// Ribbon
    /// </summary>
    public interface IRibbon
    {
        /// <summary>
        /// Ribbon control is enabled
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Creates or returns exists tab
        /// </summary>
        /// <param name="tabTitle">Tab title</param>
        ITab Tab(string tabTitle = null);
    }
}