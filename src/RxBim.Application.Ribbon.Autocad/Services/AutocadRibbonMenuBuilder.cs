namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System;
    using System.Linq;
    using System.Windows.Controls;
    using Abstractions;
    using Autodesk.Private.Windows;
    using Autodesk.Windows;
    using Extensions;
    using GalaSoft.MvvmLight.Command;
    using Models.Configurations;
    using Ribbon.Abstractions;
    using Ribbon.Services;
    using Ribbon.Services.ConfigurationBuilders;
    using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

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
        private bool _alreadyBuiltOnce;

        /// <inheritdoc />
        public AutocadRibbonMenuBuilder(
            IOnlineHelpService onlineHelpService,
            IPanelService panelService,
            IButtonService buttonService,
            IRibbonComponentStorageService storageService,
            IRibbonEventsService ribbonEventsService,
            IColorThemeService colorThemeService)
        {
            _panelService = panelService;
            _buttonService = buttonService;
            _storageService = storageService;
            _ribbonEventsService = ribbonEventsService;
            _colorThemeService = colorThemeService;
            _onlineHelpService = onlineHelpService;
            _colorThemeService.ThemeChanged += (_, _) => _buttonService.ApplyCurrentTheme(GetIconImage);
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
        protected override RibbonTab GetOrCreateTab(string tabName)
        {
            var tab = ComponentManager.Ribbon.Tabs.FirstOrDefault(x =>
                x.IsVisible && x.Title != null && x.Title.Equals(tabName, StringComparison.OrdinalIgnoreCase));

            if (tab is not null)
                return tab;

            tab = new RibbonTab
            {
                Title = tabName,
                Id = $"TAB_{tabName.GetHashCode():0}"
            };

            ComponentManager.Ribbon.Tabs.Add(tab);
            _storageService.AddTab(tab);

            return tab;
        }

        /// <inheritdoc />
        protected override RibbonPanel GetOrCreatePanel(RibbonTab acRibbonTab, string panelName) =>
            _panelService.GetOrCreatePanel(acRibbonTab, panelName);

        /// <inheritdoc />
        protected override void CreateAboutButton(RibbonTab tab, RibbonPanel panel, AboutButton aboutButtonConfig)
        {
            var orientation = aboutButtonConfig.GetSingleLargeButtonOrientation();
            _panelService.AddItem(panel,
                _buttonService.CreateAboutButton(aboutButtonConfig, RibbonItemSize.Large, orientation, GetIconImage));
        }

        /// <inheritdoc />
        protected override void CreateCommandButton(RibbonPanel panel, CommandButton cmdButtonConfig)
        {
            var orientation = cmdButtonConfig.GetSingleLargeButtonOrientation();
            _panelService.AddItem(panel,
                CreateCommandButtonInternal(cmdButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override void CreatePullDownButton(RibbonPanel panel, PullDownButton pullDownButtonConfig)
        {
            var orientation = pullDownButtonConfig.GetSingleLargeButtonOrientation();
            _panelService.AddItem(panel,
                CreatePullDownButtonInternal(pullDownButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override void AddSeparator(RibbonPanel panel) => _panelService.AddSeparator(panel);

        /// <inheritdoc />
        protected override void AddSlideOut(RibbonPanel panel) => _panelService.AddSlideOut(panel);

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
                        CreateCommandButtonInternal(cmdButton, size, Orientation.Horizontal),
                    PullDownButton pullDownButton =>
                        CreatePullDownButtonInternal(pullDownButton, size, Orientation.Horizontal),
                    _ => throw new ArgumentOutOfRangeException($"Unknown button type: {buttonConfig.GetType().Name}")
                };

                stackedItemsRow.Items.Add(buttonItem);
            }
        }

        private RibbonButton CreateCommandButtonInternal(
            CommandButton config,
            RibbonItemSize size,
            Orientation orientation)
        {
            var button =
                _buttonService.CreateNewButtonBase<RibbonButton>(config, size, orientation, false, GetIconImage, false);

            if (!string.IsNullOrWhiteSpace(config.CommandType))
            {
                var commandType = GetCommandType(config.CommandType!);
                var tooltip = GetTooltipContent(config, commandType);
                _buttonService.SetTooltipForButton(button, tooltip, config.HelpUrl, config.Description);
                var commandName = commandType.GetCommandName();
                button.CommandHandler = new RelayCommand(() => RunCommand(commandName), true);
            }
            else
            {
                _buttonService.SetTooltipForButton(button, config.ToolTip, config.HelpUrl, config.Description);
            }

            return button;
        }

        private RibbonSplitButton CreatePullDownButtonInternal(
            PullDownButton config,
            RibbonItemSize size,
            Orientation orientation)
        {
            var forceTextSettings = config.CommandButtonsList.Any(x => !string.IsNullOrWhiteSpace(x.Text));
            var splitButton = _buttonService.CreateNewButtonBase<RibbonSplitButton>(config,
                size,
                orientation,
                forceTextSettings,
                GetIconImage,
                true);

            splitButton.ListStyle = RibbonSplitButtonListStyle.List;
            splitButton.ListButtonStyle = RibbonListButtonStyle.SplitButton;
            splitButton.ListImageSize =
                size == RibbonItemSize.Standard ? RibbonImageSize.Standard : RibbonImageSize.Large;
            splitButton.IsSplit = false;
            splitButton.IsSynchronizedWithCurrentItem = false;

            foreach (var commandButtonConfig in config.CommandButtonsList)
            {
                splitButton.Items.Add(CreateCommandButtonInternal(commandButtonConfig, size, orientation));
            }

            return splitButton;
        }

        private void RunCommand(string commandName)
        {
            var document = Application.DocumentManager.MdiActiveDocument;
            document?.SendStringToExecute($"{commandName} ", false, false, true);
        }
    }
}