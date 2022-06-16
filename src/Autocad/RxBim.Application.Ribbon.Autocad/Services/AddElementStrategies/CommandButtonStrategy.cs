namespace RxBim.Application.Ribbon.Services.AddElementStrategies
{
    using System;
    using System.Windows.Controls;
    using Application.Ribbon.AddElementStrategies;
    using Autodesk.Windows;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for command button.
    /// </summary>
    public class CommandButtonStrategy : CommandButtonStrategyBase
    {
        private readonly IPanelService _panelService;
        private readonly IButtonService _buttonService;

        /// <inheritdoc />
        public CommandButtonStrategy(IPanelService panelService, IButtonService buttonService)
        {
            _panelService = panelService;
            _buttonService = buttonService;
        }

        /// <inheritdoc />
        public override void CreateElement(object panel, IRibbonPanelElement config)
        {
            if (panel is not RibbonPanel ribbonPanel || config is not CommandButton cmdButtonConfig)
                return;
            var orientation = cmdButtonConfig.GetOrientation();
            _panelService.AddItem(ribbonPanel,
                _buttonService.CreateCommandButton(cmdButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        public override object CreateElementForStack(IRibbonPanelElement config, bool small)
        {
            if (config is not CommandButton cmdButtonConfig)
                throw new InvalidOperationException($"Invalid config type: {config.GetType().FullName}");
            var size = small ? RibbonItemSize.Standard : RibbonItemSize.Large;
            return _buttonService.CreateCommandButton(cmdButtonConfig, size, Orientation.Horizontal);
        }
    }
}