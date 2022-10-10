namespace RxBim.Nuke.Versions
{
    /// <summary>
    /// Target framework property.
    /// </summary>
    /// <param name="Value">Property value.</param>
    public record TargetFramework(string Value) : ProjectSetting(nameof(TargetFramework),
        Value,
        SettingType.Property);
}