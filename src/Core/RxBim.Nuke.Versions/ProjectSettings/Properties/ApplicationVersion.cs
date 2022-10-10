namespace RxBim.Nuke.Versions
{
    /// <summary>
    /// Application version property.
    /// </summary>
    /// <param name="Value">Property value.</param>
    public record ApplicationVersion(string Value) : ProjectSettingBase(nameof(ApplicationVersion),
        Value,
        SettingType.Property);
}