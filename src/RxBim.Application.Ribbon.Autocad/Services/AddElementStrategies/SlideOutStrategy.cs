namespace RxBim.Application.Ribbon.Autocad.Services.AddElementStrategies
{
    using Abstractions;
    using Autodesk.Windows;
    using Ribbon.Abstractions;
    using Ribbon.Abstractions.ConfigurationBuilders;
    using Ribbon.Services.AddElementStrategies;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for slide-out.
    /// </summary>
    public class SlideOutStrategy : SlideOutStrategyBase
    {
        private readonly IPanelService _panelService;

        /// <inheritdoc />
        public SlideOutStrategy(IPanelService panelService)
        {
            _panelService = panelService;
        }

        /// <inheritdoc />
        public override void CreateElement(object tab, object panel, IRibbonPanelElement config)
        {
            if (panel is not RibbonPanel ribbonPanel)
                return;

            _panelService.AddSlideOut(ribbonPanel);
        }
    }
}