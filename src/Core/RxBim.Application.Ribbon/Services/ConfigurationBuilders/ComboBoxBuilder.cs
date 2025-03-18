namespace RxBim.Application.Ribbon.ConfigurationBuilders;

using System;

/// <inheritdoc cref="RxBim.Application.Ribbon.IComboBoxBuilder" />.
public class ComboBoxBuilder : RibbonPanelItemBuilderBase<ComboBox, IComboBoxBuilder>, IComboBoxBuilder
{
    /// <inheritdoc />
    public ComboBoxBuilder(string name)
        : base(name)
    {
    }

    /// <inheritdoc />
    public IComboBoxBuilder AddComboBoxMember(string name, Action<IComboBoxMemberBuilder>? builder = null)
    {
        var comboBoxMemberBuilder = new ComboBoxMemberBuilder(name);
        builder?.Invoke(comboBoxMemberBuilder);
        Item.ComboBoxMembers.Add(comboBoxMemberBuilder.Build());
        return this;
    }
}