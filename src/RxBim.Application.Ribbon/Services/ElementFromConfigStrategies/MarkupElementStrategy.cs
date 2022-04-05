namespace RxBim.Application.Ribbon.Services.ElementFromConfigStrategies
{
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Models.Configurations;

    /// <summary>
    /// The strategy for getting a markup element from a configuration section.
    /// </summary>
    public abstract class MarkupElementStrategy : IElementFromConfigStrategy
    {
        /// <summary>
        /// Element type.
        /// </summary>
        protected abstract PanelLayoutElementType ElementType { get; }

        /// <inheritdoc />
        public bool IsApplicable(IConfigurationSection elementSection)
        {
            var typeSection = elementSection.GetSection(nameof(PanelLayoutElement.LayoutElementType));
            if (!typeSection.Exists())
                return false;

            var type = typeSection.Get<PanelLayoutElementType>();
            return type == ElementType;
        }

        /// <inheritdoc />
        public void CreateFromConfigAndAdd(IConfigurationSection elementSection, IPanelBuilder panelBuilder)
        {
            AddElement(panelBuilder);
        }

        /// <summary>
        /// Action for the element adding.
        /// </summary>
        /// <param name="panelBuilder">Panel builder.</param>
        protected abstract void AddElement(IPanelBuilder panelBuilder);
    }
}