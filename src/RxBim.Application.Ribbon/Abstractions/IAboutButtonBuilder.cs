namespace RxBim.Application.Ribbon.Abstractions
{
    using Shared;

    /// <summary>
    /// Button for displaying the About window
    /// </summary>
    public interface IAboutButtonBuilder : IButtonBuilder
    {
        /// <summary>
        /// Sets content for the About window
        /// </summary>
        /// <param name="content">About window content</param>
        IAboutButtonBuilder SetContent(AboutBoxContent content);
    }
}