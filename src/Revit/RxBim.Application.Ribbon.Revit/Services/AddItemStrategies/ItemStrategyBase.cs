namespace RxBim.Application.Ribbon.Services.AddItemStrategies
{
    using System;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Basic implementation of <see cref="IAddItemStrategy"/> for Revit menu item.
    /// </summary>
    public abstract class ItemStrategyBase<TItem> : IAddItemStrategy
        where TItem : IRibbonPanelItem
    {
        private readonly MenuData _menuData;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemStrategyBase{T}"/> class.
        /// </summary>
        /// <param name="menuData"><see cref="MenuData"/>.</param>
        protected ItemStrategyBase(MenuData menuData)
        {
            _menuData = menuData;
        }

        /// <inheritdoc />
        public virtual bool IsApplicable(IRibbonPanelItem config)
        {
            return config is TItem;
        }

        /// <inheritdoc />
        public void AddItem(object tab, object panel, IRibbonPanelItem config)
        {
            if (tab is not string tabName || panel is not RibbonPanel ribbonPanel || config is not TItem itemConfig)
                return;

            AddItem(tabName, ribbonPanel, itemConfig);
        }

        /// <inheritdoc />
        public object GetItemForStack(IRibbonPanelItem config, bool small = false)
        {
            if (config is not TItem itemConfig)
                throw new InvalidOperationException($"Invalid config type: {config.GetType().FullName}");

            return GetItemForStack(itemConfig);
        }

        /// <summary>
        /// Creates and adds to ribbon an item.
        /// </summary>
        /// <param name="tabName">Ribbon tab name.</param>
        /// <param name="ribbonPanel">Ribbon panel.</param>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        protected abstract void AddItem(string tabName, RibbonPanel ribbonPanel, TItem itemConfig);

        /// <summary>
        /// Creates and returns an item for a stack.
        /// </summary>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        protected abstract RibbonItemData GetItemForStack(TItem itemConfig);

        /// <summary>
        /// Stub for GetItemForStack, if item can't be stacked.
        /// </summary>
        /// <param name="itemConfig">Ribbon item configuration.</param>
        protected RibbonItemData CannotBeStackedStub(TItem itemConfig)
        {
            throw new InvalidOperationException($"Can't be stacked: {itemConfig.GetType().FullName}");
        }

        /// <summary>
        /// Creates and returns a command button.
        /// </summary>
        /// <param name="config">Command button configuration.</param>
        protected PushButtonData CreateCommandButtonData(CommandButton config)
        {
            CheckButtonName(config);
            if (string.IsNullOrWhiteSpace(config.CommandType))
                throw new ArgumentException($"Command type not found! Button: {config.Name}");
            var cmdType = _menuData.MenuAssembly.GetTypeByName(config.CommandType!);
            var className = cmdType.FullName;
            var assemblyLocation = cmdType.Assembly.Location;
            var pushButtonData =
                new PushButtonData(config.Name, config.Text ?? config.Name, assemblyLocation, className)
                {
                    AvailabilityClassName = className
                };
            SetButtonProperties(pushButtonData, config);
            SetTooltip(pushButtonData, _menuData.GetTooltipContent(config, cmdType));
            return pushButtonData;
        }

        /// <summary>
        /// Checks button name. If name is not set, throws exception.
        /// </summary>
        /// <param name="buttonConfig">Button configuration.</param>
        protected void CheckButtonName(Button buttonConfig)
        {
            if (string.IsNullOrWhiteSpace(buttonConfig.Name))
                throw new ArgumentException("Button name not found!");
        }

        /// <summary>
        /// Sets tooltip.
        /// </summary>
        /// <param name="buttonData">Button data.</param>
        /// <param name="tooltip">Tooltip content.</param>
        protected void SetTooltip(RibbonItemData buttonData, string? tooltip)
        {
            if (tooltip != null)
                buttonData.ToolTip = tooltip;
        }

        /// <summary>
        /// Sets the general properties of the button.
        /// </summary>
        /// <param name="buttonData">Button data.</param>
        /// <param name="buttonConfig">Button configuration.</param>
        protected void SetButtonProperties(ButtonData buttonData, Button buttonConfig)
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
            buttonData.Image = _menuData.GetIconImage(buttonConfig.SmallImage, assembly);
            buttonData.LargeImage = _menuData.GetIconImage(buttonConfig.LargeImage, assembly);
        }
    }
}