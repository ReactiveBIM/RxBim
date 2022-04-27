namespace RxBim.Application.Ribbon
{
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
        IButtonBuilder LargeImage(string imageRelativePath, ThemeType theme = ThemeType.All);

        /// <summary>
        /// Sets the large image.
        /// </summary>
        /// <param name="imageRelativePath">Image relative path.</param>
        /// <param name="theme">Color theme for image.</param>
        IButtonBuilder SmallImage(string imageRelativePath, ThemeType theme = ThemeType.All);

        /// <summary>
        /// Set the description text.
        /// </summary>
        /// <param name="description">Description text.</param>
        IButtonBuilder Description(string description);

        /// <summary>
        /// Sets the tooltip text.
        /// </summary>
        /// <param name="toolTip">Tooltip text.</param>
        IButtonBuilder ToolTip(string toolTip);

        /// <summary>
        /// Sets the label text.
        /// </summary>
        /// <param name="text">Button label text.</param>
        IButtonBuilder Text(string text);

        /// <summary>
        /// Sets the help URL.
        /// </summary>
        /// <param name="url">URL address.</param>
        IButtonBuilder HelpUrl(string url);
    }
}