namespace RxBim.Nuke.Versions
{
    /// <summary>
    /// Property for constants definition.
    /// </summary>
    /// <param name="Value">Property value.</param>
    public record DefineConstants(string Value) : ProjectSettingBase(nameof(DefineConstants),
        Value,
        SettingType.Property);
}