namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using Abstractions;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.Private.Windows;
    using Autodesk.Windows;
    using Extensions;
    using GalaSoft.MvvmLight.Command;
    using Ribbon.Abstractions;
    using Ribbon.Models;
    using Ribbon.Models.Configurations;
    using Shared.Abstractions;
    using Button = Ribbon.Models.Configurations.Button;

    /// <inheritdoc />
    public class ButtonService : IButtonService
    {
        private readonly IOnlineHelpService _onlineHelpService;
        private readonly IThemeService _themeService;
        private readonly IRibbonMenuBuilderFactory _builderFactory;
        private readonly IAboutShowService _aboutShowService;
        private readonly List<(RibbonButton Button, Button Config)> _createdButtons = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonService"/> class.
        /// </summary>
        /// <param name="onlineHelpService"><see cref="IOnlineHelpService"/>.</param>
        /// <param name="themeService"><see cref="IThemeService"/>.</param>
        /// <param name="builderFactory"><see cref="IRibbonMenuBuilderFactory"/>.</param>
        /// <param name="aboutShowService"><see cref="IAboutShowService"/>.</param>
        public ButtonService(
            IOnlineHelpService onlineHelpService,
            IThemeService themeService,
            IRibbonMenuBuilderFactory builderFactory,
            IAboutShowService aboutShowService)
        {
            _onlineHelpService = onlineHelpService;
            _themeService = themeService;
            _builderFactory = builderFactory;
            _aboutShowService = aboutShowService;
        }

        /// <inheritdoc />
        public RibbonButton CreateAboutButton(
            AboutButton config,
            RibbonItemSize size,
            Orientation orientation)
        {
            var button = CreateNewButton<RibbonButton>(config, size, orientation);
            button.CommandHandler = new RelayCommand(() => _aboutShowService.ShowAboutBox(config.Content), true);
            return button;
        }

        /// <inheritdoc />
        public RibbonButton CreateCommandButton(
            CommandButton config,
            RibbonItemSize size,
            Orientation orientation)
        {
            var button = CreateNewButtonBase<RibbonButton>(config, size, orientation);
            var builder = _builderFactory.CurrentBuilder;

            if (!string.IsNullOrWhiteSpace(config.CommandType) && builder != null)
            {
                var commandType = builder.GetCommandType(config.CommandType!);
                var tooltip = builder.GetTooltipContent(config, commandType);
                button.SetTooltipForButton(tooltip,
                    config.HelpUrl,
                    config.Description,
                    _onlineHelpService.AddToolTip);
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
                button.SetTooltipForButton(config.ToolTip,
                    config.HelpUrl,
                    config.Description,
                    _onlineHelpService.AddToolTip);
            }

            return button;
        }

        /// <inheritdoc />
        public RibbonSplitButton CreatePullDownButton(
            PullDownButton config,
            RibbonItemSize size,
            Orientation orientation)
        {
            var forceTextSettings =
                config.CommandButtonsList.Any(x => !string.IsNullOrWhiteSpace(x.Text));
            var splitButton =
                CreateNewButton<RibbonSplitButton>(config, size, orientation, forceTextSettings);

            splitButton.ListStyle = RibbonSplitButtonListStyle.List;
            splitButton.ListButtonStyle = RibbonListButtonStyle.SplitButton;
            splitButton.ListImageSize =
                size == RibbonItemSize.Standard ? RibbonImageSize.Standard : RibbonImageSize.Large;
            splitButton.IsSplit = false;
            splitButton.IsSynchronizedWithCurrentItem = false;
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
            var theme = _themeService.GetCurrentTheme();
            _createdButtons.ForEach(x => SetRibbonItemImages(x.Button, x.Config, theme));
        }

        private T CreateNewButton<T>(
            Button buttonConfig,
            RibbonItemSize size,
            Orientation orientation,
            bool forceTextSettings = false)
            where T : RibbonButton, new()
        {
            var ribbonButton = CreateNewButtonBase<T>(buttonConfig, size, orientation, forceTextSettings);
            ribbonButton.SetTooltipForButton(buttonConfig.ToolTip,
                buttonConfig.HelpUrl,
                buttonConfig.Description,
                _onlineHelpService.AddToolTip);
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
            ribbonButton.SetButtonProperties(buttonConfig, size, orientation, forceTextSettings);
            SetRibbonItemImages(ribbonButton, buttonConfig, _themeService.GetCurrentTheme());
            _createdButtons.Add((ribbonButton, buttonConfig));
            return ribbonButton;
        }

        private void SetRibbonItemImages(RibbonItem button, Button buttonConfig, ThemeType themeType)
        {
            var builder = _builderFactory.CurrentBuilder;

            if (builder is null)
                return;

            if (themeType is ThemeType.Light)
            {
                button.Image = builder.GetIconImage(buttonConfig.SmallImageLight ?? buttonConfig.SmallImage);
                button.LargeImage = builder.GetIconImage(buttonConfig.LargeImageLight ?? buttonConfig.LargeImage);
            }
            else
            {
                button.Image = builder.GetIconImage(buttonConfig.SmallImage);
                button.LargeImage = builder.GetIconImage(buttonConfig.LargeImage);
            }
        }
    }
}