namespace RxBim.Application.Ribbon.Revit.Services.AddElementStrategies
{
    using System.Linq;
    using Abstractions;
    using Autodesk.Revit.UI;
    using Models;
    using Models.Configurations;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for pull-down button.
    /// </summary>
    public class PulldownButtonStrategy : ElementStrategyBase<PullDownButton>
    {
        /// <inheritdoc />
        public PulldownButtonStrategy(MenuData menuData)
            : base(menuData)
        {
        }

        /// <inheritdoc />
        protected override void CreateAndAddElement(RibbonPanel ribbonPanel, PullDownButton pullDownButtonConfig)
        {
            var pulldownButtonData = CreatePulldownButtonData(pullDownButtonConfig);
            var pulldownButton = (PulldownButton)ribbonPanel.AddItem(pulldownButtonData);

            CreateButtonsForPullDown(pullDownButtonConfig, pulldownButton);
        }

        /// <inheritdoc />
        protected override RibbonItemData CreateElementForStack(PullDownButton pullDownButtonConfig)
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