namespace RxBim.Nuke.Versions
{
    /// <summary>
    /// Runtime version property.
    /// </summary>
    /// <param name="Value">Property value.</param>
    public record RuntimeVersion(string Value) : ProjectSetting(nameof(RuntimeVersion),
        Value,
        ProjectSettingType.Property);
}