using System.Linq;
using Bimlab.Nuke.Nuget;

public readonly struct ProjectTagInfo
{
    const char TagPartsSeparator = ';';

    public ProjectTagInfo(string tag)
    {
        var parts = tag.Split(TagPartsSeparator);
        Name = parts.First();
        Version = parts.ElementAt(1);
        VersionSuffix = parts.Length > 2 ? parts.ElementAt(2) : string.Empty;
    }

    public ProjectTagInfo(ProjectInfo project)
    {
        Name = project.ProjectName;
        var versionParts = project.Version.Split('-');
        Version = versionParts.First();
        VersionSuffix = versionParts.Length > 1 ? versionParts.ElementAt(1) : string.Empty;
    }

    public string Name { get; }

    public string Version { get; }

    public string VersionSuffix { get; }

    public static bool IsCompatibleTag(string tag)
    {
        return tag.Contains(TagPartsSeparator);
    }

    public override string ToString()
    {
        return string.IsNullOrEmpty(VersionSuffix)
            ? $"{Name}{TagPartsSeparator}{Version}"
            : $"{Name}{TagPartsSeparator}{Version}{TagPartsSeparator}{VersionSuffix}";
    }
}