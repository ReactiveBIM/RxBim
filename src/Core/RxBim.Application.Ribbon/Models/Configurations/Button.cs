namespace RxBim.Application.Ribbon
{
    /// <summary>
    /// Represents a button configuration.
    /// </summary>
    public abstract class Button : RibbonPanelItemBase
    {
        private string? _helpUrl;

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
            get => _helpUrl;
            set => _helpUrl = value.GetAbsoluteUrl();
        }
    }
}