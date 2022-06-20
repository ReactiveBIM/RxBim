namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using Abstractions;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Implementation of <see cref="IItemStrategy"/> for separator.
    /// </summary>
    public class SeparatorStrategy : ItemStrategyBase<PanelLayoutItem>
    {
        private readonly IRibbonPanelItemService _ribbonPanelItemService;

        /// <inheritdoc />
        public SeparatorStrategy(IRibbonPanelItemService ribbonPanelItemService)
        {
            _ribbonPanelItemService = ribbonPanelItemService;
        }

        /// <inheritdoc />
        public override bool IsApplicable(IRibbonPanelItem item)
        {
            return base.IsApplicable(item) &&
                   ((PanelLayoutItem)item).LayoutItemType == PanelLayoutItemType.Separator;
        }

        /// <inheritdoc />
        protected override void AddItem(string tabName, RibbonPanel ribbonPanel, PanelLayoutItem itemConfig)
        {
            ribbonPanel.AddSeparator();
        }

        /// <inheritdoc />
        protected override RibbonItemData GetItemForStack(PanelLayoutItem itemConfig)
        {
            return _ribbonPanelItemService.CannotBeStackedStub(itemConfig);
        }
    }
}