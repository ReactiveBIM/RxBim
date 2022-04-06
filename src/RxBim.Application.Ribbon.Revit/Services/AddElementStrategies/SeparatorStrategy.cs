namespace RxBim.Application.Ribbon.Revit.Services.AddElementStrategies
{
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Autodesk.Revit.UI;
    using Models;
    using Models.Configurations;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for separator.
    /// </summary>
    public class SeparatorStrategy : ElementStrategyBase<PanelLayoutElement>
    {
        /// <inheritdoc />
        public SeparatorStrategy(MenuData menuData)
            : base(menuData)
        {
        }

        /// <inheritdoc />
        public override bool IsApplicable(IRibbonPanelElement config)
        {
            return base.IsApplicable(config) &&
                   ((PanelLayoutElement)config).LayoutElementType == PanelLayoutElementType.Separator;
        }

        /// <inheritdoc />
        protected override void CreateAndAddElement(RibbonPanel ribbonPanel, PanelLayoutElement elementConfig)
        {
            ribbonPanel.AddSeparator();
        }

        /// <inheritdoc />
        protected override RibbonItemData CreateElementForStack(PanelLayoutElement elementConfig)
        {
            return CannotBeStackedStub(elementConfig);
        }
    }
}