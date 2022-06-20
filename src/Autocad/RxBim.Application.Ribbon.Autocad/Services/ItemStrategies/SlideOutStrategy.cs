namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using Autodesk.Windows;

    /// <summary>
    /// Implementation of <see cref="IItemStrategy"/> for slide-out.
    /// </summary>
    public class SlideOutStrategy : ItemStrategyBase<PanelLayoutItem>
    {
        private readonly IPanelService _panelService;

        /// <inheritdoc />
        public SlideOutStrategy(IPanelService panelService)
        {
            _panelService = panelService;
        }

        /// <inheritdoc />
        public override bool IsApplicable(IRibbonPanelItem config)
        {
            return base.IsApplicable(config) &&
                   ((PanelLayoutItem)config).LayoutItemType == PanelLayoutItemType.SlideOut;
        }

        /// <inheritdoc />
        protected override void AddItem(RibbonTab ribbonTab, RibbonPanel ribbonPanel, PanelLayoutItem itemConfig)
        {
            _panelService.AddSlideOut(ribbonPanel);
        }

        /// <inheritdoc />
        protected override RibbonItem GetItemForStack(PanelLayoutItem itemConfig, RibbonItemSize size)
        {
            return CantBeStackedStub(itemConfig);
        }
    }
}