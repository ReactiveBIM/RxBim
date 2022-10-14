namespace Versions;

public record ItemAttribute(string Name, string Value);

public record ProjectItem(string Name, string Value, bool IsItem, params ItemAttribute[] ItemAttributes);

public record TargetFramework(string Value) : ProjectItem(nameof(TargetFramework), Value, false, null);

public record ApplicationVersion(string Value) : ProjectItem(nameof(ApplicationVersion), Value, false, null);

public record PackageReference(string Name, string Version)
    : ProjectItem(nameof(PackageReference), string.Empty, true, new("Include", Name), new(nameof(Version), Version));

public record RuntimePackageReference(string Name, string Version)
    : ProjectItem(nameof(PackageReference), string.Empty, true, new("Include", Name), new(nameof(Version), Version), new("ExcludeAssets", "runtime"));