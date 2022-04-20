namespace RxBim.Application.Ribbon.Abstractions.ConfigurationBuilders
{
    using Models;

    /// <summary>
    /// Defines a ribbon button configuration builder.
    /// </summary>
    public interface IButtonBuilder
    {
        /// <summary>
        /// Sets the large image.
        /// </summary>
        /// <param name="imageRelativePath">A relative path to the image.</param>
        /// <param name="theme">The button color theme.</param>
        IButtonBuilder SetLargeImage(string imageRelativePath, ThemeType theme = ThemeType.All);

        /// <summary>
        /// Sets the large image.
        /// </summary>
        /// <param name="imageRelativePath">Image relative path.</param>
        /// <param name="theme">Color theme for image.</param>
        IButtonBuilder SetSmallImage(string imageRelativePath, ThemeType theme = ThemeType.All);

        /// <summary>
        /// Set the description text.
        /// </summary>
        /// <param name="description">Description text.</param>
        IButtonBuilder SetDescription(string description);

        /// <summary>
        /// Sets the tooltip text.
        /// </summary>
        /// <param name="toolTip">Tooltip text.</param>
        IButtonBuilder SetToolTip(string toolTip);

        /// <summary>
        /// Sets the label text.
        /// </summary>
        /// <param name="text">Button label text.</param>
        IButtonBuilder SetText(string text);

        /// <summary>
        /// Sets the help URL.
        /// </summary>
        /// <param name="url">URL address.</param>
        IButtonBuilder SetHelpUrl(string url);
    }
}