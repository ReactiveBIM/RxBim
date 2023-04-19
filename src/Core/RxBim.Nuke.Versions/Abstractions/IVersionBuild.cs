#pragma warning disable CS1591, SA1205, SA1600

namespace RxBim.Nuke.Versions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Bimlab.Nuke.Components;
    using Bimlab.Nuke.Nuget;
    using global::Nuke.Common;
    using global::Nuke.Common.Git;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Tools.Git;
    using global::Nuke.Common.Utilities.Collections;
    using Serilog;
    using static global::Nuke.Common.IO.FileSystemTasks;

    public interface IVersionBuild : IPublish
    {
        Target SetVersion => _ => _
            .Description("Configures the solution to work with a specific version of all CAD/BIM applications.")
            .Requires(() => VersionNumber)
            .Executes(this.SetBuildVersion);

        Target SetVersionForApp => _ => _
            .Description("Configures the solution to work with a specific version of a specific CAD/BIM application.")
            .Requires(() => AppVersion)
            .Executes(() =>
            {
                var appVersion = AppVersion.GetAll()
                    .SingleOrError(x => x.Description == AppVersion.Description, "Selected version not found");
                this.SetBuildVersion(appVersion!);
            });

        Target ResetVersion => _ => _
            .Description("Resets the solution to its default version value.")
            .Executes(() =>
            {
                this.From<IHazSolution>()
                    .Solution.Directory.GlobFiles("**/RxBim.Build.Props")
                    .ForEach(DeleteFile);
            });

        AppVersion AppVersion { get; set; }

        VersionNumber VersionNumber { get; set; }

        Target IPublish.Publish => _ => _
            .Description("Publishes packages based on current commit tags.")
            .Requires(() => NugetApiKey, () => NugetSource)
            .DependsOn<IPack>(x => x.Pack)
            .After(Prerelease)
            .Executes(() =>
            {
                var tags = GetCurrentTags();
                if (!tags.Any())
                {
                    Log.Information("No tags, exit.");
                    return;
                }

                var publishNames = GetPackageFileNames(tags);

                publishNames.ForEach(x =>
                    PackageExtensions.PushPackage(PackagesDirectory, x, NugetApiKey, NugetSource));
            });

        /// <summary>
        /// Sets git tags for given packages.
        /// </summary>
        /// <param name="projectNames">Project names collection.</param>
        /// <param name="release">If true, tags are used for "release" version of packages.</param>
        void IPublish.TagPackages(string[] projectNames, bool release)
        {
            List<ProjectInfo>? p;

            if (GitRepository.IsOnReleaseBranch() || GitRepository.IsOnHotfixBranch())
            {
                if (release) //// RELEASE
                {
                    var pattern = new Regex(@"\d+(\.\d+)*");

                    p = PackageInfoProvider.GetSelectedProjects(projectNames)
                        .Where(x => pattern.IsMatch(x.Version!)).ToList();

                    if (!p.Any())
                        throw new InvalidOperationException("No release (-rc) packages to publish found");
                }
                else //// RELEASE CANDIDATE
                {
                    var pattern = new Regex(@"\d+(\.\d+)*-rc\d*");

                    p = PackageInfoProvider.GetSelectedProjects(projectNames)
                        .Where(x => pattern.IsMatch(x.Version!)).ToList();

                    if (!p.Any())
                        throw new InvalidOperationException("No release candidate (-rc) packages to publish found");
                }
            }
            else //// DEVELOP
            {
                var pattern = new Regex(@"\d+(\.\d+)*-dev\d*");

                p = PackageInfoProvider.GetSelectedProjects(projectNames)
                    .Where(x => pattern.IsMatch(x.Version!)).ToList();

                if (!p.Any())
                    throw new InvalidOperationException("No develop (-dev) packages to publish found");
            }

            p.ForEach(project => TagPackage(Solution, GetPackageTag(project)));
        }

        List<string> GetPackageFileNames(List<string> tags)
        {
            var appVersion = string.IsNullOrEmpty(VersionNumber)
                ? VersionNumber.GetAll().OrderBy(x => (string)x).First()
                : VersionNumber;

            const string projectNamePrefix = "RxBim.";

            var allPackagesNames = PackagesDirectory
                .GlobFiles($"{projectNamePrefix}*.nupkg")
                .Select(x => x.NameWithoutExtension)
                .ToList();

            var packNamePatterns = tags
                .Where(x => x.StartsWith(projectNamePrefix) && ProjectTagInfo.IsCompatibleTag(x))
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
                .Append(@$"(\.{appVersion})?\.{Regex.Escape(tagInfo.Version)}")
                .Append('$');

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
                .Git($"tag --points-at {GitRepository.Commit}")
                .Select(x => x.Text)
                .ToList();
        }
    }
}