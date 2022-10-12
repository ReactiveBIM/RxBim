namespace RxBim.Nuke.Versions
{
    /// <summary>
    /// Item for runtime package reference.
    /// </summary>
    /// <param name="Name">Package name.</param>
    /// <param name="Version">Package version.</param>
    public record RuntimePackageReference(string Name, string Version)
        : ProjectSetting(nameof(PackageReference), string.Empty, SettingType.Item, new("Include", Name),
            new(nameof(Version), Version),
            new("ExcludeAssets", "build; runtime"));
}