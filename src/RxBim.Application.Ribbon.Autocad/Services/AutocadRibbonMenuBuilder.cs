namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System;
    using System.Linq;
    using System.Windows.Controls;
    using Abstractions;
    using Autodesk.Private.Windows;
    using Autodesk.Windows;
    using Extensions;
    using Models;
    using Models.Configurations;
    using Ribbon.Abstractions;
    using Ribbon.Services;
    using Ribbon.Services.ConfigurationBuilders;
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
            IStrategyFactory<IAddElementStrategy> addElementsStrategiesFactory)
            : base(menuData, addElementsStrategiesFactory)
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

        /// <inheritdoc />
        protected override void CreateStackedItems(RibbonPanel panel, StackedItems stackedItems)
        {
            var stackSize = stackedItems.StackedButtons.Count;
            var stackedItemsRow = new RibbonRowPanel();
            var size = stackSize == StackedItemsBuilder.MaxStackSize
                ? RibbonItemSize.Standard
                : RibbonItemSize.Large;

            _panelService.AddItem(panel, stackedItemsRow);

            for (var i = 0; i < stackSize; i++)
            {
                if (i > 0)
                    stackedItemsRow.Items.Add(new RibbonRowBreak());

                var buttonConfig = stackedItems.StackedButtons[i];
                var buttonItem = buttonConfig switch
                {
                    AboutButton aboutButton =>
                        _buttonService.CreateAboutButton(aboutButton, size, Orientation.Horizontal, GetIconImage),
                    CommandButton cmdButton =>
                        _buttonService.CreateCommandButtonInternal(cmdButton, size, Orientation.Horizontal),
                    PullDownButton pullDownButton =>
                        _buttonService.CreatePullDownButtonInternal(pullDownButton, size, Orientation.Horizontal),
                    _ => throw new ArgumentOutOfRangeException($"Unknown button type: {buttonConfig.GetType().Name}")
                };

                stackedItemsRow.Items.Add(buttonItem);
            }
        }
    }
}