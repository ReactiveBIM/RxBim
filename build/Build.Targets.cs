#pragma warning disable CS1591, SA1205, SA1600

using Bimlab.Nuke.Components;
using RxBim.Nuke.Versions;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Serilog;

partial class Build
{
    GitHubActions GitHubActions => GitHubActions.Instance;
    
    Target SetupEnv2019 => _ => _
        .OnlyWhenDynamic(() => GitHubActions != null && !string.IsNullOrEmpty(GitHubActions.Job))
        .Before<IPublish>(x => x.Publish, x => x.Prerelease, x => x.Release, x => x.List)
        .Before<IPack>(x => x.Pack)
        .Before<ICompile>(x => x.Compile)
        .Before<IRestore>(x => x.Restore)
        .Before(Clean, Test, IntegrationTests)
        .Executes(() => Log.Information("Job: {Job}", GitHubActions.Job))
        .Executes(() => this.SetupEnvironment("2019"));

    Target SetupEnv2020 => _ => _
        .Before<IPublish>(x => x.Publish, x => x.Prerelease, x => x.Release, x => x.List)
        .Before<IPack>(x => x.Pack)
        .Before<ICompile>(x => x.Compile)
        .Before<IRestore>(x => x.Restore)
        .Before(Clean, Test, IntegrationTests)
        .Executes(() => this.SetupEnvironment("2020"));

    Target SetupEnv2021 => _ => _
        .Before<IPublish>(x => x.Publish, x => x.Prerelease, x => x.Release, x => x.List)
        .Before<IPack>(x => x.Pack)
        .Before<ICompile>(x => x.Compile)
        .Before<IRestore>(x => x.Restore)
        .Before(Clean, Test, IntegrationTests)
        .Executes(() => this.SetupEnvironment("2021"));

    Target SetupEnv2022 => _ => _
        .Before<IPublish>(x => x.Publish, x => x.Prerelease, x => x.Release, x => x.List)
        .Before<IPack>(x => x.Pack)
        .Before<ICompile>(x => x.Compile)
        .Before<IRestore>(x => x.Restore)
        .Before(Clean, Test, IntegrationTests)
        .Executes(() => this.SetupEnvironment("2022"));

    Target SetupEnv2023 => _ => _
        .Before<IPublish>(x => x.Publish, x => x.Prerelease, x => x.Release, x => x.List)
        .Before<IPack>(x => x.Pack)
        .Before<ICompile>(x => x.Compile)
        .Before<IRestore>(x => x.Restore)
        .Before(Clean, Test, IntegrationTests)
        .Executes(() => this.SetupEnvironment("2023"));
}