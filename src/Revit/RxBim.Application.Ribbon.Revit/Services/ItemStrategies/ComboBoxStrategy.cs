namespace RxBim.Application.Ribbon.Services.ItemStrategies;

using System;
using System.Linq;
using Abstractions;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using JetBrains.Annotations;
using ComboBox = RxBim.Application.Ribbon.ComboBox;
using RibbonItem = Autodesk.Windows.RibbonItem;
using RibbonPanel = Autodesk.Windows.RibbonPanel;

/// <inheritdoc />
[UsedImplicitly]
public class ComboBoxStrategy(IRibbonPanelItemService ribbonPanelItemService) : ItemStrategyBase<ComboBox>
{
    private string _tabName = string.Empty;

    /// <inheritdoc />
    protected override void AddItem(RibbonTab tab, Autodesk.Revit.UI.RibbonPanel ribbonPanel, ComboBox itemConfig)
    {
        CreateComboBox(tab, ribbonPanel, itemConfig);
    }

    /// <inheritdoc />
    protected override RibbonItemData GetItemForStack(ComboBox itemConfig)
    {
        return CantBeStackedStub(itemConfig);
    }

    private void CreateComboBox(RibbonTab tab, Autodesk.Revit.UI.RibbonPanel ribbonPanel, ComboBox itemConfig)
    {
        _tabName = tab.Title;
        var exist = tab.Panels.SelectMany(p => p.Source.Items)
            .FirstOrDefault(i => i.Name?.Equals(itemConfig.Name) ?? false);
        if (exist is RibbonCombo ribbonCombo)
        {
            foreach (var comboBoxMember in itemConfig.ComboBoxMembers)
            {
                ribbonCombo.Items.Add(new RibbonItem { Name = comboBoxMember.Name, Text = comboBoxMember.Text });
            }

            return;
        }

        var comboBox = ribbonPanelItemService.CreateComboBox(tab.Title, itemConfig);
        ComponentManager.Ribbon?.Tabs
            .FirstOrDefault(x => x.Title.Equals(_tabName, StringComparison.OrdinalIgnoreCase))
            ?.Panels.FirstOrDefault(x =>
                x.Source.AutomationName.Equals(ribbonPanel.Title, StringComparison.OrdinalIgnoreCase))
            ?.Source.Items.Add(comboBox);
    }
}