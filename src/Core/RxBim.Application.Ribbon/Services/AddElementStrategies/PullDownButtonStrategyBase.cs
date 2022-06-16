namespace RxBim.Application.Ribbon.AddElementStrategies
{
    /// <summary>
    /// Base implementation of <see cref="IAddElementStrategy"/> for pull-down button.
    /// </summary>
    public abstract class PullDownButtonStrategyBase : IAddElementStrategy
    {
        /// <inheritdoc />
        public bool IsApplicable(IRibbonPanelElement config)
        {
            return config is PullDownButton;
        }

        /// <inheritdoc />
        public abstract void CreateElement(object panel, IRibbonPanelElement config);

        /// <inheritdoc />
        public abstract object CreateElementForStack(IRibbonPanelElement config, bool small);
    }
}