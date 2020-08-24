using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("project for debug")] string Project;

    [Solution] readonly Solution Solution;

    Target CopyDebugAddin => _ => _
        .Requires(() => Project)
        .Executes(() =>
        {
            var addinFile = $"{Project}.addin";
            var addinPath = Solution.Directory / "examples" / Project / addinFile;
            var revitPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Autodesk", "Revit", "Addins", "2019", addinFile
            );

            CopyFile(addinPath, revitPath, FileExistsPolicy.Overwrite);
        });

    Target CleanOutput => _ => _
        .Requires(() => Project)
        .Executes(() =>
        {
            var appPath = Project;
            var revitPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Autodesk", "Revit", "Addins", "2019", appPath
            );

            if (Directory.Exists(revitPath))
            {
                foreach (var file in Directory.EnumerateFiles(revitPath, "*", SearchOption.AllDirectories))
                {
                    DeleteFile(file);
                }
            }
        });

    Target CopyOutput => _ => _
        .Requires(() => Project)
        .DependsOn(CleanOutput, CopyDebugAddin, Compile)
        .Executes(() =>
        {
            var appPath = Project;
            var outputPath = Solution.Directory / "examples" / appPath / "bin" / "Debug" / "net471";

            var revitPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Autodesk", "Revit", "Addins", "2019", appPath
            );

            CopyDirectoryRecursively(outputPath, revitPath, DirectoryExistsPolicy.Merge, FileExistsPolicy.Overwrite);
        });

    Target Clean => _ => _
        .Executes(() =>
        {
            GlobDirectories(Solution.Directory, "**/bin", "**/obj")
                .Where(x => !IsDescendantPath(BuildProjectDirectory, x))
                .ForEach(DeleteDirectory);
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
                .SetProjectFile(Solution.Directory)
                .SetConfiguration(Configuration));
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var path = Solution.Directory / "out";
            var sourceProjects = Solution.AllProjects.Where(x => x.Path.ToString().Contains("\\src\\"));
            foreach (var project in sourceProjects)
            {
                DotNetPack(settings => settings
                    .SetConfiguration(Configuration)
                    .SetNoBuild(true)
                    .SetNoRestore(true)
                    .SetProject(project)
                    .SetOutputDirectory(path));
            }
        });

    Target Test => _ => _
        .Executes(() =>
        {
            DotNetTest(settings => settings
                .SetProjectFile(Solution));
        });
}