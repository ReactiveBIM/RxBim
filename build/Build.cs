using System;
using System.Linq;
using System.Text;
using Bimlab.Nuke.Components;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using RxBim.Nuke.Revit.TestHelpers;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[GitHubActions("CI",
    GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[] { DevelopBranch, FeatureBranches },
    InvokedTargets = new[] { nameof(Test), nameof(IPublish.Publish) },
    ImportSecrets = new[] { "NUGET_API_KEY", "ALL_PACKAGES" })]
/*[GitHubActions("PullRequest",
    GitHubActionsImage.WindowsLatest,
    OnPullRequestBranches = new[] { DevelopBranch, "feature/**" },
    InvokedTargets = new[] { nameof(Test) })]*/
[GitHubActions("Publish",
    GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[] { MasterBranch, "release/**" },
    InvokedTargets = new[] { nameof(Test), nameof(IPublish.Publish) },
    ImportSecrets = new[] { "NUGET_API_KEY", "ALL_PACKAGES" })]
partial class Build : NukeBuild,
    IPublish
{
    const string MasterBranch = "master";
    const string DevelopBranch = "develop";
    const string FeatureBranches = "feature/**";

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
        .Before<IRestore>()
        .Executes(() =>
        {
            DotNetTest(settings => settings
                .SetProjectFile(From<IHazSolution>().Solution.Path)
                .SetConfiguration(From<IHazConfiguration>().Configuration)
                .SetFilter("FullyQualifiedName!~Integration"));
        });

    [Parameter] public bool AttachDebugger = false;

    /// <summary>
    /// Example target. Runs local only....
    /// </summary>
    Target IntegrationTests => _ => _
        .Executes(async () =>
        {
            var solution = From<IHazSolution>().Solution;

            var testProjectName = "RxBim.Transactions.IntegrationsTests";
            var project = solution.AllProjects.FirstOrDefault(x => x.Name == testProjectName) ??
                          throw new ArgumentException("project not found");

            var outputDirectory = solution.Directory / "testoutput";
            DotNetBuild(settings => settings
                .SetProjectFile(project)
                .SetConfiguration(Configuration.Debug)
                .SetOutputDirectory(outputDirectory));

            var assemblyName = testProjectName + ".dll";
            var assemblyPath = outputDirectory / assemblyName;

            var results = outputDirectory / "result.xml";
            RevitTestTasks.RevitTest(settings => settings
                .SetResults(results)
                .SetDir(outputDirectory)
                .SetProcessWorkingDirectory(outputDirectory)
                .EnableContinuous()
                .SetDebug(AttachDebugger)
                .SetAssembly(assemblyPath));

            var resultPath = solution.Directory / "result.html";

            await new ResultConverter()
                .Convert(results, resultPath);
        });

    private T From<T>()
        where T : INukeBuild
        => (T)(object)this;
}