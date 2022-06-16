namespace RxBim.Application.Ribbon.Services.AddElementStrategies
{
    using Application.Ribbon.AddElementStrategies;
    using Autodesk.Windows;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for separator.
    /// </summary>
    public class SeparatorStrategy : SeparatorStrategyBase
    {
        private readonly IPanelService _panelService;

        /// <inheritdoc />
        public SeparatorStrategy(IPanelService panelService)
        {
            _panelService = panelService;
        }

        /// <inheritdoc />
        public override void CreateElement(object tab, object panel, IRibbonPanelElement config)
        {
            if (panel is not RibbonPanel ribbonPanel)
                return;

            _panelService.AddSeparator(ribbonPanel);
        }
    }
}