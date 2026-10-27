namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Represents a button configuration.
    /// </summary>
    public abstract class Button : RibbonPanelItemBase
    {
        /// <summary>
        /// Controls button text visibility. Null preserves automatic visibility.
        /// </summary>
        public bool? ShowText { get; set; }

        /// <summary>
        /// Optional content displayed by the registered renderer in an interactive tooltip.
        /// Replaces the native tooltip while the interactive tooltip is available.
        /// Null, empty or whitespace content preserves the native tooltip.
        /// </summary>
        public string? EnhancedToolTip { get; set; }

        /// <summary>
        /// The URI string for default large button image.
        /// </summary>
        public string? LargeImage { get; set; }

        /// <summary>
        /// The URI string for large button image for light theme.
        /// </summary>
        public string? LargeImageLight { get; set; }

        /// <summary>
        /// URI string for button image for light theme.
        /// </summary>
        public string? ImageLight { get; set; }

        /// <summary>
        /// The help url for the button.
        /// </summary>
        public string? HelpUrl
        {
            get;
            set => field = value.GetAbsoluteUrl();
        }

        /// <summary>
        /// Resolves the button image path for the specified color theme.
        /// </summary>
        /// <param name="themeType">User interface color theme.</param>
        public string? ResolveImagePath(ThemeType themeType)
        {
            return themeType is ThemeType.Light ? ImageLight ?? Image : Image;
        }

        /// <summary>
        /// Resolves the large button image path for the specified color theme.
        /// </summary>
        /// <param name="themeType">User interface color theme.</param>
        public string? ResolveLargeImagePath(ThemeType themeType)
        {
            return themeType is ThemeType.Light ? LargeImageLight ?? LargeImage : LargeImage;
        }
    }
}