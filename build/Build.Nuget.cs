using System.Linq;
using Bimlab.Nuke.Nuget;
using Nuke.Common;
using Nuke.Common.IO;
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

    /// <summary>
    /// Nuget api key
    /// </summary>
    const string NugetApiKey = "oy2iwdrmpblvtmbp5twam4vhwa3sfqaezclap3dk2fuwc4";

    AbsolutePath OutputDirectory => RootDirectory / "out";

    string _projectForPublish;

    [Parameter("Project name")]
    public string ProjectForPublish
    {
        get => _projectForPublish ??= PackageInfoProvider.GetSelectedMenuOption();
        set => _projectForPublish = value;
    }

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var path = Solution.Directory / "out";

            DotNetTasks.DotNetPack(settings => settings
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetProject(GetProjectPath(Project))
                .SetOutputDirectory(path));
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
            PackageInfoProvider.GetSelectedProjects(ProjectForPublish)
                .ForEach(x => PackageExtensions.PushPackage(Solution, x, OutputDirectory, NugetApiKey, NugetSource));
        });

    Target Tag => _ => _
        .Requires(() => ProjectForPublish)
        .DependsOn(Push)
        .Executes(() =>
        {
            PackageInfoProvider.GetSelectedProjects(ProjectForPublish)
                .ForEach(x => PackageExtensions.TagPackage(Solution, x));
        });

    Target Publish => _ => _
        .Description("Публикует Nuget-пакеты")
        .DependsOn(Tag);
    
    private AbsolutePath GetProjectPath(string name)
    {
        return Solution.AllProjects.FirstOrDefault(x => x.Name == name)?.Path ?? Solution.Path;
    }
    
}