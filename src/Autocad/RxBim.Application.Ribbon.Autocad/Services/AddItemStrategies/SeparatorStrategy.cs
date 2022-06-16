namespace RxBim.Application.Ribbon.Services.AddItemStrategies
{
    using Autodesk.Windows;

    /// <summary>
    /// Implementation of <see cref="IAddItemStrategy"/> for separator.
    /// </summary>
    public class SeparatorStrategy : ItemStrategyBase<PanelLayoutItem>
    {
        private readonly IPanelService _panelService;

        /// <inheritdoc />
        public SeparatorStrategy(IPanelService panelService)
        {
            _panelService = panelService;
        }

        /// <inheritdoc />
        public override bool IsApplicable(IRibbonPanelItem config)
        {
            return base.IsApplicable(config) &&
                   ((PanelLayoutItem)config).LayoutItemType == PanelLayoutItemType.Separator;
        }

        /// <inheritdoc />
        protected override void AddItem(RibbonTab ribbonTab, RibbonPanel ribbonPanel, PanelLayoutItem itemConfig)
        {
            _panelService.AddSeparator(ribbonPanel);
        }

        /// <inheritdoc />
        protected override RibbonItem GetItemForStack(PanelLayoutItem itemConfig, RibbonItemSize size)
        {
            return CantBeStackedStub(itemConfig);
        }
    }
}