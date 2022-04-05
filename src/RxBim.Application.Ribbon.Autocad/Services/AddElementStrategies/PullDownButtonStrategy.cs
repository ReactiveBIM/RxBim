namespace RxBim.Application.Ribbon.Autocad.Services.AddElementStrategies
{
    using Abstractions;
    using Autodesk.Windows;
    using Extensions;
    using Models.Configurations;
    using Ribbon.Abstractions;
    using Ribbon.Abstractions.ConfigurationBuilders;
    using Ribbon.Services.AddElementStrategies;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for pull-down button.
    /// </summary>
    public class PullDownButtonStrategy : PullDownButtonStrategyBase
    {
        private readonly IPanelService _panelService;
        private readonly IButtonService _buttonService;

        /// <inheritdoc />
        public PullDownButtonStrategy(IPanelService panelService, IButtonService buttonService)
        {
            _panelService = panelService;
            _buttonService = buttonService;
        }

        /// <inheritdoc />
        public override void CreateElement(object tab, object panel, IRibbonPanelElement config)
        {
            if (panel is not RibbonPanel ribbonPanel || config is not PullDownButton pullDownButtonConfig)
                return;
            var orientation = pullDownButtonConfig.GetSingleLargeButtonOrientation();
            _panelService.AddItem(ribbonPanel,
                _buttonService.CreatePullDownButton(pullDownButtonConfig, RibbonItemSize.Large, orientation));
        }
    }
}