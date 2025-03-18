namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Linq;
    using Abstractions;
    using Autodesk.Revit.UI;

    /// <inheritdoc />
    internal class RibbonPanelItemService : IRibbonPanelItemService
    {
        private readonly MenuData _menuData;

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonPanelItemService"/> class.
        /// </summary>
        /// <param name="menuData"><see cref="MenuData"/>.</param>
        public RibbonPanelItemService(MenuData menuData)
        {
            _menuData = menuData;
        }

        /// <inheritdoc />
        public RibbonItemData CannotBeStackedStub(IRibbonPanelItem itemConfig)
        {
            throw new InvalidOperationException($"Can't be stacked: {itemConfig.GetType().FullName}");
        }

        /// <inheritdoc />
        public PushButtonData CreateCommandButtonData(CommandButton button)
        {
            button.LoadFromAttribute(_menuData.MenuAssembly);
            CheckButtonName(button);
            if (string.IsNullOrWhiteSpace(button.CommandType))
                throw new ArgumentException($"Command type not found! Button: {button.Name}");
            var cmdType = _menuData.MenuAssembly.GetTypeByName(button.CommandType!);
            var className = cmdType.FullName;
            var assemblyLocation = cmdType.Assembly.Location;
            var pushButtonData =
                new PushButtonData(button.Name, button.Text ?? button.Name, assemblyLocation, className)
                {
                    AvailabilityClassName = className
                };
            SetButtonProperties(pushButtonData, button);
            SetTooltip(pushButtonData, _menuData.GetTooltipContent(button, cmdType));
            return pushButtonData;
        }

        /// <inheritdoc />
        public void CheckButtonName(Button buttonConfig)
        {
            if (string.IsNullOrWhiteSpace(buttonConfig.Name))
                throw new ArgumentException("Button name not found!");
        }

        /// <inheritdoc />
        public void SetTooltip(RibbonItemData buttonData, string? tooltip)
        {
            if (tooltip != null)
                buttonData.ToolTip = tooltip;
        }

        /// <inheritdoc />
        public void SetButtonProperties(ButtonData buttonData, Button buttonConfig)
        {
            var assembly = buttonConfig is CommandButton commandButton
                ? _menuData.MenuAssembly.GetTypeByName(commandButton.CommandType!).Assembly
                : null;

            if (buttonConfig.Text != null)
                buttonData.Text = buttonConfig.Text;
            if (buttonConfig.Description != null)
                buttonData.LongDescription = buttonConfig.Description;
            if (buttonConfig.HelpUrl != null)
                buttonData.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, buttonConfig.HelpUrl));
            buttonData.Image = _menuData.GetIconImage(buttonConfig.Image, assembly);
            buttonData.LargeImage = _menuData.GetIconImage(buttonConfig.LargeImage, assembly);
        }

        /// <inheritdoc />
        public void CreateButtonsForPullDown(PullDownButton config, PulldownButton button)
        {
            foreach (var pushButtonData in config.CommandButtonsList.Select(CreateCommandButtonData))
            {
                button.AddPushButton(pushButtonData);
            }
        }
    }
}