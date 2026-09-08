namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using System.Windows.Controls;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.Windows;
    using Helpers;
    using JetBrains.Annotations;
    using RibbonItem = Autodesk.Windows.RibbonItem;

    /// <summary>
    /// Strategy for adding a <see cref="ToggleCommandButton"/> to the AutoCAD ribbon.
    /// </summary>
    [UsedImplicitly]
    public class ToggleCommandButtonStrategy : ItemStrategyBase<ToggleCommandButton>
    {
        private readonly IPanelService _panelService;
        private readonly IButtonService _buttonService;
        private readonly MenuData _menuData;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleCommandButtonStrategy"/> class.
        /// </summary>
        /// <param name="panelService"><see cref="IPanelService"/>.</param>
        /// <param name="buttonService"><see cref="IButtonService"/>.</param>
        /// <param name="menuData"><see cref="MenuData"/>.</param>
        public ToggleCommandButtonStrategy(
            IPanelService panelService,
            IButtonService buttonService,
            MenuData menuData)
        {
            _panelService = panelService;
            _buttonService = buttonService;
            _menuData = menuData;
        }

        /// <inheritdoc />
        protected override void AddItem(RibbonTab ribbonTab, RibbonPanel ribbonPanel, ToggleCommandButton itemConfig)
        {
            var orientation = itemConfig.GetOrientation();
            _panelService.AddItem(ribbonPanel, CreateToggleButton(itemConfig, RibbonItemSize.Large, orientation));
        }

        /// <inheritdoc />
        protected override RibbonItem GetItemForStack(ToggleCommandButton itemConfig, RibbonItemSize size)
        {
            return CreateToggleButton(itemConfig, size, Orientation.Horizontal);
        }

        private RibbonToggleButton CreateToggleButton(
            ToggleCommandButton config,
            RibbonItemSize size,
            Orientation orientation)
        {
            config.LoadFromAttribute(_menuData.MenuAssembly);

            var button = _buttonService.CreateNewButton<RibbonToggleButton>(
                config,
                size,
                orientation,
                false,
                true);

            button.IsThreeState = config.IsThreeState;
            button.IsCheckable = true;
            button.IsChecked = config.IsChecked;

            if (string.IsNullOrWhiteSpace(config.CommandType))
                return button;

            var commandType = _menuData.MenuAssembly.GetTypeByName(config.CommandType!);
            var commandName = commandType.GetCommandName();
            button.CommandHandler = new RelayCommand(() => RunCommand(commandName), () => true);

            return button;
        }

        private void RunCommand(string commandName)
        {
            var document = Application.DocumentManager.MdiActiveDocument;
            document?.SendStringToExecute($"{commandName} ", false, false, true);
        }
    }
}