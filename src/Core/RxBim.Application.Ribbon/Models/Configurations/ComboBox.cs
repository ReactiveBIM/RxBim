namespace RxBim.Application.Ribbon;

using System.Collections.Generic;

/// <summary>
/// Represents a combobox configuration.
/// </summary>
public class ComboBox : RibbonPanelItemBase
{
    /// <summary>
    /// Combobox members list.
    /// </summary>
    public List<ComboBoxMember> ComboBoxMembers { get; set; } = new();
}