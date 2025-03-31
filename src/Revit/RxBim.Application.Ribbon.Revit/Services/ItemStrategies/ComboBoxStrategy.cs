namespace RxBim.Application.Ribbon.Services.ItemStrategies;

using System;
using System.Collections.Generic;
using System.Linq;
using Abstractions;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using JetBrains.Annotations;
using ComboBox = RxBim.Application.Ribbon.ComboBox;
using RibbonItem = Autodesk.Windows.RibbonItem;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

/// <inheritdoc />
[UsedImplicitly]
public class ComboBoxStrategy(IRibbonPanelItemService ribbonPanelItemService, MenuData menuData) : ItemStrategyBase<ComboBox>
{
    /// <inheritdoc />
    protected override void AddItem(RibbonTab tab, RibbonPanel ribbonPanel, ComboBox itemConfig)
    {
        CreateComboBox(tab, ribbonPanel, itemConfig);
    }

    /// <inheritdoc />
    protected override RibbonItemData GetItemForStack(ComboBox itemConfig)
    {
        return new ComboBoxData(itemConfig.Name);
    }

    private void CreateComboBox(RibbonTab tab, RibbonPanel ribbonPanel, ComboBox itemConfig)
    {
        var exist = tab.Panels.SelectMany(p => p.Source.Items)
            .SelectMany(i =>
            {
                return i switch
                {
                    RibbonRowPanel ribbonRowPanel => ribbonRowPanel.Items.ToList(),
                    RibbonSplitButton ribbonSplitButton => ribbonSplitButton.Items.ToList(),
                    _ => new List<RibbonItem>([i])
                };
            })
            .FirstOrDefault(i => i.Name?.Equals(itemConfig.Name) ?? false);
        if (exist is RibbonCombo ribbonCombo)
        {
            foreach (var comboBoxMember in itemConfig.ComboBoxMembers)
            {
                ribbonCombo.Items.Add(new RibbonItem
                {
                    Name = comboBoxMember.Name,
                    Text = comboBoxMember.Text,
                    GroupName = comboBoxMember.GroupName,
                    Image = menuData.GetIconImage(comboBoxMember.Image),
                    Description = comboBoxMember.Description,
                    ToolTip = comboBoxMember.ToolTip
                });
            }

            return;
        }

        var comboBox = ribbonPanelItemService.CreateComboBox(itemConfig);
        tab.Panels.FirstOrDefault(x =>
                x.Source.AutomationName.Equals(ribbonPanel.Title, StringComparison.OrdinalIgnoreCase))
            ?.Source.Items.Add(comboBox);
    }
}