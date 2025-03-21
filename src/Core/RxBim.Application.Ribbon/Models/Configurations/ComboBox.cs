namespace RxBim.Application.Ribbon;

using System.Collections.Generic;

/// <summary>
/// Represents a combobox configuration.
/// </summary>
public class ComboBox : RibbonPanelItemBase
{
    /// <summary>
    /// Combobox width.
    /// </summary>
    public double Width { get; set; } = 200;

    /// <summary>
    /// Combobox members list.
    /// </summary>
    public List<ComboBoxMember> ComboBoxMembers { get; set; } = new();
}