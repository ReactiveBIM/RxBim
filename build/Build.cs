using System;
using System.Linq;
using System.Text;
using Bimlab.Nuke.Components;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using RxBim.Nuke.Revit.TestHelpers;
using RxBim.Nuke.Versions;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[UnsetVisualStudioEnvironmentVariables]
[GitHubActions("CI",
    GitHubActionsImage.WindowsLatest,
    FetchDepth = 0,
    OnPushBranches = new[]
    {
        DevelopBranch,
        FeatureBranches
    },
    InvokedTargets = new[]
    {
        nameof(Test),
        nameof(IPublish.Publish)
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
partial class Build : NukeBuild
{
    const string MasterBranch = "master";
    const string DevelopBranch = "develop";
    const string FeatureBranches = "feature/**";
    const string ReleaseBranches = "release/**";
    const string HotfixBranches = "hotfix/**";

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
                .SetProjectFile(this.From<IHazSolution>().Solution.Path)
                .SetConfiguration(this.From<IHazConfiguration>().Configuration)
                .SetFilter("FullyQualifiedName!~Integration"));
        });

    [Parameter] public bool AttachDebugger;

    /// <summary>
    /// Example target. Runs local only..
    /// </summary>
    Target IntegrationTests => _ => _
        .Executes(async () =>
        {
            var solution = this.From<IHazSolution>().Solution;

            const string testProjectName = "RxBim.Transactions.Revit.IntegrationsTests";
            var project = solution.AllProjects.FirstOrDefault(x => x.Name == testProjectName) ??
                          throw new ArgumentException("project not found");

            var outputDirectory = solution.Directory / "testoutput";
            DotNetBuild(settings => settings
                .SetProjectFile(project)
                .SetConfiguration(Configuration.Debug)
                .SetOutputDirectory(outputDirectory));

            const string assemblyName = testProjectName + ".dll";
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

    string IVersionBuild.ProjectNamePrefix => "RxBim.";
}