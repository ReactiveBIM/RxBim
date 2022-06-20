namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using Abstractions;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Implementation of <see cref="IItemStrategy"/> for slide-out.
    /// </summary>
    public class SlideOutStrategy : ItemStrategyBase<PanelLayoutItem>
    {
        private readonly IRibbonPanelItemService _ribbonPanelItemService;

        /// <inheritdoc />
        public SlideOutStrategy(IRibbonPanelItemService ribbonPanelItemService)
        {
            _ribbonPanelItemService = ribbonPanelItemService;
        }

        /// <inheritdoc />
        public override bool IsApplicable(IRibbonPanelItem item)
        {
            return base.IsApplicable(item) &&
                   ((PanelLayoutItem)item).LayoutItemType == PanelLayoutItemType.SlideOut;
        }

        /// <inheritdoc />
        protected override void AddItem(string tabName, RibbonPanel ribbonPanel, PanelLayoutItem itemConfig)
        {
            ribbonPanel.AddSlideOut();
        }

        /// <inheritdoc />
        protected override RibbonItemData GetItemForStack(PanelLayoutItem itemConfig)
        {
            return _ribbonPanelItemService.CannotBeStackedStub(itemConfig);
        }
    }
}