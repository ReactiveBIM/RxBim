namespace RxBim.Application.Ribbon.Services.AddElementStrategies
{
    using System;
    using System.Windows.Controls;
    using Application.Ribbon.AddElementStrategies;
    using Autodesk.Windows;

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
        public override void CreateElement(object panel, IRibbonPanelElement config)
        {
            if (panel is not RibbonPanel ribbonPanel || config is not PullDownButton pullDownButtonConfig)
                return;
            var orientation = pullDownButtonConfig.GetOrientation();
            _panelService.AddItem(ribbonPanel,
                _buttonService.CreatePullDownButton(pullDownButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        public override object CreateElementForStack(IRibbonPanelElement config, bool small)
        {
            if (config is not PullDownButton pullDownButtonConfig)
                throw new InvalidOperationException($"Invalid config type: {config.GetType().FullName}");
            var size = small ? RibbonItemSize.Standard : RibbonItemSize.Large;
            return _buttonService.CreatePullDownButton(pullDownButtonConfig, size, Orientation.Horizontal);
        }
    }
}