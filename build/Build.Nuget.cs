using System.Linq;
using Bimlab.Nuke.Nuget;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.NuGet;

partial class Build
{
    /// <summary>
    /// NuGet source URL
    /// </summary>
    [Parameter("NuGet source URL")]
    public readonly string NugetSource = "https://api.nuget.org/v3/index.json";

    /// <summary>
    /// NuGet symbol source URL
    /// </summary>
    [Parameter("NuGet symbol source URL")]
    public readonly string NugetSymbolSource = "https://nuget.smbsrc.net/";

    // Nuget
    const string NugetApiKey = "oy2iwdrmpblvtmbp5twam4vhwa3sfqaezclap3dk2fuwc4";

    AbsolutePath OutputDirectory => RootDirectory / "out";

    string _projectForPublish;

    [Parameter("Project name")]
    public string ProjectForPublish
    {
        get => _projectForPublish ??= _packageInfoProvider.GetSelectedMenuOption();
        set => _projectForPublish = value;
    }

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var path = Solution.Directory / "out";
            var sourceProjects = Solution.AllProjects.Where(x => x.Path.ToString().Contains("\\src\\"));
            foreach (var project in sourceProjects)
            {
                DotNetTasks.DotNetPack(settings => settings
                    .SetConfiguration(Configuration)
                    .SetNoBuild(true)
                    .SetNoRestore(true)
                    .SetProject(project)
                    .SetOutputDirectory(path));
            }
        });

    Target CheckUncommitted => _ => _
        .DependsOn(Pack)
        .Executes(() => PackageExtensions.CheckUncommittedChanges(Solution));

    Target Push => _ => _
        .Unlisted()
        .Requires(() => ProjectForPublish)
        .DependsOn(CheckUncommitted)
        .Executes(() =>
        {
            _packageInfoProvider.GetSelectedProjects(ProjectForPublish)
                .ForEach(x =>
                {
                    var project = Solution.AllProjects.FirstOrDefault(p => p.Path == x.ProjectName);
                    if (project != null)
                    {
                        var package = Solution.Directory / "out" /
                                      $"{project.GetProperty("PackageId")}.{project.GetProperty("Version")}.nupkg";
                        NuGetTasks.NuGetPush(settings => settings
                            .SetWorkingDirectory(project.Directory)
                            .SetApiKey(NugetApiKey)
                            .SetSource(NugetSource)
                            .SetTargetPath(package));
                    }
                });
        });

    Target Tag => _ => _
        .Requires(() => ProjectForPublish)
        .DependsOn(Push)
        .Executes(() =>
        {
            _packageInfoProvider.GetSelectedProjects(ProjectForPublish)
                .ForEach(x => PackageExtensions.TagPackage(Solution, x));
        });

    Target Publish => _ => _
        .Description("Публикует Nuget-пакеты")
        .DependsOn(Tag);
}