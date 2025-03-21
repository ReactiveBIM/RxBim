namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Linq;
    using Abstractions;
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using ComboBox = ComboBox;
    using RibbonItem = Autodesk.Windows.RibbonItem;

    /// <inheritdoc />
    internal class RibbonPanelItemService(MenuData menuData, IComboBoxEventsHandler comboBoxEventsHandler) : IRibbonPanelItemService
    {
        private string _tabName = string.Empty;

        /// <inheritdoc />
        public PushButtonData CreateCommandButtonData(CommandButton button)
        {
            button.LoadFromAttribute(menuData.MenuAssembly);
            CheckButtonName(button);
            if (string.IsNullOrWhiteSpace(button.CommandType))
                throw new ArgumentException($"Command type not found! Button: {button.Name}");
            var cmdType = menuData.MenuAssembly.GetTypeByName(button.CommandType!);
            var className = cmdType.FullName;
            var assemblyLocation = cmdType.Assembly.Location;
            var pushButtonData =
                new PushButtonData(button.Name, button.Text ?? button.Name, assemblyLocation, className)
                {
                    AvailabilityClassName = className
                };
            SetButtonProperties(pushButtonData, button);
            SetTooltip(pushButtonData, menuData.GetTooltipContent(button, cmdType));
            return pushButtonData;
        }

        /// <inheritdoc />
        public RibbonCombo CreateComboBox(string tabName, ComboBox itemConfig)
        {
            _tabName = tabName;
            var comboBox = new RibbonCombo
            {
                Name = itemConfig.Name,
                Text = itemConfig.Text,
                Description = itemConfig.Description,
                Image = menuData.GetIconImage(itemConfig.Image),
                ToolTip = itemConfig.ToolTip,
                Width = itemConfig.Width
            };

            comboBox.CurrentChanged += ComboBoxOnCurrentChanged;

            foreach (var comboBoxMember in itemConfig.ComboBoxMembers)
            {
                comboBox.Items.Add(new RibbonItem { Name = comboBoxMember.Name, Text = comboBoxMember.Text });
            }

            return comboBox;
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
                ? menuData.MenuAssembly.GetTypeByName(commandButton.CommandType!).Assembly
                : null;

            if (buttonConfig.Text != null)
                buttonData.Text = buttonConfig.Text;
            if (buttonConfig.Description != null)
                buttonData.LongDescription = buttonConfig.Description;
            if (buttonConfig.HelpUrl != null)
                buttonData.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, buttonConfig.HelpUrl));
            buttonData.Image = menuData.GetIconImage(buttonConfig.Image, assembly);
            buttonData.LargeImage = menuData.GetIconImage(buttonConfig.LargeImage, assembly);
        }

        /// <inheritdoc />
        public void CreateButtonsForPullDown(PullDownButton config, PulldownButton button)
        {
            foreach (var pushButtonData in config.CommandButtonsList.Select(CreateCommandButtonData))
            {
                button.AddPushButton(pushButtonData);
            }
        }

        private void ComboBoxOnCurrentChanged(object sender, RibbonPropertyChangedEventArgs e)
        {
            if (e.OldValue is not RibbonItem oldItem || e.NewValue is not RibbonItem newItem)
                return;

            comboBoxEventsHandler.HandleCurrentChanged(_tabName, oldItem.Text, newItem.Text);
        }
    }
}