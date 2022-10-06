namespace Versions;

public record ItemAttribute(string Name, string Value);

public record ProjectItem(string Name, string Value, ItemType Type, params ItemAttribute[] Attributes);

public record TargetFramework(string Value) : ProjectItem(nameof(TargetFramework), Value, ItemType.Property);

public record ApplicationVersion(string Value) : ProjectItem(nameof(ApplicationVersion), Value, ItemType.Property);

public record PackageReference(string Name, string Version)
    : ProjectItem(nameof(PackageReference), string.Empty, ItemType.Item, new("Include", Name),
        new(nameof(Version), Version));

public record RuntimePackageReference(string Name, string Version)
    : ProjectItem(nameof(PackageReference), string.Empty, ItemType.Item, new("Include", Name),
        new(nameof(Version), Version),
        new("ExcludeAssets", "runtime"));