using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Bimlab.Nuke.Nuget;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using PikTools.Nuke.Builds;
using PikTools.Nuke.Revit;
using static Nuke.Common.IO.CompressionTasks;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
partial class Build : RevitPikToolsBuild
{
    // public static int Main() => Execute<Build>(x => x.CopyOutput);

    public static int Main() => Execute<Build>(x => x.GenerateProjectProps);

    PackageInfoProvider _packageInfoProvider;
    string _project;


    public Build()
    {
        Console.OutputEncoding = Encoding.UTF8;
        _packageInfoProvider = new PackageInfoProvider(() => Solution);
    }

    const string MsiBuilderProjectName = "PikTools.MsiBuilder.Bin";
    const string MsiBuilderEnv = "PIKTOOLS_MSIBUILDER_BIN";

    [Parameter("Select project")]
    override public string Project
    {
        get => _project ??= _packageInfoProvider.GetSelectedMenuOption();
        set => _project = value;
    }

    Project MsiBuilderProject =>
        Solution.AllProjects.FirstOrDefault(x => x.Name == MsiBuilderProjectName);

    string MsiBuilderArtifactPath => Solution.Directory / "out" /
                                     $"{MsiBuilderProject.Name}_{MsiBuilderProject.GetProperty("Version")}.zip";

    Target PublishMsiBuildTool => _ => _
        .Executes(() =>
        {
            DotNetPublish(settings => settings
                .SetProject(MsiBuilderProject)
                .SetConfiguration(Configuration.Release));

            var publishDir = MsiBuilderProject.Directory / "bin" / Configuration.Release /
                             MsiBuilderProject.GetProperty("TargetFramework") / "publish";

            if (FileExists((AbsolutePath)MsiBuilderArtifactPath))
            {
                DeleteFile(MsiBuilderArtifactPath);
            }

            CompressZip(publishDir,
                MsiBuilderArtifactPath,
                fileMode: FileMode.CreateNew,
                compressionLevel: CompressionLevel.Optimal);

            Logger.Info($"PikTools.MsiBuilder.Bin created successfully!\nResult placed in {MsiBuilderArtifactPath}");
        });

    Target InstallMsiBuildTool => _ => _
        .DependsOn(PublishMsiBuildTool)
        .Executes(() =>
        {
            var installDir = (AbsolutePath)Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) /
                             MsiBuilderProjectName;

            UncompressZip(MsiBuilderArtifactPath, installDir);

            Environment.SetEnvironmentVariable(
                MsiBuilderEnv,
                installDir / $"{MsiBuilderProjectName}.exe",
                EnvironmentVariableTarget.Machine);

            Logger.Info("PikTools.MsiBuilder.Bin installed successfully!");
        });
}