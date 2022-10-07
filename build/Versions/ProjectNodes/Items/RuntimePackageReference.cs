public record RuntimePackageReference(string Name, string Version)
    : ProjectNode(nameof(PackageReference), string.Empty, NodeType.Item, new("Include", Name),
        new(nameof(Version), Version),
        new("ExcludeAssets", "runtime"));