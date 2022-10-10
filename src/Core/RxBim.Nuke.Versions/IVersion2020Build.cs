#pragma warning disable SA1205
#pragma warning disable SA1600
#pragma warning disable CS1591

namespace RxBim.Nuke.Versions
{
    using global::Nuke.Common;

    partial interface IVersionBuild
    {
        Target SetupEnv2020 => _ => _
            .Before(Compile, Restore, Pack, Release, Prerelease, Publish)
            .Executes(() =>
            {
                this.SetupEnvironment(AppVersion.Revit2020);
                this.SetupEnvironment(AppVersion.Autocad2020);
            });

        Target Compile2020 => _ => _.DependsOn(SetupEnv2020, Compile);

        Target Restore2020 => _ => _.DependsOn(SetupEnv2020, Restore);

        Target Pack2020 => _ => _.DependsOn(SetupEnv2020, Pack);

        Target Release2020 => _ => _.DependsOn(SetupEnv2020, Release);

        Target Prerelease2020 => _ => _.DependsOn(SetupEnv2020, Prerelease);

        Target Publish2020 => _ => _.DependsOn(SetupEnv2020, Publish);
    }
}