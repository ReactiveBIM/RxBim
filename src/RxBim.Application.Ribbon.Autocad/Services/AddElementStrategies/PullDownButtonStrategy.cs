namespace RxBim.Application.Ribbon.Autocad.Services.AddElementStrategies
{
    using System.Windows.Controls;
    using Abstractions;
    using Autodesk.Windows;
    using Extensions;
    using Models.Configurations;
    using Ribbon.Abstractions;

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
        protected override void CreateAndAddElement(
            RibbonTab ribbonTab,
            RibbonPanel ribbonPanel,
            PullDownButton pullDownButtonConfig)
        {
            var orientation = pullDownButtonConfig.GetSingleLargeButtonOrientation();
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