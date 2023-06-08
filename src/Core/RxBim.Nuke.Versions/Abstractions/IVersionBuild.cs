#pragma warning disable CS1591, SA1205, SA1600

namespace RxBim.Nuke.Versions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Bimlab.Nuke;
    using Bimlab.Nuke.Components;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Utilities.Collections;
    using static global::Nuke.Common.IO.FileSystemTasks;
    using static PublishTagService;

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
                var appVersion = AppVersion.GetAll().SingleOrError(x => x.Description == AppVersion.Description,
                    (string)"Selected version not found");
                this.SetBuildVersion(appVersion!);
            });

        Target ResetVersion => _ => _
            .Description("Resets the solution to its default version value.")
            .Executes(() =>
            {
                this.From<IHazSolution>().Solution.Directory.GlobFiles("**/RxBim.Build.Props").ForEach(DeleteFile);
            });

        AppVersion AppVersion { get; set; }

        VersionNumber VersionNumber { get; set; }

        string ProjectNamePrefix => string.Empty;

        IEnumerable<string> IPublish.GetPackageFileNames(IEnumerable<string> tags)
        {
            var appVersion = string.IsNullOrEmpty(VersionNumber)
                ? VersionNumber.GetAll().OrderBy(x => (string)x).First()
                : VersionNumber;

            var allPackagesNames = PackagesDirectory
                .GlobFiles($"{ProjectNamePrefix}*.nupkg")
                .Select(x => x.NameWithoutExtension)
                .ToList();

            var packNamePatterns = tags
                .Where(x => x.StartsWith(ProjectNamePrefix) && IsCompatibleTag(x))
                .Select(x => GetPackageNamePattern(x, appVersion))
                .ToList();

            return allPackagesNames
                .Where(name => packNamePatterns.Any(pattern => Regex.IsMatch(name, pattern)))
                .ToList();
        }

        /// <summary>
        /// Returns a tag for the project.
        /// </summary>
        /// <param name="project">Project</param>
        string IPublish.GetPackageTag(ProjectInfo project)
        {
            return GetTagValue(project);
        }
    }
}