using System;
using System.Linq;
using System.Text;
using Bimlab.Nuke.Components;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.CI.SpaceAutomation;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[GitHubActions("CI",
    GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[] { DevelopBranch },
    OnPullRequestBranches = new[] { DevelopBranch },
    InvokedTargets = new[] { nameof(Test), nameof(ICompile.Compile) },
    ImportSecrets = new[] { "NUGET_API_KEY", "ALL_PROJECTS" })]
[GitHubActions("Publish",
    GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[] { MasterBranch },
    InvokedTargets = new[] { nameof(IPublish.Publish) },
    ImportSecrets = new[] { "NUGET_API_KEY", "ALL_PROJECTS" })]
[SpaceAutomation(
    name: "CI",
    image: "mcr.microsoft.com/dotnet/sdk:3.1",
    OnPushBranchIncludes = new[] { DevelopBranch },
    OnPush = true,
    InvokedTargets = new[] { nameof(Test) })]
partial class Build : NukeBuild,
    IHazSolution,
    IRestore,
    ICompile,
    IHazGitRepository,
    IPack,
    IPublish
{
    const string MasterBranch = "master";
    const string DevelopBranch = "develop";
    
    /// <summary>
    /// Solution
    /// </summary>
    [Solution]
    public readonly Solution Solution;

    Solution IHazSolution.Solution => Solution;

    [UsedImplicitly]
    Target Clean => _ => _
        .Before<IRestore>()
        .Executes(() =>
        {
            GlobDirectories(Solution.Directory, "**/bin", "**/obj")
                .Where(x => !IsDescendantPath(BuildProjectDirectory, x))
                .ForEach(FileSystemTasks.DeleteDirectory);
        });

    [UsedImplicitly]
    public Target Test => _ => _
        .Executes(() =>
        {
            DotNetTest(settings => settings
                .SetProjectFile(Solution.Path)
                .SetConfiguration(From<IHazConfiguration>().Configuration));
        });

    public Build()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    static int Main() => Execute<Build>(x => x.From<IPublish>().List);

    T From<T>()
        where T : INukeBuild
        => (T)(object)this;
}