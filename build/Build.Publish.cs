using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bimlab.Nuke.Components;
using Bimlab.Nuke.Nuget;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.Git;
using RxBim.Nuke.Versions;
using Serilog;

partial class Build
{
    const string RxBimProjectNamePrefix = "RxBim.";

    public Target Publish => _ => _
        .Description("Publishes packages based on current commit tags.")
        .Requires(() => From<IPublish>().NugetApiKey, () => From<IPublish>().NugetSource)
        .DependsOn<IPack>(x => x.Pack)
        .After(From<IPublish>().Prerelease)
        .Executes(() =>
        {
            var tags = GetCurrentTags();
            if (!tags.Any())
            {
                Log.Information("No tags, exit.");
                return;
            }

            var publishNames = GetPackageFileNames(tags);

            publishNames
                .ForEach(x => PackageExtensions.PushPackage(From<IPack>().PackagesDirectory, x,
                    From<IPublish>().NugetApiKey, From<IPublish>().NugetSource));
        });

    /// <summary>
    /// Sets git tags for given packages.
    /// </summary>
    /// <param name="projectNames">Project names collection.</param>
    /// <param name="release">If true, tags are used for "release" version of packages.</param>
    public void TagPackages(string[] projectNames, bool release = false)
    {
        List<ProjectInfo>? p;

        if (From<IHazGitRepository>().GitRepository.IsOnReleaseBranch() ||
            From<IHazGitRepository>().GitRepository.IsOnHotfixBranch())
        {
            if (release) //// RELEASE
            {
                var pattern = new Regex(@"\d+(\.\d+)*");

                p = From<IPublish>().PackageInfoProvider.GetSelectedProjects(projectNames)
                    .Where(x => pattern.IsMatch(x.Version!)).ToList();

                if (!p.Any())
                    throw new InvalidOperationException("No release (-rc) packages to publish found");
            }
            else //// RELEASE CANDIDATE
            {
                var pattern = new Regex(@"\d+(\.\d+)*-rc\d*");

                p = From<IPublish>().PackageInfoProvider.GetSelectedProjects(projectNames)
                    .Where(x => pattern.IsMatch(x.Version!)).ToList();

                if (!p.Any())
                    throw new InvalidOperationException("No release candidate (-rc) packages to publish found");
            }
        }
        else //// DEVELOP
        {
            var pattern = new Regex(@"\d+(\.\d+)*-dev\d*");

            p = From<IPublish>().PackageInfoProvider.GetSelectedProjects(projectNames)
                .Where(x => pattern.IsMatch(x.Version!)).ToList();

            if (!p.Any())
                throw new InvalidOperationException("No develop (-dev) packages to publish found");
        }

        p.ForEach(project => TagPackage(From<IHazSolution>().Solution, GetPackageTag(project)));
    }

    List<string> GetPackageFileNames(List<string> tags)
    {
        var appVersion = string.IsNullOrEmpty(CurrentAppVersionNumber)
            ? VersionNumber.GetAll().OrderBy(x => (string)x).First()
            : CurrentAppVersionNumber;

        var allPackagesNames = From<IPack>().PackagesDirectory
            .GlobFiles($"{RxBimProjectNamePrefix}*.nupkg")
            .Select(x => x.NameWithoutExtension)
            .ToList();

        var packNamePatterns = tags
            .Where(x => x.StartsWith(RxBimProjectNamePrefix) && ProjectTagInfo.IsCompatibleTag(x))
            .Select(x => GetPackageNamePattern(new ProjectTagInfo(x), appVersion))
            .ToList();

        return allPackagesNames
            .Where(name => packNamePatterns.Any(pattern => Regex.IsMatch(name, pattern)))
            .ToList();
    }

    static string GetPackageNamePattern(ProjectTagInfo tagInfo, VersionNumber appVersion)
    {
        var patternBuilder = new StringBuilder()
            .Append('^')
            .Append(Regex.Escape(tagInfo.Name))
            .Append(@$"(\.{appVersion})?")
            .Append(@$"\.{Regex.Escape(tagInfo.Version)}(\.0){{0,3}}");

        if (!string.IsNullOrEmpty(tagInfo.VersionSuffix))
            patternBuilder.Append(@$"-{Regex.Escape(tagInfo.VersionSuffix)}");

        patternBuilder.Append('$');

        return patternBuilder.ToString();
    }

    /// <summary>
    /// Returns a tag for the project.
    /// </summary>
    /// <param name="project">Project</param>
    static string GetPackageTag(ProjectInfo project)
    {
        return new ProjectTagInfo(project).ToString();
    }

    static void TagPackage(Solution solution, string tagValue)
    {
        GitTasks.Git($"tag {tagValue}", solution.Directory);
    }

    List<string> GetCurrentTags()
    {
        return GitTasks
            .Git($"tag --points-at {From<IHazGitRepository>().GitRepository.Commit}")
            .Select(x => x.Text)
            .ToList();
    }
}