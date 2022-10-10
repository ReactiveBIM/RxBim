#pragma warning disable CS1591
#pragma warning disable SA1205
#pragma warning disable SA1600

namespace RxBim.Nuke.Versions
{
    using Bimlab.Nuke.Components;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Utilities.Collections;
    using static global::Nuke.Common.IO.FileSystemTasks;

    public partial interface IVersionBuild : IPublish
    {
        Target SetupEnv => _ => _
            .Description("Sets the solution up to work with particular version of CAD/BIM.")
            .Requires(() => AppVersion)
            .Executes(() =>
            {
                var appVersion = AppVersion.GetAll()
                    .SingleOrError(x => x.Description == AppVersion.Description, "Selected version not found");
                this.SetupEnvironment(appVersion!);
            });

        Target ResetEnv => _ => _
            .Description("Resets the solution to its defaults.")
            .Executes(() =>
            {
                this.From<IHazSolution>()
                    .Solution.Directory.GlobFiles("**/RxBim.Build.Props")
                    .ForEach(DeleteFile);
            });

        AppVersion AppVersion { get; }
    }
}