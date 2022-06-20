namespace RxBim.Application.Ribbon.ItemFromConfigStrategies
{
    /// <summary>
    /// The strategy for getting a slide-out from a configuration section.
    /// </summary>
    public class SlideOutFromConfigStrategy : MarkupItemFromConfigStrategy
    {
        /// <inheritdoc />
        protected override PanelLayoutItemType ItemType => PanelLayoutItemType.SlideOut;

        /// <inheritdoc />
        protected override void AddItem(IPanelBuilder panelBuilder)
        {
            panelBuilder.SlideOut();
        }
    }
}