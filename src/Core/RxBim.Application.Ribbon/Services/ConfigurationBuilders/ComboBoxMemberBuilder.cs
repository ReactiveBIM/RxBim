namespace RxBim.Application.Ribbon.ConfigurationBuilders;

/// <inheritdoc cref="RxBim.Application.Ribbon.IComboBoxMemberBuilder" />.
public class ComboBoxMemberBuilder : RibbonPanelItemBuilderBase<ComboBoxMember, IComboBoxMemberBuilder>, IComboBoxMemberBuilder
{
    /// <inheritdoc />
    public ComboBoxMemberBuilder(string name)
        : base(name)
    {
    }

    /// <inheritdoc />
    public IComboBoxMemberBuilder GroupName(string groupName)
    {
        Item.GroupName = groupName;
        return this;
    }
}