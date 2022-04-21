namespace RxBim.Application.Ribbon
{
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