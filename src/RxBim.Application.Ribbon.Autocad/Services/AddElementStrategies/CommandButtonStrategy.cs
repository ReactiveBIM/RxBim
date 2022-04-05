namespace RxBim.Application.Ribbon.Autocad.Services.AddElementStrategies
{
    using System.Windows.Controls;
    using Abstractions;
    using Autodesk.Windows;
    using Extensions;
    using Models.Configurations;
    using Ribbon.Abstractions;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for command button.
    /// </summary>
    public class CommandButtonStrategy : ElementStrategyBase<CommandButton>
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
        protected override void CreateAndAddElement(RibbonPanel ribbonPanel, CommandButton cmdButtonConfig)
        {
            var orientation = cmdButtonConfig.GetSingleLargeButtonOrientation();
            _panelService.AddItem(ribbonPanel,
                _buttonService.CreateCommandButton(cmdButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override RibbonItem CreateElementForStack(CommandButton cmdButtonConfig, RibbonItemSize size)
        {
            return _buttonService.CreateCommandButton(cmdButtonConfig, size, Orientation.Horizontal);
        }
    }
}