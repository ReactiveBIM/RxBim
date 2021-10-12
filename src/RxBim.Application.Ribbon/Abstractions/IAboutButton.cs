namespace RxBim.Application.Ribbon.Abstractions
{
    using Shared;

    /// <summary>
    /// Button for displaying the About window
    /// </summary>
    public interface IAboutButton : IButton
    {
        /// <summary>
        /// Sets content for the About window
        /// </summary>
        /// <param name="content">About window content</param>
        IAboutButton SetContent(AboutBoxContent content);
    }
}