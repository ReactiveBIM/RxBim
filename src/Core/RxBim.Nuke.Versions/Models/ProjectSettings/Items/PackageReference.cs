namespace RxBim.Nuke.Versions
{
    /// <summary>
    /// Item for package reference.
    /// </summary>
    /// <param name="Name">Package name.</param>
    /// <param name="Version">Package version.</param>
    public record PackageReference(string Name, string Version)
        : ProjectSetting(nameof(PackageReference), string.Empty, ProjectSettingType.Item, new("Include", Name),
            new ProjectSettingAttribute(nameof(Version), Version));
}