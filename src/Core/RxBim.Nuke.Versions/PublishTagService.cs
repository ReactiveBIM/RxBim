namespace RxBim.Nuke.Versions;

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bimlab.Nuke;

/// <summary>
/// Project package publish tag service.
/// </summary>
public static class PublishTagService
{
    private const char TagPartsSeparator = ';';

    /// <summary>
    /// The tag is compatible with the project package tag.
    /// </summary>
    /// <param name="tag">Tag value.</param>
    /// <returns></returns>
    public static bool IsCompatibleTag(string tag)
    {
        return tag.Contains(TagPartsSeparator);
    }

    /// <summary>
    /// Returns tag value from project name and version.
    /// </summary>
    /// <param name="projectInfo">Project info.</param>
    public static string GetTagValue(ProjectInfo projectInfo)
    {
        var name = projectInfo.ProjectName ?? throw new InvalidOperationException("Project name can't be null!");
        var version = projectInfo.Package.Version ?? throw new InvalidOperationException("Project version can't be null!");
        return $"{name}{TagPartsSeparator}{version}";
    }

    /// <summary>
    /// Returns a regular expression pattern for the package filename.
    /// </summary>
    /// <param name="tag">Tag value.</param>
    /// <param name="appVersion">Selected version number.</param>
    public static string GetPackageNamePattern(string tag, VersionNumber appVersion)
    {
        var parts = tag.Split(TagPartsSeparator);
        var projectName = parts.First();
        var projectVersion = parts.ElementAt(1);

        var patternBuilder = new StringBuilder()
            .Append('^')
            .Append(Regex.Escape(projectName))
            .Append(@$"(\.{appVersion})?\.{Regex.Escape(projectVersion)}")
            .Append('$');

        return patternBuilder.ToString();
    }
}