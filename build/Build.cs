using System;
using System.Linq;
using System.Text;
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
    OnPullRequestBranches = new[] { DevelopBranch },
    InvokedTargets = new[] { nameof(Test), nameof(Compile) },
    ImportSecrets = new[] { "NUGET_API_KEY", "ALL_PROJECTS" })]
[GitHubActions("Publish",
    GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[] { MasterBranch },
    InvokedTargets = new[] { nameof(Publish) },
    ImportSecrets = new[] { "NUGET_API_KEY", "ALL_PROJECTS" })]
[SpaceAutomation(
    name: "CI",
    image: "mcr.microsoft.com/dotnet/sdk:3.1",
    OnPushBranchIncludes = new[] { DevelopBranch },
    OnPush = true,
    InvokedTargets = new[] { nameof(Test) })]
partial class Build : NukeBuild
{
    public Build()
    {
        Console.OutputEncoding = Encoding.UTF8;
        _packageInfoProvider = new PackageInfoProvider(() => Solution);
    }

    public static int Main() => Execute<Build>(x => x.List);

    Target Clean => _ => _
        .Executes(() =>
        {
            GlobDirectories(Solution.Directory, "**/bin", "**/obj")
                .Where(x => !IsDescendantPath(BuildProjectDirectory, x))
                .ForEach(FileSystemTasks.DeleteDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution.Path));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(settings => settings
                .SetProjectFile(Solution.Path)
                .SetConfiguration(Configuration));
        });

    public Target Test => _ => _
        .Executes(() =>
        {
            DotNetTest(settings => settings
                .SetProjectFile(Solution.Path)
                .SetConfiguration(Configuration));
        });
}