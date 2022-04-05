namespace RxBim.Application.Ribbon.Models.Configurations
{
    using Abstractions.ConfigurationBuilders;

    /// <summary>
    /// Ribbon panel layout element.
    /// </summary>
    public class PanelLayoutElement : IRibbonPanelElement
    {
        /// <summary>
        /// Layout element type.
        /// </summary>
        public PanelLayoutElementType LayoutElementType { get; set; }
    }
}