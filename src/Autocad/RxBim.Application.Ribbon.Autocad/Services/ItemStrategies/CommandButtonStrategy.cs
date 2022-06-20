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
        private readonly MenuData _menuData;

        /// <inheritdoc />
        public CommandButtonStrategy(IPanelService panelService, IButtonService buttonService, MenuData menuData)
        {
            _panelService = panelService;
            _buttonService = buttonService;
            _menuData = menuData;
        }

        /// <inheritdoc />
        protected override void AddItem(RibbonTab ribbonTab, RibbonPanel ribbonPanel, CommandButton cmdButtonConfig)
        {
            cmdButtonConfig.LoadFromAttribute(_menuData.MenuAssembly);
            var orientation = cmdButtonConfig.GetOrientation();
            _panelService.AddItem(ribbonPanel,
                _buttonService.CreateCommandButton(cmdButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override RibbonItem GetItemForStack(CommandButton cmdButtonConfig, RibbonItemSize size)
        {
            cmdButtonConfig.LoadFromAttribute(_menuData.MenuAssembly);
            return _buttonService.CreateCommandButton(cmdButtonConfig, size, Orientation.Horizontal);
        }
    }
}