namespace RxBim.Application.Ribbon.Services.AddElementStrategies
{
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Models;
    using Models.Configurations;

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
        public abstract void CreateAndAddElement(object panel, IRibbonPanelElement config);

        /// <inheritdoc />
        public abstract object CreateElementForStack(IRibbonPanelElement config, bool small);
    }
}