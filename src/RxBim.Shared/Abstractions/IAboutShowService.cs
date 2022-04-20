namespace RxBim.Shared.Abstractions
{
    using System;

    /// <summary>
    /// Abstraction for showing about window.
    /// </summary>
    [Obsolete("Will be deleted at the next release!")]
    public interface IAboutShowService
    {
        /// <summary>
        /// Shows about window.
        /// </summary>
        /// <param name="content">A content.</param>
        void ShowAboutBox(AboutBoxContent content);
    }
}