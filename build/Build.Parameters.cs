using System.Linq;
using Bimlab.Nuke.Nuget;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using RxBim.Nuke.Builds;

partial class Build
{
    const string MasterBranch = "master";
    const string DevelopBranch = "develop";
    const string ReleaseBranchPrefix = "release";
    const string HotfixBranchPrefix = "hotfix";
    
    readonly PackageInfoProvider _packageInfoProvider;
    string[] _projects;

    [Solution]
    [JetBrains.Annotations.NotNull]
    public readonly Solution Solution;

    AbsolutePath OutputDirectory => RootDirectory / "out";

    [Parameter("Project name list")]
    public string[] Projects
    {
        get
        {
            return _projects ??= GetProjects();

            string[] GetProjects() => AllProjects
                ? _packageInfoProvider.Projects.Select(x => x.ProjectName).ToArray()
                : _packageInfoProvider.GetSelectedMenuOption();
        }

        set => _projects = value;
    }

    [Parameter("Select all projects")]
    public bool AllProjects { get; set; } = !IsLocalBuild;

    /// <summary>
    /// Configuration to build - Default is 'Debug' (local) or 'Release' (server)
    /// </summary>
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    public Configuration Configuration { get; set; } = IsLocalBuild ? Configuration.Debug : Configuration.Release;

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

    [Parameter("NuGet API key")]
    public readonly string NugetApiKey;
}