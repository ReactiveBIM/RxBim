namespace RxBim.Nuke.Versions;

using System.Linq;
using Bimlab.Nuke.Nuget;

/// <summary>
/// Project package tag information.
/// </summary>
public readonly struct ProjectTagInfo
{
    private const char TagPartsSeparator = ';';

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectTagInfo"/> struct.
    /// </summary>
    /// <param name="tag">Tag value.</param>
    public ProjectTagInfo(string tag)
    {
        var parts = tag.Split(TagPartsSeparator);
        Name = parts.First();
        Version = parts.ElementAt(1);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectTagInfo"/> struct.
    /// </summary>
    /// <param name="projectInfo">Project info.</param>
    public ProjectTagInfo(ProjectInfo projectInfo)
    {
        Name = projectInfo.ProjectName;
        Version = projectInfo.Version;
    }

    /// <summary>
    /// Project name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Project version.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// The tag is compatible with the project package tag.
    /// </summary>
    /// <param name="tag">Tag value.</param>
    /// <returns></returns>
    public static bool IsCompatibleTag(string tag)
    {
        return tag.Contains(TagPartsSeparator);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Name}{TagPartsSeparator}{Version}";
    }
}