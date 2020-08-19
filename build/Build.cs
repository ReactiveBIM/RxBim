using System;
using System.IO;
using System.Linq;
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

    [Solution] readonly Solution Solution;

    Target CopyDebugAddin => _ => _
        .Executes(() =>
        {
            var addinFile = "PikTools.App.Example.addin";
            var addinPath = Solution.Directory / "PikTools.Application.Example" / addinFile;
            var revitPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Autodesk", "Revit", "Addins", "2019", addinFile
            );

            CopyFile(addinPath, revitPath, FileExistsPolicy.Overwrite);
        });

    Target CopyOutput => _ => _
        .DependsOn(CopyDebugAddin, Compile)
        .Executes(() =>
        {
            var appPath = "PikTools.Application.Example";
            var outputPath = Solution.Directory / appPath / "bin" / "Debug" / "net471";

            var revitPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Autodesk", "Revit", "Addins", "2019", appPath
            );

            CopyDirectoryRecursively(outputPath, revitPath, DirectoryExistsPolicy.Merge, FileExistsPolicy.Overwrite);
        });

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            GlobDirectories(Solution.Directory, "**/bin", "**/obj")
                .Where(x => !IsDescendantPath(BuildProjectDirectory, x))
                .ForEach(DeleteDirectory);
        });

    Target Restore => _ => _
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
            foreach (var project in Solution.Projects.Where(x => !x.Name.Contains("Example")))
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