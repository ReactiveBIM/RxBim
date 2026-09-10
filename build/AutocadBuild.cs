using Nuke.Common.Execution;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tooling;
using RxBim.Nuke.AutoCAD;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

/// <summary>
/// Builds AutoCAD sample installers using AutoCAD package manifests.
/// </summary>
[UnsetVisualStudioEnvironmentVariables]
class AutocadBuild : AutocadRxBimBuild
{
    /// <inheritdoc />
    protected override void RestoreInternal()
    {
        DotNetRestore(settings => settings.SetProjectFile(ProjectForInstallBuild.Path));
    }

    /// <inheritdoc />
    protected override void CompileInternal()
    {
        DotNetBuild(settings => settings
            .SetProjectFile(ProjectForInstallBuild.Path)
            .SetConfiguration(Configuration)
            .Apply(CompileSettings));
    }
}