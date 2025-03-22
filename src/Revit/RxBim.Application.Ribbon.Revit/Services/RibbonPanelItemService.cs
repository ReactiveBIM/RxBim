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
        public RibbonCombo CreateComboBox(ComboBox itemConfig)
        {
            var comboBox = CreateComboBoxInternal(itemConfig);

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

        /// <inheritdoc />
        public void SetComboBoxProperties(ComboBox config, Autodesk.Revit.UI.ComboBox comboBox, string tabName, string panelName)
        {
            var existComboBox = ComponentManager.Ribbon?.Tabs
                .FirstOrDefault(t => t.Title.Equals(tabName))
                ?.Panels.FirstOrDefault(p => p.Source.AutomationName.Equals(panelName))
                ?.Source.Items.OfType<RibbonRowPanel>().SelectMany(row => row.Items)
                .OfType<RibbonCombo>()
                .FirstOrDefault(i => i.Id.EndsWith(comboBox.Name));
            if (existComboBox == null)
                return;

            existComboBox.Width = config.Width;
            foreach (var member in config.ComboBoxMembers)
            {
                comboBox.AddItem(new ComboBoxMemberData(member.Name, member.Text));
            }

            existComboBox.CurrentChanged += ComboBoxOnCurrentChanged;
        }

        private void ComboBoxOnCurrentChanged(object? sender, RibbonPropertyChangedEventArgs e)
        {
            if (sender is not RibbonCombo ribbonCombo)
                return;
            if (e.OldValue is not RibbonItem oldItem || e.NewValue is not RibbonItem newItem)
                return;

            comboBoxEventsHandler.HandleCurrentChanged(ribbonCombo.Id, oldItem.Text, newItem.Text);
        }

        private RibbonCombo CreateComboBoxInternal(ComboBox itemConfig)
        {
            return new RibbonCombo
            {
                Name = itemConfig.Name,
                Text = itemConfig.Text,
                Description = itemConfig.Description,
                Image = menuData.GetIconImage(itemConfig.Image),
                ToolTip = itemConfig.ToolTip,
                Width = itemConfig.Width
            };
        }
    }
}