namespace RxBim.Application.Ribbon.Services.ElementFromConfigStrategies
{
    using Abstractions.ConfigurationBuilders;
    using Models;

    /// <summary>
    /// The strategy for getting a separator from a configuration section.
    /// </summary>
    public class SeparatorStrategy : MarkupElementStrategy
    {
        /// <inheritdoc />
        protected override PanelLayoutElementType ElementType => PanelLayoutElementType.Separator;

        /// <inheritdoc />
        protected override void AddElement(IPanelBuilder panelBuilder)
        {
            panelBuilder.AddSeparator();
        }
    }
}