using System;
using Bimlab.Nuke.Nuget;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

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

    AbsolutePath OutputDirectory => RootDirectory / "out";

    string _projectForPublish;

    [Parameter("Project name")]
    public string ProjectForPublish
    {
        get => _projectForPublish ??= PackageInfoProvider.GetSelectedMenuOption();
        set => _projectForPublish = value;
    }

    Target SetProjects => _ => _
        .Executes(() =>
        {
            Project = "all";
        });

    Target Pack => _ => _
        .DependsOn(SetProjects, Compile)
        .Executes(() =>
        {
            PackageInfoProvider.GetSelectedProjects(Project)
                .ForEach(x => PackInternal(Solution, x, OutputDirectory, Configuration));
        });

    /// <summary>
    /// Makes a package
    /// </summary>
    /// <param name="solution">Solution</param>
    /// <param name="project">Project</param>
    /// <param name="outDir">Output directory</param>
    /// <param name="configuration">Build configuration</param>
    static void PackInternal(
        Solution solution,
        ProjectInfo project,
        AbsolutePath outDir,
        string configuration)
    {
        var path = solution.GetProject(project.ProjectName)?.Path;

        if (!string.IsNullOrEmpty(path))
        {
            DotNetTasks.DotNetPack(s => s
                .SetProject(path)
                .SetOutputDirectory(outDir)
                .SetConfiguration(configuration)
                .EnableNoBuild()
                .EnableNoRestore());
        }
    }

    Target CheckUncommitted => _ => _
        .After(Pack)
        .Executes(() =>
        {
            PackageExtensions.CheckUncommittedChanges(Solution);
        });

    Target Push => _ => _
        .Unlisted()
        .DependsOn(Pack, CheckUncommitted)
        .Executes(() =>
        {
            var nugetApiKey = Environment.GetEnvironmentVariable("NUGET_API_KEY");
            if (string.IsNullOrWhiteSpace(nugetApiKey))
            {
                throw new ArgumentException("NUGET_API_KEY variable is not setted");
            }

            PackageInfoProvider.GetSelectedProjects(Project)
                .ForEach(x => PackageExtensions.PushPackage(Solution, x, OutputDirectory, nugetApiKey, NugetSource));
        });

    Target Tag => _ => _
        .DependsOn(Push)
        .Executes(() =>
        {
            PackageInfoProvider.GetSelectedProjects(Project)
                .ForEach(x => PackageExtensions.TagPackage(Solution, x));
        });

    Target Publish => _ => _
        .Description("Публикует Nuget-пакеты")
        .DependsOn(Tag);
}