namespace RxBim.Application.Ribbon.Services
{
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
        private readonly IThemedRibbonButtonService<RibbonButton> _themedButtonService;
        private readonly IAutocadEnhancedToolTipService? _enhancedToolTipService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonService"/> class.
        /// </summary>
        /// <param name="menuData"><see cref="MenuData"/>.</param>
        /// <param name="onlineHelpService"><see cref="IOnlineHelpService"/>.</param>
        /// <param name="themedButtonService"><see cref="IThemedRibbonButtonService{TButton}"/>.</param>
        public ButtonService(
            MenuData menuData,
            IOnlineHelpService onlineHelpService,
            IThemedRibbonButtonService<RibbonButton> themedButtonService)
            : this(menuData, onlineHelpService, themedButtonService, null)
        {
        }

        /// <summary>
        /// Initializes a button service with optional interactive tooltips.
        /// </summary>
        /// <param name="menuData">The menu metadata.</param>
        /// <param name="onlineHelpService">The native online help service.</param>
        /// <param name="themedButtonService">The button image service.</param>
        /// <param name="enhancedToolTipService">The interactive tooltip adapter.</param>
        public ButtonService(
            MenuData menuData,
            IOnlineHelpService onlineHelpService,
            IThemedRibbonButtonService<RibbonButton> themedButtonService,
            IAutocadEnhancedToolTipService? enhancedToolTipService)
        {
            _menuData = menuData;
            _onlineHelpService = onlineHelpService;
            _themedButtonService = themedButtonService;
            _enhancedToolTipService = enhancedToolTipService;
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
            _themedButtonService.Register(ribbonButton, config);

            if (addToolTip)
            {
                SetTooltip(ribbonButton, config.ToolTip, config.HelpUrl, config.Description);
                _enhancedToolTipService?.Register(ribbonButton, config);
            }

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

            _enhancedToolTipService?.Register(button, config);
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
                var commandButton = CreateCommandButton(commandButtonConfig, size, orientation);
                splitButton.Items.Add(commandButton);
                _enhancedToolTipService?.Register(commandButton, commandButtonConfig, splitButton);
            }

            return splitButton;
        }

        /// <inheritdoc />
        public void ClearButtonCache()
        {
            _themedButtonService.Clear();
            _enhancedToolTipService?.Clear();
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

        private void RunCommand(string commandName)
        {
            var document = Application.DocumentManager.MdiActiveDocument;
            document?.SendStringToExecute($"{commandName} ", false, false, true);
        }
    }
}