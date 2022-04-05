namespace RxBim.Application.Ribbon.Models.Configurations
{
    using Abstractions.ConfigurationBuilders;

    /// <summary>
    /// The Configuration for a button.
    /// </summary>
    public abstract class Button : IRibbonPanelElement
    {
        /// <summary>
        /// The button name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The button label text.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// URI string for default large button image.
        /// </summary>
        public string? LargeImage { get; set; }

        /// <summary>
        /// URI string for default small button image.
        /// </summary>
        public string? SmallImage { get; set; }

        /// <summary>
        /// URI string for large button image for light theme.
        /// </summary>
        public string? LargeImageLight { get; set; }

        /// <summary>
        /// URI string for small button image for light theme.
        /// </summary>
        public string? SmallImageLight { get; set; }

        /// <summary>
        /// The button description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The button tooltip.
        /// </summary>
        public string? ToolTip { get; set; }

        /// <summary>
        /// Help url for the button.
        /// </summary>
        public string? HelpUrl { get; set; }
    }
}