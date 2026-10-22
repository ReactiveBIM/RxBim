namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using Abstractions;
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

    /// <summary>
    /// Implementation of <see cref="IItemStrategy"/> for pull-down button.
    /// </summary>
    public class PulldownButtonStrategy : ItemStrategyBase<PullDownButton>
    {
        private readonly IRibbonPanelItemService _ribbonPanelItemService;

        /// <inheritdoc />
        public PulldownButtonStrategy(IRibbonPanelItemService ribbonPanelItemService)
        {
            _ribbonPanelItemService = ribbonPanelItemService;
        }

        /// <inheritdoc />
        protected override void AddItem(RibbonTab tab, RibbonPanel ribbonPanel, PullDownButton itemConfig)
        {
            var pulldownButtonData = CreatePulldownButtonData(itemConfig);
            var pulldownButton = (PulldownButton)ribbonPanel.AddItem(pulldownButtonData);
            _ribbonPanelItemService.RegisterButton(pulldownButton, itemConfig);
            _ribbonPanelItemService.CreateButtonsForPullDown(itemConfig, pulldownButton);
            _ribbonPanelItemService.RegisterEnhancedToolTip(itemConfig, tab, ribbonPanel.Title);
            _ribbonPanelItemService.SetButtonTextVisibility(itemConfig, tab, ribbonPanel.Title);
        }

        /// <inheritdoc />
        protected override RibbonItemData GetItemForStack(PullDownButton itemConfig)
        {
            return CreatePulldownButtonData(itemConfig);
        }

        private PulldownButtonData CreatePulldownButtonData(PullDownButton pullDownButtonConfig)
        {
            _ribbonPanelItemService.CheckButtonName(pullDownButtonConfig);
            var pulldownButtonData = new PulldownButtonData(
                pullDownButtonConfig.Name,
                pullDownButtonConfig.Text ?? pullDownButtonConfig.Name);
            _ribbonPanelItemService.SetButtonProperties(pulldownButtonData, pullDownButtonConfig);
            _ribbonPanelItemService.SetTooltip(pulldownButtonData, pullDownButtonConfig.ToolTip);
            return pulldownButtonData;
        }
    }
}