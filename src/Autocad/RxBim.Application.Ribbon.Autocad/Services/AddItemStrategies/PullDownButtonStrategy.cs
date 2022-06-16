namespace RxBim.Application.Ribbon.Services.AddItemStrategies
{
    using System.Windows.Controls;
    using Autodesk.Windows;

    /// <summary>
    /// Implementation of <see cref="IAddItemStrategy"/> for pull-down button.
    /// </summary>
    public class PullDownButtonStrategy : ItemStrategyBase<PullDownButton>
    {
        private readonly IPanelService _panelService;
        private readonly IButtonService _buttonService;

        /// <inheritdoc />
        public PullDownButtonStrategy(IPanelService panelService, IButtonService buttonService)
        {
            _panelService = panelService;
            _buttonService = buttonService;
        }

        /// <inheritdoc />
        protected override void AddItem(
            RibbonTab ribbonTab,
            RibbonPanel ribbonPanel,
            PullDownButton pullDownButtonConfig)
        {
            var orientation = pullDownButtonConfig.GetOrientation();
            _panelService.AddItem(ribbonPanel,
                _buttonService.CreatePullDownButton(pullDownButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override RibbonItem GetItemForStack(PullDownButton pullDownButtonConfig, RibbonItemSize size)
        {
            return _buttonService.CreatePullDownButton(pullDownButtonConfig, size, Orientation.Horizontal);
        }
    }
}