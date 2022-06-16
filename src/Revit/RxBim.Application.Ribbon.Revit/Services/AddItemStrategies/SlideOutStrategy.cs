namespace RxBim.Application.Ribbon.Services.AddItemStrategies
{
    using Autodesk.Revit.UI;

    /// <summary>
    /// Implementation of <see cref="IAddItemStrategy"/> for slide-out.
    /// </summary>
    public class SlideOutStrategy : ItemStrategyBase<PanelLayoutItem>
    {
        /// <inheritdoc />
        public SlideOutStrategy(MenuData menuData)
            : base(menuData)
        {
        }

        /// <inheritdoc />
        public override bool IsApplicable(IRibbonPanelItem config)
        {
            return base.IsApplicable(config) &&
                   ((PanelLayoutItem)config).LayoutItemType == PanelLayoutItemType.SlideOut;
        }

        /// <inheritdoc />
        protected override void CreateAndAddItem(RibbonPanel ribbonPanel, PanelLayoutItem itemConfig)
        {
            ribbonPanel.AddSlideOut();
        }

        /// <inheritdoc />
        protected override RibbonItemData CreateItemForStack(PanelLayoutItem itemConfig)
        {
            return CannotBeStackedStub(itemConfig);
        }
    }
}