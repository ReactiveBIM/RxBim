namespace RxBim.Application.Ribbon.Abstractions
{
    /// <summary>
    /// Ribbon
    /// </summary>
    public interface IRibbon
    {
        /// <summary>
        /// Ribbon is valid
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Creates or returns exists tab
        /// </summary>
        /// <param name="tabTitle">Tab title</param>
        ITab Tab(string tabTitle = null);
    }
}