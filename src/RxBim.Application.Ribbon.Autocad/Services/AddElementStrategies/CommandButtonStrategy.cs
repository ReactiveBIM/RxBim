namespace RxBim.Application.Ribbon.Autocad.Services.AddElementStrategies
{
    using System;
    using System.Windows.Controls;
    using Abstractions;
    using Autodesk.Windows;
    using Extensions;
    using Models.Configurations;
    using Ribbon.Abstractions;
    using Ribbon.Abstractions.ConfigurationBuilders;
    using Ribbon.Services.AddElementStrategies;

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
        public override void CreateAndAddElement(object panel, IRibbonPanelElement config)
        {
            if (panel is not RibbonPanel ribbonPanel || config is not CommandButton cmdButtonConfig)
                return;
            var orientation = cmdButtonConfig.GetSingleLargeButtonOrientation();
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