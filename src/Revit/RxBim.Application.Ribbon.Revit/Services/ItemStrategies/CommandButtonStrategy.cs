namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using Abstractions;
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using JetBrains.Annotations;
    using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

    /// <summary>
    /// Implementation of <see cref="IItemStrategy"/> for command button.
    /// </summary>
    [UsedImplicitly]
    public class CommandButtonStrategy : ItemStrategyBase<CommandButton>
    {
        private readonly IRibbonPanelItemService _ribbonPanelItemService;

        /// <inheritdoc />
        public CommandButtonStrategy(MenuData menuData, IRibbonPanelItemService ribbonPanelItemService)
        {
            _ribbonPanelItemService = ribbonPanelItemService;
        }

        /// <inheritdoc />
        protected override void AddItem(RibbonTab tab, RibbonPanel panel, CommandButton cmdButtonConfig)
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