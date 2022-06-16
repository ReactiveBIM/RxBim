﻿namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Controls;
    using Autodesk.Windows;
    using ConfigurationBuilders;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilder"/> for AutoCAD.
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
        protected override RibbonTab GetOrCreateTab(string title)
        {
            var acRibbonTab = ComponentManager.Ribbon.Tabs.FirstOrDefault(x =>
                x.IsVisible &&
                x.Title != null &&
                x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (acRibbonTab is not null)
                return acRibbonTab;

            acRibbonTab = new RibbonTab
            {
                Title = title,
                Id = $"TAB_{title.GetHashCode():0}"
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
            var orientation = aboutButtonConfig.GetOrientation();
            _panelService.AddItem(panel,
                CreateAboutButtonInternal(aboutButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override void CreateCommandButton(RibbonPanel panel, CommandButton cmdButtonConfig)
        {
            var orientation = cmdButtonConfig.GetOrientation();
            _panelService.AddItem(panel,
                _buttonService.CreateCommandButton(cmdButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override void CreatePullDownButton(RibbonPanel panel, PullDownButton pullDownButtonConfig)
        {
            var orientation = pullDownButtonConfig.GetOrientation();
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
                {
                    stackedItemsRow.Items.Add(new RibbonRowBreak());
                }

                var buttonConfig = stackedItems.StackedButtons[i];
                var buttonItem = buttonConfig switch
                {
                    AboutButton aboutButton => CreateAboutButtonInternal(aboutButton, size, Orientation.Horizontal),
                    CommandButton cmdButton => _buttonService.CreateCommandButton(cmdButton,
                        size,
                        Orientation.Horizontal),
                    PullDownButton pullDownButton => CreatePullDownButtonInternal(pullDownButton,
                        size,
                        Orientation.Horizontal),
                    _ => throw new ArgumentOutOfRangeException($"Unknown button type: {buttonConfig.GetType().Name}")
                };

                stackedItemsRow.Items.Add(buttonItem);
            }
        }

        private RibbonItem CreateAboutButtonInternal(AboutButton config, RibbonItemSize large, Orientation orientation)
        {
            return _buttonService.CreateAboutButton(config, large, orientation);
        }

        private RibbonButton CreatePullDownButtonInternal(
            PullDownButton pullDownButtonConfig,
            RibbonItemSize size,
            Orientation orientation)
        {
            var splitButton = _buttonService.CreatePullDownButton(pullDownButtonConfig, size, orientation);

            foreach (var commandButtonConfig in pullDownButtonConfig.CommandButtonsList)
            {
                splitButton.Items.Add(
                    _buttonService.CreateCommandButton(commandButtonConfig, size, orientation));
            }

            return splitButton;
        }
    }
}