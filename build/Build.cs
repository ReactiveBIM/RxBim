using System;
using System.Text;
using Bimlab.Nuke.Components;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Tools.DotNet;
using RxBim.Nuke.Revit;
using RxBim.Nuke.Versions;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[UnsetVisualStudioEnvironmentVariables]
[GitHubActions("CI",
    GitHubActionsImage.WindowsLatest,
    FetchDepth = 0,
    OnPushBranches = new[]
    {
        DevelopBranch, FeatureBranches, BugfixBranches
    },
    InvokedTargets = new[]
    {
        nameof(Test), nameof(IPublish.Publish)
    },
    ImportSecrets = new[]
    {
        "NUGET_API_KEY", "ALL_PACKAGES"
    })]
[GitHubActions("Publish",
    GitHubActionsImage.WindowsLatest,
    FetchDepth = 0,
    OnPushBranches = new[]
    {
        MasterBranch, ReleaseBranches, HotfixBranches
    },
    InvokedTargets = new[]
    {
        nameof(Test), nameof(IPublish.Publish)
    },
    ImportSecrets = new[]
    {
        "NUGET_API_KEY", "ALL_PACKAGES"
    })]
[PublicAPI]
partial class Build : RevitRxBimBuild, IVersions
{
    const string MasterBranch = "master";
    const string DevelopBranch = "develop";
    const string ReleaseBranches = "release/**";
    const string HotfixBranches = "hotfix/**";
    const string FeatureBranches = "feature/**";
    const string BugfixBranches = "bugfix/**";

    public Build()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    public static int Main() => Execute<Build>(x => x.From<IPublish>().Compile);

    public Target Test => _ => _
        .Before<IClean>()
        .Before<IRestore>()
        .Executes(() =>
        {
            DotNetTest(settings => settings
                .SetProjectFile(this.From<IHasSolution>().Solution.Path)
                .SetConfiguration(this.From<IHasConfiguration>().Configuration)
                .SetFilter("FullyQualifiedName!~Integration"));
        });

    string IVersionBuild.ProjectNamePrefix => "RxBim.";
}