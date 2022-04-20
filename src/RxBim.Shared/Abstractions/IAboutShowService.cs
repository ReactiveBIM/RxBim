namespace RxBim.Shared.Abstractions
{
    /// <summary>
    /// Abstraction for showing about window.
    /// </summary>
    public interface IAboutShowService
    {
        /// <summary>
        /// Shows about window.
        /// </summary>
        /// <param name="content">A content.</param>
        void ShowAboutBox(AboutBoxContent content);
    }
}
