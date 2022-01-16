using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Bimlab.Nuke.Components;
using ConsoleTableExt;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
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
                .SetFilter("FullyQualifiedName!~IntegrationTests"));
        });

    Target IntegrationTests => _ => _
        // .OnlyWhenDynamic(() => false)
        .Executes(() =>
        {
            var solution = From<IHazSolution>().Solution;
            //
            var testProjectName = "RxBim.Example.IntegrationTests";
            // var project = solution.AllProjects.FirstOrDefault(x => x.Name == testProjectName) ??
            //               throw new ArgumentException("project not found");
            //
            var outputDirectory = solution.Directory / "testoutput";
            // DotNetBuild(settings => settings
            //     .SetProjectFile(project)
            //     .SetConfiguration("Debug")
            //     .SetOutputDirectory(outputDirectory));
            //
            var assemblyName = testProjectName + ".dll";
            // var assemblyPath = outputDirectory / assemblyName;
            //
            var results = outputDirectory / "result.xml";
            // RevitTestTasks.RevitTest(settings => settings
            //     // .SetRevit(RevitVersions._2021)
            //     .SetResults(results)
            //     .SetDir(outputDirectory)
            //     .SetProcessWorkingDirectory(outputDirectory)
            //     .EnableContinuous()
            //     .SetAssembly(assemblyPath));

            var doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(results));

            var list = doc.DocumentElement.SelectNodes("/test-results/test-suite/results/test-suite");

            foreach (XmlElement node in list)
            {
                var testFixtureName = node.Attributes["name"].Value;
                var cases = node.FirstChild.ChildNodes;
                var data = new List<List<object>>();
                foreach (XmlElement @case in cases)
                {
                    var caseName = @case.Attributes["name"].Value;
                    var isSuccess = @case.Attributes["success"].Value == "True";
                    var executionTime = @case.Attributes["time"].Value;
                    var failureMessage = isSuccess ? "-" : @case.FirstChild.FirstChild.InnerText + @case.FirstChild.LastChild.InnerText;
                    data.Add(new List<object>
                    {
                        caseName,
                        isSuccess ? "✔" : "❌",
                        executionTime,
                        failureMessage
                    });
                }

                ConsoleTableBuilder.From(() => new ConsoleTableBaseData
                    {
                        Rows = data,
                        Column = new List<object>
                        {
                            "Test name", "Result", "Execution time", "Failure message"
                        }
                    })
                    .WithFormat(ConsoleTableBuilderFormat.Default)
                    .WithTitle($"{assemblyName} - {testFixtureName} - ✔")
                    .ExportAndWriteLine();
            }
        });

    private T From<T>()
        where T : INukeBuild
        => (T)(object)this;
}