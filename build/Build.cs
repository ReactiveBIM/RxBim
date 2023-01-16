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
using Nuke.Common.Utilities.Collections;
using RxBim.Nuke.Revit.TestHelpers;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[UnsetVisualStudioEnvironmentVariables]
[GitHubActions("CI",
    GitHubActionsImage.WindowsLatest,
    FetchDepth = 0,
    OnPushBranches = new[]
    {
        DevelopBranch, FeatureBranches
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

    public static int Main() => Execute<Build>(x => x.From<ICompile>().Compile);

    Target Clean => _ => _
        .Before<IRestore>()
        .Executes(() =>
        {
            GlobDirectories(From<IHazSolution>().Solution.Directory, "**/bin", "**/obj")
                .Where(x => !IsDescendantPath(BuildProjectDirectory, x))
                .ForEach(DeleteDirectory);
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

    [Parameter] public bool AttachDebugger;

    /// <summary>
    /// Example target. Runs local only....
    /// </summary>
    Target IntegrationTests => _ => _
        .Executes(async () =>
        {
            var solution = From<IHazSolution>().Solution;

            var testProjectName = "RxBim.Transactions.Revit.IntegrationsTests";
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

    Target Prerelease2019 => _ => _
        .DependsOn(SetupEnv2019)
        .Triggers(From<IPublish>().Prerelease);
    
    Target Prerelease2020 => _ => _
        .DependsOn(Prerelease2019)
        .DependsOn(SetupEnv2020)
        .Triggers(From<IPublish>().Prerelease);
    
    Target Prerelease2021 => _ => _
        .DependsOn(Prerelease2020)
        .DependsOn(SetupEnv2021)
        .Triggers(From<IPublish>().Prerelease);
    
    Target Prerelease2022 => _ => _
        .DependsOn(Prerelease2021)
        .DependsOn(SetupEnv2022)
        .Triggers(From<IPublish>().Prerelease);
    
    Target Prerelease2023 => _ => _
        .DependsOn(Prerelease2022)
        .DependsOn(SetupEnv2023)
        .Triggers(From<IPublish>().Prerelease);

    T From<T>()
        where T : INukeBuild
        => (T)(object)this;
}