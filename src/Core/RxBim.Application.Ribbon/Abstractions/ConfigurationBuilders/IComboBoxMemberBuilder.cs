namespace RxBim.Application.Ribbon;

/// <summary>
/// Defines a ribbon combo box member builder.
/// </summary>
public interface IComboBoxMemberBuilder : IRibbonPanelItemBuilder<IComboBoxMemberBuilder>
{
    /// <summary>
    /// Sets group name.
    /// </summary>
    /// <param name="groupName">Group name.</param>
    IComboBoxMemberBuilder GroupName(string groupName);
}