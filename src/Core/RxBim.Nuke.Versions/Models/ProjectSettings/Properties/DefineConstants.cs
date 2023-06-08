namespace RxBim.Nuke.Versions
{
    /// <summary>
    /// Property for constants definition.
    /// </summary>
    /// <param name="Value">Property value.</param>
    public record DefineConstants(string Value) : ProjectSetting(nameof(DefineConstants),
        Value,
        ProjectSettingType.Property);
}