namespace RxBim.Application.Ribbon.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.Private.Windows;
    using Autodesk.Windows;
    using GalaSoft.MvvmLight.Command;
    using Shared.Abstractions;
    using Button = Button;

    /// <inheritdoc />
    public class ButtonService : IButtonService
    {
        private readonly IOnlineHelpService _onlineHelpService;
        private readonly IColorThemeService _colorThemeService;
        private readonly IRibbonMenuBuilderFactory _builderFactory;
        private readonly IAboutShowService _aboutShowService;
        private readonly List<(RibbonButton Button, Button Config)> _createdButtons = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonService"/> class.
        /// </summary>
        /// <param name="onlineHelpService"><see cref="IOnlineHelpService"/>.</param>
        /// <param name="colorThemeService"><see cref="IColorThemeService"/>.</param>
        /// <param name="builderFactory"><see cref="IRibbonMenuBuilderFactory"/>.</param>
        /// <param name="aboutShowService"><see cref="IAboutShowService"/>.</param>
        public ButtonService(
            IOnlineHelpService onlineHelpService,
            IColorThemeService colorThemeService,
            IRibbonMenuBuilderFactory builderFactory,
            IAboutShowService aboutShowService)
        {
            _onlineHelpService = onlineHelpService;
            _colorThemeService = colorThemeService;
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
                SetTooltip(button, tooltip, config.HelpUrl, config.Description);
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
                SetTooltip(button, config.ToolTip, config.HelpUrl, config.Description);
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

            foreach (var commandButtonConfig in config.CommandButtonsList)
                splitButton.Items.Add(CreateCommandButton(commandButtonConfig, size, orientation));

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
            var theme = _colorThemeService.GetCurrentTheme();
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
            SetTooltip(ribbonButton, buttonConfig.ToolTip, buttonConfig.HelpUrl, buttonConfig.Description);
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
            SetRibbonItemImages(ribbonButton, buttonConfig, _colorThemeService.GetCurrentTheme());
            _createdButtons.Add((ribbonButton, buttonConfig));
            return ribbonButton;
        }

        private void SetRibbonItemImages(RibbonItem button, Button buttonConfig, ThemeType themeType)
        {
            var builder = _builderFactory.CurrentBuilder;

            if (builder is null)
                return;

            var assembly = buttonConfig is CommandButton commandButton
                ? builder.GetCommandType(commandButton.CommandType!).Assembly
                : null;

            if (themeType is ThemeType.Light)
            {
                button.Image = builder.GetIconImage(buttonConfig.SmallImageLight ?? buttonConfig.SmallImage, assembly);
                button.LargeImage =
                    builder.GetIconImage(buttonConfig.LargeImageLight ?? buttonConfig.LargeImage, assembly);
            }
            else
            {
                button.Image = builder.GetIconImage(buttonConfig.SmallImage, assembly);
                button.LargeImage = builder.GetIconImage(buttonConfig.LargeImage, assembly);
            }
        }

        private void SetTooltip(
            RibbonItem ribbonButton,
            string? tooltipText,
            string? helpUrl,
            string? description)
        {
            var hasToolTip = !string.IsNullOrWhiteSpace(tooltipText);
            var hasHelpUrl = !string.IsNullOrWhiteSpace(helpUrl);

            if (!hasToolTip && !hasHelpUrl)
                return;

            var toolTip = new RibbonToolTip
                { Title = string.IsNullOrWhiteSpace(ribbonButton.Text) ? ribbonButton.Name : ribbonButton.Text };

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

            ribbonButton.ToolTip = toolTip;
        }
    }
}