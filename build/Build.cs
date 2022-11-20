using System;
using System.Collections.Generic;
using System.IO;
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
using Serilog;
using Versions;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Enumeration = Versions.Enumeration;

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
        MasterBranch, "release/**"
    },
    InvokedTargets = new[]
    {
        nameof(Test), nameof(IPublish.Publish)
    },
    ImportSecrets = new[]
    {
        "NUGET_API_KEY", "ALL_PACKAGES"
    })]
partial class Build : NukeBuild, IPublish
{
    const string MasterBranch = "master";
    const string DevelopBranch = "develop";
    const string FeatureBranches = "feature/**";
    const string Revit = "Revit";

    public Build()
    {
        Console.OutputEncoding = Encoding.UTF8;
    }

    public static int Main() => Execute<Build>(x => x.From<IPublish>().List);

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

    [Parameter] public bool AttachDebugger = false;

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

    [Parameter(ValueProviderMember = nameof(values))]
    string AppVersion { get; set; }

    IEnumerable<string> values => Enumeration.GetAll<AppVersion>().Select(x => x.ToString());

    Target SetupEnv => _ => _
        .Description("Sets the solution up to work with particular version of CAD/BIM.")
        .Requires(() => AppVersion)
        .Executes(() =>
        {
            var appVersion = Enumeration.GetAll<AppVersion>()
                .SingleOrError(x => x.ToString() == AppVersion, "Selected application not found");
            SetupEnvironment(appVersion);
        });

    Target ResetEnv => _ => _
        .Description("Resets the sulition to its defaults.")
        .Executes(() =>
        {
            From<IHazSolution>().Solution.Directory.GlobFiles("**/RxBim.Build.Props")
                .ForEach(DeleteFile);
        });

    private T From<T>()
        where T : INukeBuild
        => (T)(object)this;

    private void SetupEnvironment(AppVersion appVersion)
    {
        From<IHazSolution>().Solution.AllProjects
            .Where(x => x.Directory.ToString().Contains(appVersion.Name) || x.Name.Contains(appVersion.Name))
            .ForEach(p =>
            {
                File.WriteAllText(p.Directory / "RxBim.Build.Props", appVersion.ToProjectProps(), Encoding.UTF8);
                Log.Information("Project {project} set up for {app}", p.Name, appVersion.FullName);
            });
    }
}