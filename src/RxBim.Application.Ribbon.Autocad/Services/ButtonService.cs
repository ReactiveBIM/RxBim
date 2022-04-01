namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using Abstractions;
    using Autodesk.Windows;
    using Extensions;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Models.Configurations;
    using Shared.Abstractions;
    using Button = Models.Configurations.Button;

    /// <inheritdoc />
    internal class ButtonService : IButtonService
    {
        private readonly IOnlineHelpService _onlineHelpService;
        private readonly IColorThemeService _colorThemeService;
        private readonly IAboutShowService _aboutShowService;
        private readonly List<(RibbonButton Button, Button Config)> _createdButtons = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonService"/> class.
        /// </summary>
        /// <param name="onlineHelpService"><see cref="IOnlineHelpService"/>.</param>
        /// <param name="colorThemeService"><see cref="IColorThemeService"/>.</param>
        /// <param name="aboutShowService"><see cref="IAboutShowService"/>.</param>
        public ButtonService(
            IOnlineHelpService onlineHelpService,
            IColorThemeService colorThemeService,
            IAboutShowService aboutShowService)
        {
            _onlineHelpService = onlineHelpService;
            _colorThemeService = colorThemeService;
            _aboutShowService = aboutShowService;
        }

        /// <inheritdoc/>
        public T CreateNewButtonBase<T>(
            Button config,
            RibbonItemSize size,
            Orientation orientation,
            bool forceTextSettings,
            Func<string?, BitmapImage?> getImage,
            bool addToolTip)
            where T : RibbonButton, new()
        {
            var ribbonButton = new T();
            ribbonButton.SetButtonProperties(config, size, orientation, forceTextSettings);
            SetRibbonItemImages(ribbonButton, config, getImage);
            _createdButtons.Add((ribbonButton, config));
            if (addToolTip)
                SetTooltipForButton(ribbonButton, config.ToolTip, config.HelpUrl, config.Description);
            return ribbonButton;
        }

        /// <inheritdoc/>
        public void SetTooltipForButton(RibbonItem button, string? tooltipText, string? helpUrl, string? description)
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

        /// <inheritdoc />
        public RibbonButton CreateAboutButton(
            AboutButton config,
            RibbonItemSize size,
            Orientation orientation,
            Func<string?, BitmapImage?> getImage)
        {
            var button = CreateNewButtonBase<RibbonButton>(config, size, orientation, false, getImage, true);
            button.CommandHandler = new RelayCommand(() => _aboutShowService.ShowAboutBox(config.Content), true);
            return button;
        }

        /// <inheritdoc />
        public void ClearButtonCache()
        {
            _createdButtons.Clear();
        }

        /// <inheritdoc/>
        public void ApplyCurrentTheme(Func<string?, BitmapImage?> getImage)
        {
            _createdButtons.ForEach(x => SetRibbonItemImages(x.Button, x.Config, getImage));
        }

        private void SetRibbonItemImages(RibbonItem button, Button buttonConfig, Func<string?, BitmapImage?> getImage)
        {
            var themeType = _colorThemeService.GetCurrentTheme();
            if (themeType is ThemeType.Light)
            {
                button.Image = getImage(buttonConfig.SmallImageLight ?? buttonConfig.SmallImage);
                button.LargeImage = getImage(buttonConfig.LargeImageLight ?? buttonConfig.LargeImage);
            }
            else
            {
                button.Image = getImage(buttonConfig.SmallImage);
                button.LargeImage = getImage(buttonConfig.LargeImage);
            }
        }
    }
}