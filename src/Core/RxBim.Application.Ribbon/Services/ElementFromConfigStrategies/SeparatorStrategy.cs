namespace RxBim.Application.Ribbon.ElementFromConfigStrategies
{
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
            panelBuilder.Separator();
        }
    }
}