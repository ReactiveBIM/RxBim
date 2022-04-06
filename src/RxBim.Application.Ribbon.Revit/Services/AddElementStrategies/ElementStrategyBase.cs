namespace RxBim.Application.Ribbon.Revit.Services.AddElementStrategies
{
    using Abstractions;
    using Abstractions.ConfigurationBuilders;
    using Autodesk.Revit.UI;
    using Models;
    using Models.Configurations;
    using Ribbon.Extensions;

    /// <summary>
    /// Basic implementation of <see cref="IAddElementStrategy"/> for Revit menu item.
    /// </summary>
    public abstract class ElementStrategyBase<TElement> : IAddElementStrategy
        where TElement : IRibbonPanelElement
    {
        private readonly MenuData _menuData;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementStrategyBase{TElement}"/> class.
        /// </summary>
        /// <param name="menuData"><see cref="MenuData"/>.</param>
        protected ElementStrategyBase(MenuData menuData)
        {
            _menuData = menuData;
        }

        /// <inheritdoc />
        public virtual bool IsApplicable(IRibbonPanelElement config)
        {
            return config is TElement;
        }

        /// <inheritdoc />
        public void CreateAndAddElement(object panel, IRibbonPanelElement config)
        {
            if (panel is not RibbonPanel ribbonPanel || config is not TElement elementConfig)
                return;

            CreateAndAddElement(ribbonPanel, elementConfig);
        }

        /// <inheritdoc />
        public object CreateElementForStack(IRibbonPanelElement config, bool small = false)
        {
            if (config is not TElement elementConfig)
                throw new System.InvalidOperationException($"Invalid config type: {config.GetType().FullName}");

            return CreateElementForStack(elementConfig);
        }

        /// <summary>
        /// Creates and adds to ribbon an element.
        /// </summary>
        /// <param name="ribbonPanel">Ribbon panel.</param>
        /// <param name="elementConfig">Ribbon item configuration.</param>
        protected abstract void CreateAndAddElement(RibbonPanel ribbonPanel, TElement elementConfig);

        /// <summary>
        /// Creates and returns an element for a stack.
        /// </summary>
        /// <param name="elementConfig">Ribbon item configuration.</param>
        protected abstract RibbonItemData CreateElementForStack(TElement elementConfig);

        /// <summary>
        /// Stub for CreateElementForStack, if element can't be stacked.
        /// </summary>
        /// <param name="elementConfig">Ribbon item configuration.</param>
        protected RibbonItemData CannotBeStackedStub(TElement elementConfig)
        {
            throw new System.InvalidOperationException($"Can't be stacked: {elementConfig.GetType().FullName}");
        }

        /// <summary>
        /// Creates and returns a command button.
        /// </summary>
        /// <param name="config">Command button configuration.</param>
        protected PushButtonData CreateCommandButtonData(CommandButton config)
        {
            CheckButtonName(config);
            if (string.IsNullOrWhiteSpace(config.CommandType))
                throw new System.ArgumentException($"Command type not found! Button: {config.Name}");
            var cmdType = _menuData.MenuAssembly.GetTypeFromName(config.CommandType!);
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
                throw new System.ArgumentException("Button name not found!");
        }

        /// <summary>
        /// Sets tooltip.
        /// </summary>
        /// <param name="buttonData">Button data.</param>
        /// <param name="tooltip">Tooltip content.</param>
        protected void SetTooltip(RibbonItemData buttonData, string tooltip)
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
            if (buttonConfig.Text != null)
                buttonData.Text = buttonConfig.Text;
            if (buttonConfig.Description != null)
                buttonData.LongDescription = buttonConfig.Description;
            if (buttonConfig.HelpUrl != null)
                buttonData.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, buttonConfig.HelpUrl));
            buttonData.Image = _menuData.GetIconImage(buttonConfig.SmallImage);
            buttonData.LargeImage = _menuData.GetIconImage(buttonConfig.LargeImage);
        }
    }
}