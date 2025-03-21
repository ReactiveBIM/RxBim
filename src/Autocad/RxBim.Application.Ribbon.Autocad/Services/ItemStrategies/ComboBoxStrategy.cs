namespace RxBim.Application.Ribbon.Services.ItemStrategies
{
    using System;
    using System.Linq;
    using System.Windows.Controls;
    using Autodesk.Windows;
    using JetBrains.Annotations;
    using RxBim.Application.Ribbon;
    using ComboBox = Application.Ribbon.ComboBox;

    /// <inheritdoc />
    [UsedImplicitly]
    public class ComboBoxStrategy(IPanelService panelService, IComboBoxEventsHandler comboBoxEventsHandler, MenuData menuData) : ItemStrategyBase<ComboBox>
    {
        /// <inheritdoc />
        protected override void AddItem(RibbonTab ribbonTab, RibbonPanel ribbonPanel, ComboBox comboBoxConfig)
        {
            var exist = ribbonTab.Panels.SelectMany(p => p.Source.Items)
                .OfType<RibbonRowPanel>()
                .SelectMany(r => r.Items)
                .FirstOrDefault(i => i.Name?.Equals(comboBoxConfig.Name) ?? false);
            if (exist is RibbonCombo ribbonCombo)
            {
                foreach (var comboBoxMember in comboBoxConfig.ComboBoxMembers)
                {
                    ribbonCombo.Items.Add(new RibbonItem { Name = comboBoxMember.Name, Text = comboBoxMember.Text });
                }

                return;
            }

            panelService.AddItem(ribbonPanel, CreateComboBox(comboBoxConfig));
        }

        /// <inheritdoc />
        protected override RibbonItem GetItemForStack(ComboBox comboBoxConfig, RibbonItemSize size)
        {
            return CreateComboBox(comboBoxConfig);
        }

        private RibbonCombo CreateComboBox(ComboBox comboBoxConfig)
        {
            var comboBox = new RibbonCombo
            {
                Name = comboBoxConfig.Name,
                Text = comboBoxConfig.Text,
                Description = comboBoxConfig.Description,
                Image = menuData.GetIconImage(comboBoxConfig.Image),
                ToolTip = comboBoxConfig.ToolTip,
                Width = comboBoxConfig.Width
            };

            comboBox.CurrentChanged += ComboBoxOnCurrentChanged;

            foreach (var comboBoxMember in comboBoxConfig.ComboBoxMembers)
            {
                comboBox.Items.Add(new RibbonItem { Name = comboBoxMember.Name, Text = comboBoxMember.Text });
            }

            return comboBox;
        }

        private void ComboBoxOnCurrentChanged(object? sender, RibbonPropertyChangedEventArgs e)
        {
            if (sender is not RibbonCombo ribbonCombo)
                return;
            if (e.OldValue is not RibbonItem oldItem || e.NewValue is not RibbonItem newItem)
                return;

            comboBoxEventsHandler.HandleCurrentChanged(ribbonCombo.Id, oldItem.Text, newItem.Text);
        }
    }
}