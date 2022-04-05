namespace RxBim.Application.Ribbon.Services.ElementFromConfigStrategies
{
    using Abstractions.ConfigurationBuilders;
    using Models;

    /// <summary>
    /// The strategy for getting a slide-out from a configuration section.
    /// </summary>
    public class SlideOutStrategy : MarkupElementStrategy
    {
        /// <inheritdoc />
        protected override PanelLayoutElementType ElementType => PanelLayoutElementType.SlideOut;

        /// <inheritdoc />
        protected override void AddElement(IPanelBuilder panelBuilder)
        {
            panelBuilder.AddSlideOut();
        }
    }
}