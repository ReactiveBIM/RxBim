namespace RxBim.Application.Ribbon.Services.AddItemStrategies
{
    using System.Linq;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Implementation of <see cref="IAddItemStrategy"/> for pull-down button.
    /// </summary>
    public class PulldownButtonStrategy : ItemStrategyBase<PullDownButton>
    {
        /// <inheritdoc />
        public PulldownButtonStrategy(MenuData menuData)
            : base(menuData)
        {
        }

        /// <inheritdoc />
        protected override void AddItem(string tabName, RibbonPanel ribbonPanel, PullDownButton pullDownButtonConfig)
        {
            var pulldownButtonData = CreatePulldownButtonData(pullDownButtonConfig);
            var pulldownButton = (PulldownButton)ribbonPanel.AddItem(pulldownButtonData);

            CreateButtonsForPullDown(pullDownButtonConfig, pulldownButton);
        }

        /// <inheritdoc />
        protected override RibbonItemData GetItemForStack(PullDownButton pullDownButtonConfig)
        {
            return CreatePulldownButtonData(pullDownButtonConfig);
        }

        private void CreateButtonsForPullDown(PullDownButton pullDownButtonConfig, PulldownButton pulldownButton)
        {
            foreach (var pushButtonData in pullDownButtonConfig.CommandButtonsList.Select(CreateCommandButtonData))
            {
                pulldownButton.AddPushButton(pushButtonData);
            }
        }

        private PulldownButtonData CreatePulldownButtonData(PullDownButton pullDownButtonConfig)
        {
            CheckButtonName(pullDownButtonConfig);
            var pulldownButtonData = new PulldownButtonData(
                pullDownButtonConfig.Name,
                pullDownButtonConfig.Text ?? pullDownButtonConfig.Name);
            SetButtonProperties(pulldownButtonData, pullDownButtonConfig);
            SetTooltip(pulldownButtonData, pullDownButtonConfig.ToolTip);
            return pulldownButtonData;
        }
    }
}