namespace RxBim.Application.Ribbon.Services.AddElementStrategies
{
    using Autodesk.Windows;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for slide-out.
    /// </summary>
    public class SlideOutStrategy : ElementStrategyBase<PanelLayoutElement>
    {
        private readonly IPanelService _panelService;

        /// <inheritdoc />
        public SlideOutStrategy(IPanelService panelService)
        {
            _panelService = panelService;
        }

        /// <inheritdoc />
        public override bool IsApplicable(IRibbonPanelElement config)
        {
            return base.IsApplicable(config) &&
                   ((PanelLayoutElement)config).LayoutElementType == PanelLayoutElementType.SlideOut;
        }

        /// <inheritdoc />
        protected override void CreateAndAddElement(RibbonPanel ribbonPanel, PanelLayoutElement elementConfig)
        {
            _panelService.AddSlideOut(ribbonPanel);
        }

        /// <inheritdoc />
        protected override RibbonItem CreateElementForStack(PanelLayoutElement elementConfig, RibbonItemSize size)
        {
            return CantBeStackedStub(elementConfig);
        }
    }
}