namespace RxBim.Shared
{
    using System;

    /// <summary>
    /// Contains a command metadata.
    /// </summary>
    public class RxBimCommandAttribute : Attribute
    {
        /// <summary>
        /// The label text.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// The URI string for default large button image.
        /// </summary>
        public string? LargeImage { get; set; }

        /// <summary>
        /// The URI string for default button image.
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// The URI string for large button image for light theme.
        /// </summary>
        public string? LargeImageLight { get; set; }

        /// <summary>
        /// URI string for small button image for light theme.
        /// </summary>
        public string? SmallImageLight { get; set; }

        /// <summary>
        /// The description text.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The tooltip text.
        /// </summary>
        public string? ToolTip { get; set; }

        /// <summary>
        /// Optional Markdown content for an interactive ribbon tooltip.
        /// The native tooltip is used when this content is absent or empty.
        /// </summary>
        public string? EnhancedToolTip { get; set; }

        /// <summary>
        /// The help url for the button.
        /// </summary>
        public string? HelpUrl { get; set; }
    }
}