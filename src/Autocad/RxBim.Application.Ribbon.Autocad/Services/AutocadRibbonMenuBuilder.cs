namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Autodesk.Windows;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilder"/> for AutoCAD.
    /// </summary>
    public class AutocadRibbonMenuBuilder : RibbonMenuBuilderBase<RibbonTab, RibbonPanel>
    {
        private readonly IOnlineHelpService _onlineHelpService;
        private readonly IPanelService _panelService;
        private readonly IButtonService _buttonService;
        private readonly IRibbonComponentStorageService _storageService;
        private readonly IRibbonEventsService _ribbonEventsService;
        private readonly IThemedRibbonButtonService<RibbonButton> _themedButtonService;
        private readonly ITabService _tabService;
        private bool _alreadyBuiltOnce;

        /// <inheritdoc />
        public AutocadRibbonMenuBuilder(
            IOnlineHelpService onlineHelpService,
            IPanelService panelService,
            IButtonService buttonService,
            IRibbonComponentStorageService storageService,
            IRibbonEventsService ribbonEventsService,
            IThemedRibbonButtonService<RibbonButton> themedButtonService,
            ITabService tabService,
            MenuData menuData,
            IServiceProvider serviceProvider)
            : base(menuData, serviceProvider)
        {
            _panelService = panelService;
            _buttonService = buttonService;
            _storageService = storageService;
            _ribbonEventsService = ribbonEventsService;
            _themedButtonService = themedButtonService;
            _tabService = tabService;
            _onlineHelpService = onlineHelpService;
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
                _themedButtonService.Run();
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