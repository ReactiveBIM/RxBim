namespace RxBim.Application.Ribbon.Services.AddItemStrategies
{
    using System.Windows.Controls;
    using Autodesk.Windows;

    /// <summary>
    /// Implementation of <see cref="IAddItemStrategy"/> for command button.
    /// </summary>
    public class CommandButtonStrategy : ItemStrategyBase<CommandButton>
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
        protected override void AddItem(RibbonPanel ribbonPanel, CommandButton cmdButtonConfig)
        {
            var orientation = cmdButtonConfig.GetOrientation();
            _panelService.AddItem(ribbonPanel,
                _buttonService.CreateCommandButton(cmdButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override RibbonItem GetItemForStack(CommandButton cmdButtonConfig, RibbonItemSize size)
        {
            return _buttonService.CreateCommandButton(cmdButtonConfig, size, Orientation.Horizontal);
        }
    }
}