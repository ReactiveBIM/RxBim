namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Defines a ribbon panel item configuration builder.
    /// </summary>
    public interface IRibbonPanelItemBuilder<out TItemBuilder>
        where TItemBuilder : class, IRibbonPanelItemBuilder<TItemBuilder>
    {
        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="imageRelativePath">Image relative path.</param>
        /// <param name="theme">Color theme for image.</param>
        TItemBuilder Image(string imageRelativePath, ThemeType theme = ThemeType.All);

        /// <summary>
        /// Set the description text.
        /// </summary>
        /// <param name="description">Description text.</param>
        TItemBuilder Description(string description);

        /// <summary>
        /// Sets the tooltip text.
        /// </summary>
        /// <param name="toolTip">Tooltip text.</param>
        TItemBuilder ToolTip(string toolTip);

        /// <summary>
        /// Sets the label text.
        /// </summary>
        /// <param name="text">Button label text.</param>
        TItemBuilder Text(string text);
    }
}