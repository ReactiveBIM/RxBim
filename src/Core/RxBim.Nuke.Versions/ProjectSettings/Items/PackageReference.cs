namespace RxBim.Nuke.Versions
{
    /// <summary>
    /// Item for package reference.
    /// </summary>
    /// <param name="Name">Package name.</param>
    /// <param name="Version">Package version.</param>
    public record PackageReference(string Name, string Version)
        : ProjectSettingBase(nameof(PackageReference), string.Empty, SettingType.Item, new("Include", Name),
            new SettingAttribute(nameof(Version), Version));
}