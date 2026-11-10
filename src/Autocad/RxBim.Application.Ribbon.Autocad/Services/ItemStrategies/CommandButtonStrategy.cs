namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using System.Windows.Controls;
    using Autodesk.Windows;

    /// <summary>
    /// Implementation of <see cref="IItemStrategy"/> for command button.
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
        protected override void AddItem(RibbonTab ribbonTab, RibbonPanel ribbonPanel, CommandButton itemConfig)
        {
            _panelService.AddItem(ribbonPanel,
                _buttonService.CreateCommandButton(itemConfig, RibbonItemSize.Large));
        }

        /// <inheritdoc />
        protected override RibbonItem GetItemForStack(CommandButton itemConfig, RibbonItemSize size)
        {
            return _buttonService.CreateCommandButton(itemConfig, size, Orientation.Horizontal);
        }
    }
}