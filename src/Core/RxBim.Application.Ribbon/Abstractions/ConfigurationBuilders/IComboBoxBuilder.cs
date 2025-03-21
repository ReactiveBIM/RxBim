namespace RxBim.Application.Ribbon;

using System;

/// <summary>
/// Defines a ribbon combo box member builder.
/// </summary>
public interface IComboBoxBuilder : IRibbonPanelItemBuilder<IComboBoxBuilder>
{
    /// <summary>
    /// Sets combobox width.
    /// </summary>
    /// <param name="width">Width.</param>
    IComboBoxBuilder SetWidth(double width);

    /// <summary>
    /// Adds a new <see cref="ComboBoxMember"/> to <see cref="ComboBox"/>.
    /// </summary>
    /// <param name="name">The combo box member internal name.</param>
    /// <param name="builder">The combo box member builder.</param>
    IComboBoxBuilder AddComboBoxMember(string name, Action<IComboBoxMemberBuilder>? builder = null);
}