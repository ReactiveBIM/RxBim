namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using Abstractions;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Implementation of <see cref="IItemStrategy"/> for command button.
    /// </summary>
    public class CommandButtonStrategy : ItemStrategyBase<CommandButton>
    {
        private readonly MenuData _menuData;
        private readonly IRibbonPanelItemService _ribbonPanelItemService;

        /// <inheritdoc />
        public CommandButtonStrategy(MenuData menuData, IRibbonPanelItemService ribbonPanelItemService)
        {
            _menuData = menuData;
            _ribbonPanelItemService = ribbonPanelItemService;
        }

        /// <inheritdoc />
        protected override void AddItem(string tabName, RibbonPanel panel, CommandButton cmdButtonConfig)
        {
            var pushButtonData = _ribbonPanelItemService.CreateCommandButtonData(cmdButtonConfig);
            panel.AddItem(pushButtonData);
        }

        /// <inheritdoc />
        protected override RibbonItemData GetItemForStack(CommandButton cmdButtonConfig)
        {
            return _ribbonPanelItemService.CreateCommandButtonData(cmdButtonConfig);
        }
    }
}