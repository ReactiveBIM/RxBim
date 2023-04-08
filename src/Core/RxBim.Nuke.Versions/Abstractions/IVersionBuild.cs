#pragma warning disable CS1591, SA1205, SA1600

namespace RxBim.Nuke.Versions
{
    using Bimlab.Nuke.Components;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Utilities.Collections;
    using static global::Nuke.Common.IO.FileSystemTasks;

    public interface IVersionBuild : IPublish
    {
        Target SetupEnv => _ => _
            .Description("Configures the solution to work with a specific version of all CAD/BIM applications.")
            .Requires(() => CurrentAppVersionNumber)
            .Executes(this.SetupEnvironment);

        Target SetupEnvForApp => _ => _
            .Description("Configures the solution to work with a specific version of a specific CAD/BIM application.")
            .Requires(() => CurrentAppVersion)
            .Executes(() =>
            {
                var appVersion = AppVersion.GetAll()
                    .SingleOrError(x => x.Description == CurrentAppVersion.Description, "Selected version not found");
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

        AppVersion CurrentAppVersion { get; set; }

        VersionNumber CurrentAppVersionNumber { get; set; }
    }
}