namespace RxBim.Application.Ribbon.Abstractions
{
    using Models;

    /// <summary>
    /// Ribbon button
    /// </summary>
    public interface IButtonBuilder
    {
        /// <summary>
        /// Sets a large image for the button
        /// </summary>
        /// <param name="imageRelativePath">Image relative path</param>
        /// <param name="theme">Color theme for image</param>
        IButtonBuilder SetLargeImage(string imageRelativePath, ThemeType theme = ThemeType.All);

        /// <summary>
        /// Sets a small image for the button
        /// </summary>
        /// <param name="imageRelativePath">Image relative path</param>
        /// <param name="theme">Color theme for image</param>
        IButtonBuilder SetSmallImage(string imageRelativePath, ThemeType theme = ThemeType.All);

        /// <summary>
        /// Set description for the button
        /// </summary>
        /// <param name="description">Description text</param>
        IButtonBuilder SetDescription(string description);
    }
}