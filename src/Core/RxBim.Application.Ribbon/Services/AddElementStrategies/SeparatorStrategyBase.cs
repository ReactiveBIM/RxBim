namespace RxBim.Application.Ribbon.AddElementStrategies
{
    /// <summary>
    /// Base implementation of <see cref="IAddElementStrategy"/> for separator.
    /// </summary>
    public abstract class SeparatorStrategyBase : IAddElementStrategy
    {
        /// <inheritdoc />
        public bool IsApplicable(IRibbonPanelElement config)
        {
            return config is PanelLayoutElement { LayoutElementType: PanelLayoutElementType.Separator };
        }

        /// <inheritdoc />
        public abstract void CreateElement(object tab, object panel, IRibbonPanelElement config);
    }
}