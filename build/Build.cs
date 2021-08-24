using System;
using System.Text;
using Bimlab.Nuke.Nuget;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using RxBim.Nuke.Revit;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[GitHubActions("CI",
    GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[] { "master" },
    InvokedTargets = new[] { nameof(Publish) },
    ImportSecrets = new[] { "NUGET_API_KEY" })]
partial class Build : RevitRxBimBuild
{
    public static int Main() => Execute<Build>(x => x.GenerateProjectProps);

    readonly PackageInfoProvider PackageInfoProvider;

    public Build()
    {
        Console.OutputEncoding = Encoding.UTF8;
        PackageInfoProvider = new PackageInfoProvider(() => Solution);
    }
}