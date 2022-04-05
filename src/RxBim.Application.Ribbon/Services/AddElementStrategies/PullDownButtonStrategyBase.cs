namespace RxBim.Application.Ribbon.Services.AddElementStrategies
{
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Models.Configurations;

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
        public abstract void CreateElement(
            object tab,
            object panel,
            IRibbonPanelElement config);
    }
}