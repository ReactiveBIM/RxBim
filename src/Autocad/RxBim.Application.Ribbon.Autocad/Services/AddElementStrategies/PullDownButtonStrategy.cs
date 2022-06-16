namespace RxBim.Application.Ribbon.Services.AddElementStrategies
{
    using System.Windows.Controls;
    using Autodesk.Windows;

    /// <summary>
    /// Implementation of <see cref="IAddElementStrategy"/> for pull-down button.
    /// </summary>
    public class PullDownButtonStrategy : ElementStrategyBase<PullDownButton>
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
        protected override void CreateAndAddElement(RibbonPanel ribbonPanel, PullDownButton pullDownButtonConfig)
        {
            var orientation = pullDownButtonConfig.GetOrientation();
            _panelService.AddItem(ribbonPanel,
                _buttonService.CreatePullDownButton(pullDownButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override RibbonItem CreateElementForStack(PullDownButton pullDownButtonConfig, RibbonItemSize size)
        {
            return _buttonService.CreatePullDownButton(pullDownButtonConfig, size, Orientation.Horizontal);
        }
    }
}