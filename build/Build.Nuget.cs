using System;
using System.Linq;
using Bimlab.Nuke.Nuget;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
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
        .Requires(() => Project)
        .Executes(() =>
        {
            _packageInfoProvider.GetSelectedProjects(Project)
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
        var path = solution.GetProject(project.ProjectName).Path;
        DotNetTasks.DotNetPack(s => s
            .SetProject(path)
            .SetOutputDirectory(outDir)
            .SetConfiguration(configuration)
            .EnableNoBuild());
    }

    Target CheckUncommitted => _ => _
        .After(Pack)
        .Executes(() =>
        {
            PackageExtensions.CheckUncommittedChanges(Solution);
        });

    Target Push => _ => _
        .Unlisted()
        .Requires(() => Project)
        .DependsOn(Pack/*, CheckUncommitted*/)
        .Executes(() =>
        {
            _packageInfoProvider.GetSelectedProjects(Project)
                .ForEach(x => PackageExtensions.PushPackage(Solution, x, OutputDirectory, NugetApiKey, NugetSource));
        });

    Target Tag => _ => _
        .Requires(() => Project)
        .DependsOn(Push)
        .Executes(() =>
        {
            _packageInfoProvider.GetSelectedProjects(Project)
                .ForEach(x => PackageExtensions.TagPackage(Solution, x));
        });

    Target Publish => _ => _
        .Description("Публикует Nuget-пакеты")
        .DependsOn(Tag);
    
    Target List => _ => _
        .Executes(() =>
        {
            var projects = _packageInfoProvider.Projects;
            Console.WriteLine("\nPackage list:");
            foreach (var projectInfo in projects)
            {
                Console.WriteLine(projectInfo.MenuItem);
            }
        });
    
    private AbsolutePath GetProjectPath(string name)
    {
        return Solution.AllProjects.FirstOrDefault(x => x.Name == name)?.Path ?? Solution.Path;
    }
}