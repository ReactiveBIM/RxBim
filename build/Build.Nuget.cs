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

            DotNetTasks.DotNetPack(settings => settings
                .SetProject(GetProjectPath(Project))
                .SetOutputDirectory(path)
                .SetConfiguration(Configuration)
                .EnableNoBuild());
        });

    Target CheckUncommitted => _ => _
        .DependsOn(Pack)
        .Executes(() =>
        {
            ////PackageExtensions.CheckUncommittedChanges(Solution);
        });

    Target Push => _ => _
        .Unlisted()
        .Requires(() => Project)
        .DependsOn(CheckUncommitted)
        .Executes(() =>
        {
            var ppp = _packageInfoProvider.Projects; ////GetSelectedProjects(Project);
            /*_packageInfoProvider.GetSelectedProjects(Project)
                .ForEach(x => PackageExtensions.PushPackage(Solution, x, OutputDirectory, NugetApiKey, NugetSource));*/
        });

    Target Tag => _ => _
        .Requires(() => Project)
        .DependsOn(Push)
        .Executes(() =>
        {
            /*_packageInfoProvider.GetSelectedProjects(Project)
                .ForEach(x => PackageExtensions.TagPackage(Solution, x));*/
        });

    Target Publish => _ => _
        .Description("Публикует Nuget-пакеты")
        .DependsOn(Tag);
    
    private AbsolutePath GetProjectPath(string name)
    {
        return Solution.AllProjects.FirstOrDefault(x => x.Name == name)?.Path ?? Solution.Path;
    }
    
}