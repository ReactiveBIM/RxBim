namespace RxBim.Application.Ribbon
{
    /*
    public interface IButtonBuilderBase
    {
    }
    */
    
    /// <summary>
    /// Defines a ribbon button configuration builder.
    /// </summary>
    public interface IButtonBuilder<out TButtonBuilder> /////: IButtonBuilderBase
        where TButtonBuilder : class, IButtonBuilder<TButtonBuilder>
    {
        /// <summary>
        /// Sets the large image.
        /// </summary>
        /// <param name="imageRelativePath">A relative path to the image.</param>
        /// <param name="theme">The button color theme.</param>
        TButtonBuilder LargeImage(string imageRelativePath, ThemeType theme = ThemeType.All);

        /// <summary>
        /// Sets the large image.
        /// </summary>
        /// <param name="imageRelativePath">Image relative path.</param>
        /// <param name="theme">Color theme for image.</param>
        TButtonBuilder SmallImage(string imageRelativePath, ThemeType theme = ThemeType.All);

        /// <summary>
        /// Set the description text.
        /// </summary>
        /// <param name="description">Description text.</param>
        TButtonBuilder Description(string description);

        /// <summary>
        /// Sets the tooltip text.
        /// </summary>
        /// <param name="toolTip">Tooltip text.</param>
        TButtonBuilder ToolTip(string toolTip);

        /// <summary>
        /// Sets the label text.
        /// </summary>
        /// <param name="text">Button label text.</param>
        TButtonBuilder Text(string text);

        /// <summary>
        /// Sets the help URL.
        /// </summary>
        /// <param name="url">URL address.</param>
        TButtonBuilder HelpUrl(string url);
    }
}