using System;
using Bimlab.Nuke.Nuget;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.GitHub;

partial class Build
{
    Target List => _ => _
        .Executes(() =>
        {
            var projects = _packageInfoProvider.Projects;
            Console.WriteLine("\nPackage list:");
            foreach (var projectInfo in projects)
            {
                Console.WriteLine(projectInfo.MenuItem);
            }
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Requires(() => Projects)
        .Executes(() =>
        {
            _packageInfoProvider.GetSelectedProjects(Projects)
                .ForEach(x => PackInternal(Solution, x, OutputDirectory, Configuration));
        });

    /// <summary>
    /// Makes a package
    /// </summary>
    /// <param name="solution">Solution</param>
    /// <param name="project">Project</param>
    /// <param name="outDir">Output directory</param>
    /// <param name="configuration">Build configuration</param>
    static void PackInternal(
        Solution solution,
        ProjectInfo project,
        AbsolutePath outDir,
        string configuration)
    {
        var path = solution.GetProject(project.ProjectName)?.Path;

        if (!string.IsNullOrEmpty(path))
        {
            DotNetTasks.DotNetPack(s => s
                .SetProject(path)
                .SetOutputDirectory(outDir)
                .SetConfiguration(configuration)
                .EnableNoBuild());
        }
    }

    Target CheckUncommitted => _ => _
        .After(Pack)
        .Executes(() =>
        {
            PackageExtensions.CheckUncommittedChanges(Solution);
        });

    Target Push => _ => _
        .Requires(() => NugetApiKey)
        .Unlisted()
        .DependsOn(Pack, CheckUncommitted)
        .Executes(() =>
        {
            _packageInfoProvider.GetSelectedProjects(Projects)
                .ForEach(x => PackageExtensions.PushPackage(Solution, x, OutputDirectory, NugetApiKey, NugetSource));
        });

    Target Tag => _ => _
        .DependsOn(Push)
        .Executes(() =>
        {
            _packageInfoProvider.GetSelectedProjects(Projects)
                .ForEach(x => PackageExtensions.TagPackage(Solution, x));
        });
    
    Target PushGit => _ => _
        .After(Tag)
        .Executes(() =>
        {
            GitTasks.Git("push --tags");
        });

    Target Publish => _ => _
        .Description("Publish nuget packages")
        .DependsOn(Tag, PushGit);
}