namespace RxBim.Application.Ribbon.Services.ElementFromConfigStrategies
{
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// The strategy for getting a simple element from a configuration section.
    /// </summary>
    public abstract class SimpleElementStrategyBase<T> : IElementFromConfigStrategy
        where T : IRibbonPanelElement
    {
        /// <inheritdoc />
        public abstract bool IsApplicable(IConfigurationSection elementSection);

        /// <inheritdoc />
        public void CreateAndAddToPanelConfig(
            IConfigurationSection elementSection,
            IPanelBuilder panelBuilder)
        {
            panelBuilder.AddElement(CreateForStack(elementSection));
        }

        /// <inheritdoc />
        public IRibbonPanelElement CreateForStack(IConfigurationSection elementSection)
        {
            return elementSection.Get<T>();
        }
    }
}