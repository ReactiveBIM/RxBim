namespace RxBim.Application.Ribbon.Revit.Services.AddElementStrategies
{
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Autodesk.Revit.UI;
    using Models;
    using Models.Configurations;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for slide-out.
    /// </summary>
    public class SlideOutStrategy : ElementStrategyBase<PanelLayoutElement>
    {
        /// <inheritdoc />
        public SlideOutStrategy(MenuData menuData)
            : base(menuData)
        {
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
            ribbonPanel.AddSlideOut();
        }

        /// <inheritdoc />
        protected override RibbonItemData CreateElementForStack(PanelLayoutElement elementConfig)
        {
            return CannotBeStackedStub(elementConfig);
        }
    }
}