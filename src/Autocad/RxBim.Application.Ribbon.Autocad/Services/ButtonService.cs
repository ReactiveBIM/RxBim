﻿namespace RxBim.Application.Ribbon.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.Private.Windows;
    using Autodesk.Windows;
    using Helpers;
    using Button = Button;

    /// <inheritdoc />
    public class ButtonService : IButtonService
    {
        private readonly MenuData _menuData;
        private readonly IOnlineHelpService _onlineHelpService;
        private readonly IColorThemeService _colorThemeService;
        private readonly List<(RibbonButton Button, Button Config)> _createdButtons = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonService"/> class.
        /// </summary>
        /// <param name="menuData"><see cref="MenuData"/>.</param>
        /// <param name="onlineHelpService"><see cref="IOnlineHelpService"/>.</param>
        /// <param name="colorThemeService"><see cref="IColorThemeService"/>.</param>
        public ButtonService(
            MenuData menuData,
            IOnlineHelpService onlineHelpService,
            IColorThemeService colorThemeService)
        {
            _menuData = menuData;
            _onlineHelpService = onlineHelpService;
            _colorThemeService = colorThemeService;
        }

        /// <inheritdoc/>
        public T CreateNewButton<T>(
            Button config,
            RibbonItemSize size,
            Orientation orientation,
            bool forceTextSettings,
            bool addToolTip)
            where T : RibbonButton, new()
        {
            var ribbonButton = new T();
            ribbonButton.SetProperties(config, size, orientation, forceTextSettings);
            SetRibbonItemImages(ribbonButton, config);
            _createdButtons.Add((ribbonButton, config));
            if (addToolTip)
                SetTooltip(ribbonButton, config.ToolTip, config.HelpUrl, config.Description);
            return ribbonButton;
        }

        /// <inheritdoc />
        public RibbonButton CreateCommandButton(
            CommandButton config,
            RibbonItemSize size,
            Orientation? orientation = null)
        {
            config.LoadFromAttribute(_menuData.MenuAssembly);

            var orientationValue = orientation ?? config.GetOrientation();

            var button = CreateNewButton<RibbonButton>(config, size, orientationValue, false, false);

            if (!string.IsNullOrWhiteSpace(config.CommandType))
            {
                var commandType = _menuData.MenuAssembly.GetTypeByName(config.CommandType!);
                var tooltip = _menuData.GetTooltipContent(config, commandType);
                SetTooltip(button, tooltip, config.HelpUrl, config.Description);
                var commandName = commandType.GetCommandName();
                button.CommandHandler = new RelayCommand(() => RunCommand(commandName), () => true);
            }
            else
            {
                SetTooltip(button, config.ToolTip, config.HelpUrl, config.Description);
            }

            return button;
        }

        /// <inheritdoc/>
        public RibbonSplitButton CreatePullDownButton(
            PullDownButton config,
            RibbonItemSize size,
            Orientation orientation)
        {
            var forceTextSettings = config.CommandButtonsList.Any(x => !string.IsNullOrWhiteSpace(x.Text));
            var splitButton = CreateNewButton<RibbonSplitButton>(config,
                size,
                orientation,
                forceTextSettings,
                true);

            splitButton.ListStyle = RibbonSplitButtonListStyle.List;
            splitButton.ListButtonStyle = RibbonListButtonStyle.SplitButton;
            splitButton.ListImageSize =
                size == RibbonItemSize.Standard ? RibbonImageSize.Standard : RibbonImageSize.Large;
            splitButton.IsSplit = false;
            splitButton.IsSynchronizedWithCurrentItem = false;

            foreach (var commandButtonConfig in config.CommandButtonsList)
            {
                splitButton.Items.Add(CreateCommandButton(commandButtonConfig, size, orientation));
            }

            return splitButton;
        }

        /// <inheritdoc />
        public void ClearButtonCache()
        {
            _createdButtons.Clear();
        }

        /// <inheritdoc />
        public void ApplyCurrentTheme()
        {
            _createdButtons.ForEach(x => SetRibbonItemImages(x.Button, x.Config));
        }

        private void SetTooltip(RibbonItem button, string? tooltipText, string? helpUrl, string? description)
        {
            var hasToolTip = !string.IsNullOrWhiteSpace(tooltipText);
            var hasHelpUrl = !string.IsNullOrWhiteSpace(helpUrl);

            if (!hasToolTip && !hasHelpUrl)
                return;

            var toolTip = new RibbonToolTip
                { Title = string.IsNullOrWhiteSpace(button.Text) ? button.Name : button.Text };

            if (!string.IsNullOrWhiteSpace(description))
                toolTip.ExpandedContent = description;

            if (hasToolTip)
                toolTip.Content = tooltipText;

            if (hasHelpUrl)
            {
                toolTip.HelpTopic = helpUrl;
                toolTip.IsHelpEnabled = true;
            }
            else
            {
                toolTip.IsHelpEnabled = false;
            }

            _onlineHelpService.AddToolTip(toolTip);

            button.ToolTip = toolTip;
        }

        private void SetRibbonItemImages(RibbonItem button, Button buttonConfig)
        {
            var assembly = buttonConfig is CommandButton commandButton
                ? _menuData.MenuAssembly.GetTypeByName(commandButton.CommandType!).Assembly
                : null;

            var themeType = _colorThemeService.GetCurrentTheme();

            if (themeType is ThemeType.Light)
            {
                button.Image =
                    _menuData.GetIconImage(buttonConfig.ImageLight ?? buttonConfig.Image, assembly);
                button.LargeImage =
                    _menuData.GetIconImage(buttonConfig.LargeImageLight ?? buttonConfig.LargeImage, assembly);
            }
            else
            {
                button.Image = _menuData.GetIconImage(buttonConfig.Image, assembly);
                button.LargeImage = _menuData.GetIconImage(buttonConfig.LargeImage, assembly);
            }
        }

        private void RunCommand(string commandName)
        {
            var document = Application.DocumentManager.MdiActiveDocument;
            document?.SendStringToExecute($"{commandName} ", false, false, true);
        }
    }
}