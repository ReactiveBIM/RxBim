namespace RxBim.Application.Ribbon.Services.AddItemStrategies
{
    using Autodesk.Revit.UI;

    /// <summary>
    /// Implementation of <see cref="IAddItemStrategy"/> for separator.
    /// </summary>
    public class SeparatorStrategy : ItemStrategyBase<PanelLayoutItem>
    {
        /// <inheritdoc />
        public SeparatorStrategy(MenuData menuData)
            : base(menuData)
        {
        }

        /// <inheritdoc />
        public override bool IsApplicable(IRibbonPanelItem config)
        {
            return base.IsApplicable(config) &&
                   ((PanelLayoutItem)config).LayoutItemType == PanelLayoutItemType.Separator;
        }

        /// <inheritdoc />
        protected override void CreateAndAddItem(RibbonPanel ribbonPanel, PanelLayoutItem itemConfig)
        {
            ribbonPanel.AddSeparator();
        }

        /// <inheritdoc />
        protected override RibbonItemData CreateItemForStack(PanelLayoutItem itemConfig)
        {
            return CannotBeStackedStub(itemConfig);
        }
    }
}