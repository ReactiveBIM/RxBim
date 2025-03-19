namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using Abstractions;
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

    /// <summary>
    /// Implementation of <see cref="IItemStrategy"/> for separator.
    /// </summary>
    public class SeparatorStrategy : ItemStrategyBase<PanelLayoutItem>
    {
        /// <inheritdoc />
        public override bool IsApplicable(IRibbonPanelItem item)
        {
            return base.IsApplicable(item) &&
                   ((PanelLayoutItem)item).LayoutItemType == PanelLayoutItemType.Separator;
        }

        /// <inheritdoc />
        protected override void AddItem(RibbonTab tab, RibbonPanel ribbonPanel, PanelLayoutItem itemConfig)
        {
            ribbonPanel.AddSeparator();
        }

        /// <inheritdoc />
        protected override RibbonItemData GetItemForStack(PanelLayoutItem itemConfig)
        {
            return CantBeStackedStub(itemConfig);
        }
    }
}