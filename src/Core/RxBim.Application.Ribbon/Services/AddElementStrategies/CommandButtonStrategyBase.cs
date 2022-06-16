namespace RxBim.Application.Ribbon.AddElementStrategies
{
    /// <summary>
    /// Base implementation of <see cref="IAddElementStrategy"/> for command button.
    /// </summary>
    public abstract class CommandButtonStrategyBase : IAddElementStrategy
    {
        /// <inheritdoc />
        public bool IsApplicable(IRibbonPanelElement config)
        {
            return config is CommandButton;
        }

        /// <inheritdoc />
        public abstract void CreateElement(object panel, IRibbonPanelElement config);

        /// <inheritdoc />
        public abstract object CreateElementForStack(IRibbonPanelElement config, bool small);
    }
}