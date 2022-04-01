namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Controls;
    using Abstractions;
    using Autodesk.Windows;
    using Extensions;
    using Models.Configurations;
    using Ribbon.Abstractions;
    using Ribbon.Services;
    using Ribbon.Services.ConfigurationBuilders;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilder"/> for AutoCAD
    /// </summary>
    public class AutocadRibbonMenuBuilder : RibbonMenuBuilderBase<RibbonTab, RibbonPanel>
    {
        private readonly IOnlineHelpService _onlineHelpService;
        private readonly IPanelService _panelService;
        private readonly IButtonService _buttonService;

        /// <inheritdoc />
        public AutocadRibbonMenuBuilder(
            Assembly menuAssembly,
            IOnlineHelpService onlineHelpService,
            IPanelService panelService,
            IButtonService buttonService)
            : base(menuAssembly)
        {
            _panelService = panelService;
            _buttonService = buttonService;
            _onlineHelpService = onlineHelpService;
        }

        /// <inheritdoc />
        protected override void PreBuildActions()
        {
            base.PreBuildActions();
            _buttonService.ClearButtonCache();
            _onlineHelpService.ClearToolTipsCache();
        }

        /// <inheritdoc />
        protected override bool CheckRibbonCondition() => ComponentManager.Ribbon != null;

        /// <inheritdoc />
        protected override RibbonTab GetOrCreateTab(string tabName)
        {
            var acRibbonTab = ComponentManager.Ribbon.Tabs.FirstOrDefault(x =>
                x.IsVisible &&
                x.Title != null &&
                x.Title.Equals(tabName, StringComparison.OrdinalIgnoreCase));

            if (acRibbonTab is not null)
                return acRibbonTab;

            acRibbonTab = new RibbonTab
            {
                Title = tabName,
                Id = $"TAB_{tabName.GetHashCode():0}"
            };

            ComponentManager.Ribbon.Tabs.Add(acRibbonTab);

            return acRibbonTab;
        }

        /// <inheritdoc />
        protected override RibbonPanel GetOrCreatePanel(RibbonTab acRibbonTab, string panelName) =>
            _panelService.GetOrCreatePanel(acRibbonTab, panelName);

        /// <inheritdoc />
        protected override void CreateAboutButton(RibbonTab tab, RibbonPanel panel, AboutButton aboutButtonConfig)
        {
            var orientation = aboutButtonConfig.GetSingleLargeButtonOrientation();
            _panelService.AddItem(panel,
                _buttonService.CreateAboutButton(aboutButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override void CreateCommandButton(RibbonPanel panel, CommandButton cmdButtonConfig)
        {
            var orientation = cmdButtonConfig.GetSingleLargeButtonOrientation();
            _panelService.AddItem(panel,
                _buttonService.CreateCommandButton(cmdButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override void CreatePullDownButton(RibbonPanel panel, PullDownButton pullDownButtonConfig)
        {
            var orientation = pullDownButtonConfig.GetSingleLargeButtonOrientation();
            _panelService.AddItem(panel,
                _buttonService.CreatePullDownButton(pullDownButtonConfig, RibbonItemSize.Large, orientation));
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
                {
                    stackedItemsRow.Items.Add(new RibbonRowBreak());
                }

                var buttonConfig = stackedItems.StackedButtons[i];
                var buttonItem = buttonConfig switch
                {
                    AboutButton aboutButton =>
                        _buttonService.CreateAboutButton(aboutButton, size, Orientation.Horizontal),
                    CommandButton cmdButton =>
                        _buttonService.CreateCommandButton(cmdButton, size, Orientation.Horizontal),
                    PullDownButton pullDownButton =>
                        _buttonService.CreatePullDownButton(pullDownButton, size, Orientation.Horizontal),
                    _ => throw new ArgumentOutOfRangeException($"Unknown button type: {buttonConfig.GetType().Name}")
                };

                stackedItemsRow.Items.Add(buttonItem);
            }
        }
    }
}