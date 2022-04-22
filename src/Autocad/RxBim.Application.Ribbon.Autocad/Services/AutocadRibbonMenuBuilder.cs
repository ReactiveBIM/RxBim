namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Controls;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.Private.Windows;
    using Autodesk.Windows;
    using GalaSoft.MvvmLight.CommandWpf;
    using RxBim.Application.Ribbon.ConfigurationBuilders;
    using Button = Application.Ribbon.Button;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilder"/> for AutoCAD.
    /// </summary>
    public class AutocadRibbonMenuBuilder : RibbonMenuBuilderBase<RibbonTab, RibbonPanel>
    {
        private readonly Func<ThemeType> _getCurrentTheme;
        private readonly Action _prebuildAction;
        private readonly Action<RibbonToolTip> _toolTipAction;
        private readonly List<(RibbonButton, Button)> _createdButtons = new();

        /// <inheritdoc />
        public AutocadRibbonMenuBuilder(
            Assembly menuAssembly,
            Func<ThemeType> getCurrentTheme,
            Action prebuildAction,
            Action<RibbonToolTip> toolTipAction)
            : base(menuAssembly)
        {
            _getCurrentTheme = getCurrentTheme;
            _prebuildAction = prebuildAction;
            _toolTipAction = toolTipAction;
        }

        /// <summary>
        /// Apply current theme for all menu buttons.
        /// </summary>
        public void ApplyCurrentTheme()
        {
            var theme = _getCurrentTheme();
            _createdButtons.ForEach(x => SetRibbonItemImages(x.Item1, x.Item2, theme));
        }

        /// <inheritdoc />
        protected override void PreBuildActions()
        {
            base.PreBuildActions();
            _createdButtons.Clear();
            _prebuildAction();
        }

        /// <inheritdoc />
        protected override bool CheckRibbonCondition()
        {
            return ComponentManager.Ribbon != null;
        }

        /// <inheritdoc />
        protected override RibbonTab GetOrCreateTab(string title)
        {
            var acRibbonTab = ComponentManager.Ribbon.Tabs.FirstOrDefault(x =>
                x.IsVisible &&
                x.Title != null &&
                x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (acRibbonTab is null)
            {
                acRibbonTab = new RibbonTab
                    { Title = title, Id = $"TAB_{title.GetHashCode():0}" };
                ComponentManager.Ribbon.Tabs.Add(acRibbonTab);
            }

            return acRibbonTab;
        }

        /// <inheritdoc />
        protected override RibbonPanel GetOrCreatePanel(RibbonTab acRibbonTab, string panelName)
        {
            var acRibbonPanel = acRibbonTab.Panels.FirstOrDefault(x =>
                x.Source.Name != null &&
                x.Source.Name.Equals(panelName, StringComparison.OrdinalIgnoreCase));
            if (acRibbonPanel is null)
            {
                acRibbonPanel = new RibbonPanel
                {
                    Source = new RibbonPanelSource
                    {
                        Name = panelName,
                        Title = panelName,
                        Id = $"{acRibbonTab.Id}_PANEL_{panelName.GetHashCode():0}"
                    },
                };

                acRibbonTab.Panels.Add(acRibbonPanel);
            }

            if (acRibbonPanel.GetCurrentRowOrNull() is null)
                acRibbonPanel.AddNewRow();

            return acRibbonPanel;
        }

        /// <inheritdoc />
        protected override void CreateAboutButton(RibbonTab tab, RibbonPanel panel, AboutButton aboutButtonConfig)
        {
            var orientation = aboutButtonConfig.GetOrientation();
            panel.AddToCurrentRow(CreateAboutButtonInternal(aboutButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override void CreateCommandButton(RibbonPanel panel, CommandButton cmdButtonConfig)
        {
            var orientation = cmdButtonConfig.GetOrientation();
            panel.AddToCurrentRow(CreateCommandButtonInternal(cmdButtonConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override void CreatePullDownButton(RibbonPanel panel, PullDownButton pullDownButtonConfig)
        {
            var orientation = pullDownButtonConfig.GetOrientation();
            panel.AddToCurrentRow(CreatePullDownButtonInternal(pullDownButtonConfig,
                RibbonItemSize.Large,
                orientation));
        }

        /// <inheritdoc />
        protected override void AddSeparator(RibbonPanel panel)
        {
            panel.AddToCurrentRow(new RibbonSeparator());
        }

        /// <inheritdoc />
        protected override void AddSlideOut(RibbonPanel panel)
        {
            if (panel.HasSlideOut())
                return;

            panel.Source.Items.Add(new RibbonPanelBreak());
            panel.AddNewRow();
        }

        /// <inheritdoc />
        protected override void CreateStackedItems(RibbonPanel panel, StackedItems stackedItems)
        {
            var stackSize = stackedItems.StackedButtons.Count;
            var stackedItemsRow = new RibbonRowPanel();
            var size = stackSize == StackedItemsBuilder.MaxStackSize
                ? RibbonItemSize.Standard
                : RibbonItemSize.Large;

            panel.AddToCurrentRow(stackedItemsRow);

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
                    CommandButton cmdButton => CreateCommandButtonInternal(cmdButton, size, Orientation.Horizontal),
                    PullDownButton pullDownButton => CreatePullDownButtonInternal(pullDownButton,
                        size,
                        Orientation.Horizontal),
                    _ => throw new ArgumentOutOfRangeException($"Unknown button type: {buttonConfig.GetType().Name}")
                };

                stackedItemsRow.Items.Add(buttonItem);
            }
        }

        private T CreateNewButton<T>(
            Button buttonConfig,
            RibbonItemSize size,
            Orientation orientation,
            bool forceTextSettings = false)
            where T : RibbonButton, new()
        {
            var ribbonButton = CreateNewButtonBase<T>(buttonConfig, size, orientation, forceTextSettings);
            ribbonButton.SetTooltip(buttonConfig.ToolTip,
                buttonConfig.HelpUrl,
                buttonConfig.Description,
                _toolTipAction);
            return ribbonButton;
        }

        private T CreateNewButtonBase<T>(
            Button buttonConfig,
            RibbonItemSize size,
            Orientation orientation,
            bool forceTextSettings = false)
            where T : RibbonButton, new()
        {
            var ribbonButton = new T();
            ribbonButton.SetProperties(buttonConfig, size, orientation, forceTextSettings);
            SetRibbonItemImages(ribbonButton, buttonConfig, _getCurrentTheme());
            _createdButtons.Add((ribbonButton, buttonConfig));
            return ribbonButton;
        }

        private void SetRibbonItemImages(RibbonItem button, Button buttonConfig, ThemeType themeType)
        {
            if (themeType is ThemeType.Light)
            {
                button.Image = GetIconImage(buttonConfig.SmallImageLight ?? buttonConfig.SmallImage);
                button.LargeImage = GetIconImage(buttonConfig.LargeImageLight ?? buttonConfig.LargeImage);
            }
            else
            {
                button.Image = GetIconImage(buttonConfig.SmallImage);
                button.LargeImage = GetIconImage(buttonConfig.LargeImage);
            }
        }

        private RibbonButton CreateAboutButtonInternal(
            AboutButton aboutButtonConfig,
            RibbonItemSize size,
            Orientation orientation)
        {
            var button = CreateNewButton<RibbonButton>(aboutButtonConfig, size, orientation);
            button.CommandHandler = new RelayCommand(() =>
                {
                    if (!TryShowAboutWindow(aboutButtonConfig.Content))
                        Application.ShowAlertDialog(aboutButtonConfig.Content.ToString());
                },
                true);
            return button;
        }

        private RibbonButton CreateCommandButtonInternal(
            CommandButton buttonConfig,
            RibbonItemSize size,
            Orientation orientation)
        {
            var button = CreateNewButtonBase<RibbonButton>(buttonConfig, size, orientation);
            if (!string.IsNullOrWhiteSpace(buttonConfig.CommandType))
            {
                var commandType = GetCommandType(buttonConfig.CommandType!);
                var tooltip = GetTooltipContent(buttonConfig, commandType);
                button.SetTooltip(tooltip, buttonConfig.HelpUrl, buttonConfig.Description, _toolTipAction);
                var commandName = commandType.GetCommandName();
                button.CommandHandler = new RelayCommand(() =>
                    {
                        Application.DocumentManager.MdiActiveDocument?
                            .SendStringToExecute($"{commandName} ", false, false, true);
                    },
                    true);
            }
            else
            {
                button.SetTooltip(buttonConfig.ToolTip,
                    buttonConfig.HelpUrl,
                    buttonConfig.Description,
                    _toolTipAction);
            }

            return button;
        }

        private RibbonButton CreatePullDownButtonInternal(
            PullDownButton pullDownButtonConfig,
            RibbonItemSize size,
            Orientation orientation)
        {
            var forceTextSettings =
                pullDownButtonConfig.CommandButtonsList.Any(x => !string.IsNullOrWhiteSpace(x.Text));
            var splitButton =
                CreateNewButton<RibbonSplitButton>(pullDownButtonConfig, size, orientation, forceTextSettings);

            splitButton.ListStyle = RibbonSplitButtonListStyle.List;
            splitButton.ListButtonStyle = RibbonListButtonStyle.SplitButton;
            splitButton.ListImageSize =
                size == RibbonItemSize.Standard ? RibbonImageSize.Standard : RibbonImageSize.Large;
            splitButton.IsSplit = false;
            splitButton.IsSynchronizedWithCurrentItem = false;

            foreach (var commandButtonConfig in pullDownButtonConfig.CommandButtonsList)
            {
                splitButton.Items.Add(CreateCommandButtonInternal(commandButtonConfig, size, orientation));
            }

            return splitButton;
        }
    }
}