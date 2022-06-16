namespace RxBim.Application.Ribbon.AddElementStrategies
{
    /// <summary>
    /// Base implementation of <see cref="IAddElementStrategy"/> for slide-out.
    /// </summary>
    public abstract class SlideOutStrategyBase : IAddElementStrategy
    {
        /// <inheritdoc />
        public bool IsApplicable(IRibbonPanelElement config)
        {
            return config is PanelLayoutElement { LayoutElementType: PanelLayoutElementType.SlideOut };
        }

        /// <inheritdoc />
        public abstract void CreateElement(object panel, IRibbonPanelElement config);

        /// <inheritdoc />
        public abstract object CreateElementForStack(IRibbonPanelElement config, bool small);
    }
}