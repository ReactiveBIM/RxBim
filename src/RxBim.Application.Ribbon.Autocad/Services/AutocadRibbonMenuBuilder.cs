namespace RxBim.Application.Ribbon.Autocad.Services
{
    using Abstractions;
    using Autodesk.Windows;
    using Models;
    using Ribbon.Abstractions;
    using Ribbon.Services;
    using Shared.Abstractions;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilder"/> for AutoCAD
    /// </summary>
    public class AutocadRibbonMenuBuilder : RibbonMenuBuilderBase<RibbonTab, RibbonPanel>
    {
        private readonly IOnlineHelpService _onlineHelpService;
        private readonly IPanelService _panelService;
        private readonly IButtonService _buttonService;
        private readonly IRibbonComponentStorageService _storageService;
        private readonly IRibbonEventsService _ribbonEventsService;
        private readonly IColorThemeService _colorThemeService;
        private readonly ITabService _tabService;
        private bool _alreadyBuiltOnce;

        /// <inheritdoc />
        public AutocadRibbonMenuBuilder(
            IOnlineHelpService onlineHelpService,
            IPanelService panelService,
            IButtonService buttonService,
            IRibbonComponentStorageService storageService,
            IRibbonEventsService ribbonEventsService,
            IColorThemeService colorThemeService,
            ITabService tabService,
            MenuData menuData,
            IDiCollectionService<IAddElementStrategy> strategiesService)
            : base(menuData, strategiesService)
        {
            _panelService = panelService;
            _buttonService = buttonService;
            _storageService = storageService;
            _ribbonEventsService = ribbonEventsService;
            _colorThemeService = colorThemeService;
            _tabService = tabService;
            _onlineHelpService = onlineHelpService;
            _colorThemeService.ThemeChanged += (_, _) => _buttonService.ApplyCurrentTheme();
            _ribbonEventsService.NeedRebuild += (_, _) => BuildRibbonMenu();
        }

        /// <inheritdoc />
        protected override void PreBuildActions()
        {
            base.PreBuildActions();
            if (!_alreadyBuiltOnce)
            {
                _alreadyBuiltOnce = true;
                _onlineHelpService.Run();
                _ribbonEventsService.Run();
                _colorThemeService.Run();
            }
            else
            {
                _buttonService.ClearButtonCache();
                _onlineHelpService.ClearToolTipsCache();
                _storageService.DeleteComponents();
            }
        }

        /// <inheritdoc />
        protected override bool CheckRibbonCondition() => ComponentManager.Ribbon != null;

        /// <inheritdoc />
        protected override RibbonTab GetOrCreateTab(string tabName) =>
            _tabService.GetTab(tabName) ?? _tabService.CreateTab(tabName);

        /// <inheritdoc />
        protected override RibbonPanel GetOrCreatePanel(RibbonTab acRibbonTab, string panelName) =>
            _panelService.GetOrCreatePanel(acRibbonTab, panelName);
    }
}