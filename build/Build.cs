using System;
using System.Linq;
using System.Text;
using Bimlab.Nuke.Components;
using Bimlab.Nuke.Nuget;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI.SpaceAutomation;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[GitHubActions("CI",
    GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[] { DevelopBranch },
    OnPullRequestBranches = new[] { DevelopBranch, "feature/**" },
    InvokedTargets = new[] { nameof(Test), nameof(IPublish.Publish) },
    ImportSecrets = new[] { "NUGET_API_KEY", "ALL_PACKAGES" })]
[GitHubActions("Publish",
    GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[] { MasterBranch, "release/**" },
    InvokedTargets = new[] { nameof(Test), nameof(IPublish.Publish) },
    ImportSecrets = new[] { "NUGET_API_KEY", "ALL_PACKAGES" })]
[SpaceAutomation(
    name: "CI",
    image: "mcr.microsoft.com/dotnet/sdk:3.1",
    OnPushBranchIncludes = new[] { DevelopBranch },
    OnPush = true,
    InvokedTargets = new[] { nameof(Test) })]
partial class Build : NukeBuild,
    IPublish
{
    const string MasterBranch = "master";
    const string DevelopBranch = "develop";

    public Build()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    public static int Main() => Execute<Build>(x => x.From<IPublish>().List);

    Target Clean => _ => _
        .Executes(() =>
        {
            GlobDirectories(From<IHazSolution>().Solution.Directory, "**/bin", "**/obj")
                .Where(x => !IsDescendantPath(BuildProjectDirectory, x))
                .ForEach(FileSystemTasks.DeleteDirectory);
        });
    

    public Target Test => _ => _
        .Before(Clean)
        .Before<ICompile>()
        .Executes(() =>
        {
            DotNetTest(settings => settings
                .SetProjectFile(From<IHazSolution>().Solution.Path)
                .SetConfiguration(From<IHazConfiguration>().Configuration));
        });
    
    private T From<T>()
        where T : INukeBuild
        => (T)(object)this;
}