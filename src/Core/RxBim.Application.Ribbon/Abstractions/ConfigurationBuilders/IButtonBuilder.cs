namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Defines a ribbon button configuration builder.
    /// </summary>
    public interface IButtonBuilder<out TButtonBuilder> : IRibbonPanelItemBuilder<TButtonBuilder>
        where TButtonBuilder : class, IButtonBuilder<TButtonBuilder>
    {
        /// <summary>
        /// Sets the large image.
        /// </summary>
        /// <param name="imageRelativePath">A relative path to the image.</param>
        /// <param name="theme">The button color theme.</param>
        TButtonBuilder LargeImage(string imageRelativePath, ThemeType theme = ThemeType.All);

        /// <summary>
        /// Sets the help URL.
        /// </summary>
        /// <param name="url">URL address.</param>
        TButtonBuilder HelpUrl(string url);
    }
}