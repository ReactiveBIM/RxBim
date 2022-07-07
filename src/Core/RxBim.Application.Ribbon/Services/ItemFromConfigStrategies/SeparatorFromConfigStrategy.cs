namespace RxBim.Application.Ribbon.ItemFromConfigStrategies
{
    /// <summary>
    /// The strategy for getting a separator from a configuration section.
    /// </summary>
    public class SeparatorFromConfigStrategy : MarkupItemFromConfigStrategy
    {
        /// <inheritdoc />
        protected override PanelLayoutItemType ItemType => PanelLayoutItemType.Separator;

        /// <inheritdoc />
        protected override void AddItem(IPanelBuilder panelBuilder)
        {
            panelBuilder.Separator();
        }
    }
}