namespace RxBim.Application.Ribbon.ElementFromConfigStrategies
{
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
        public void CreateFromConfigAndAdd(
            IConfigurationSection elementSection,
            IPanelBuilder panelBuilder)
        {
            var element = elementSection.Get<T>();
            panelBuilder.AddElement(element);
        }
    }
}