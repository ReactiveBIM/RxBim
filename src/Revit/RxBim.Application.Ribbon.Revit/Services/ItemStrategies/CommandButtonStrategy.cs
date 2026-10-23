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
        protected override void AddItem(RibbonTab tab, RibbonPanel ribbonPanel, CommandButton itemConfig)
        {
            var pushButtonData = _ribbonPanelItemService.CreateCommandButtonData(itemConfig);
            var pushButton = (PushButton)ribbonPanel.AddItem(pushButtonData);
            _ribbonPanelItemService.RegisterButton(pushButton, itemConfig);
            _ribbonPanelItemService.RegisterEnhancedToolTip(itemConfig, tab, ribbonPanel.Title);
            _ribbonPanelItemService.SetButtonTextVisibility(itemConfig, tab, ribbonPanel.Title);
        }

        /// <inheritdoc />
        protected override RibbonItemData GetItemForStack(CommandButton itemConfig)
        {
            return _ribbonPanelItemService.CreateCommandButtonData(itemConfig);
        }
    }
}