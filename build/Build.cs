using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Bimlab.Nuke.Nuget;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using PikTools.Nuke;
using static Bimlab.Nuke.Nuget.PackageExtensions;
using static Nuke.Common.IO.CompressionTasks;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tooling.ProcessTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
partial class Build : PikToolsBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    
    PackageInfoProvider _packageInfoProvider;

    public Build()
    {
        _packageInfoProvider = new PackageInfoProvider(() => Solution);
    }

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

    Target Test => _ => _
        .Executes(() =>
        {
            DotNetTest(settings => settings
                .SetProjectFile(Solution));
        });

    Target PublishMsiBuildTool => _ => _
        .Executes(() =>
        {
            var projectName = "PikTools.MsiBuilder.Bin";
            var project = Solution.AllProjects.FirstOrDefault(x => x.Name == projectName);

            DotNetPublish(settings => settings
                .SetProject(project)
                .SetConfiguration(Configuration.Release));

            var publishDir = project.Directory / "bin" / Configuration.Release /
                             project.GetProperty("TargetFramework") / "publish";

            var zipFilePath = Solution.Directory / "out" / $"{project.Name}_{project.GetProperty("Version")}.zip";
            if (FileExists(zipFilePath))
            {
                DeleteFile(zipFilePath);
            }

            CompressZip(publishDir,
                zipFilePath,
                fileMode: FileMode.CreateNew,
                compressionLevel: CompressionLevel.Optimal);

           Logger.Info($"PikTools.MsiBuilder.Bin created successfully!\nResult placed in {zipFilePath}");
        });
}