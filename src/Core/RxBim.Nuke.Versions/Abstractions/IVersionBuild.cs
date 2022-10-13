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
            .Description("Configures the solution to work with a specific version of all CAD/BIM applications.")
            .Requires(() => AppVersionNumber)
            .Before(Compile, Restore, Pack, Release, Prerelease, Publish)
            .Executes(() => this.SetupEnvironment(AppVersionNumber));

        Target SetupEnvForApp => _ => _
            .Description("Configures the solution to work with a specific version of a specific CAD/BIM application.")
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

        Target CompileVersion => _ => _.DependsOn(SetupEnv, Compile);

        Target RestoreVersion => _ => _.DependsOn(SetupEnv, Restore);

        Target PackVersion => _ => _.DependsOn(SetupEnv, Pack);

        Target ReleaseVersion => _ => _.DependsOn(SetupEnv, Release);

        Target PrereleaseVersion => _ => _.DependsOn(SetupEnv, Prerelease);

        Target PublishVersion => _ => _.DependsOn(SetupEnv, Publish);

        AppVersion AppVersion { get; }

        AppVersionNumber AppVersionNumber { get; }
    }
}