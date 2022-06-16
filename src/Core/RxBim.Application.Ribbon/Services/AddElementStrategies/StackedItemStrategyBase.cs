namespace RxBim.Application.Ribbon.AddElementStrategies
{
    /// <summary>
    /// Base implementation of <see cref="IAddElementStrategy"/> for stacked items.
    /// </summary>
    public abstract class StackedItemStrategyBase : IAddElementStrategy
    {
        /// <inheritdoc />
        public bool IsApplicable(IRibbonPanelElement config)
        {
            return config is StackedItems;
        }

        /// <inheritdoc />
        public abstract void CreateElement(object panel, IRibbonPanelElement config);

        /// <inheritdoc />
        public abstract object CreateElementForStack(IRibbonPanelElement config, bool small);
    }
}