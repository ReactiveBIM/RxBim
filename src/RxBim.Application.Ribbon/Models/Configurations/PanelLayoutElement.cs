namespace RxBim.Application.Ribbon.Models.Configurations
{
    using Abstractions.ConfigurationBuilders;

    /// <summary>
    /// Represents a ribbon panel layout element.
    /// </summary>
    public class PanelLayoutElement : IRibbonPanelElement
    {
        /// <summary>
        /// The layout element type.
        /// </summary>
        public PanelLayoutElementType LayoutElementType { get; set; }
    }
}