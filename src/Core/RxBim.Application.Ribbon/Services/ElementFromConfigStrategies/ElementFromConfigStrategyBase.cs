namespace RxBim.Application.Ribbon.ElementFromConfigStrategies
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;

    /// <inheritdoc />
    public abstract class ElementFromConfigStrategyBase : IElementFromConfigStrategy
    {
        /// <inheritdoc />
        public abstract bool IsApplicable(IConfigurationSection elementSection);

        /// <inheritdoc />
        public abstract void CreateFromConfigAndAdd(
            IConfigurationSection elementSection,
            ICollection<IRibbonPanelElement> elements);

        /// <summary>
        /// Creates a simple element and adds to a elements collection.
        /// </summary>
        /// <param name="elementSection">Element configuration section.</param>
        /// <param name="elements">Elements collection.</param>
        /// <typeparam name="T">Element type.</typeparam>
        protected void CreateSimpleFromConfigAndAddInternal<T>(
            IConfigurationSection elementSection,
            ICollection<IRibbonPanelElement> elements)
            where T : IRibbonPanelElement
        {
            var button = elementSection.Get<T>();
            elements.Add(button);
        }
    }
}