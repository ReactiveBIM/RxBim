namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using Abstractions;
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

    /// <summary>
    /// Implementation of <see cref="IItemStrategy"/> for slide-out.
    /// </summary>
    public class SlideOutStrategy : ItemStrategyBase<PanelLayoutItem>
    {
        /// <inheritdoc />
        public override bool IsApplicable(IRibbonPanelItem item)
        {
            return base.IsApplicable(item) &&
                   ((PanelLayoutItem)item).LayoutItemType == PanelLayoutItemType.SlideOut;
        }

        /// <inheritdoc />
        protected override void AddItem(RibbonTab tab, RibbonPanel ribbonPanel, PanelLayoutItem itemConfig)
        {
            ribbonPanel.AddSlideOut();
        }

        /// <inheritdoc />
        protected override RibbonItemData GetItemForStack(PanelLayoutItem itemConfig)
        {
            return CantBeStackedStub(itemConfig);
        }
    }
}