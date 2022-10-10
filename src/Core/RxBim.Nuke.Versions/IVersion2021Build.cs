#pragma warning disable CS1591
#pragma warning disable SA1205
#pragma warning disable SA1600

namespace RxBim.Nuke.Versions
{
    using global::Nuke.Common;

    partial interface IVersionBuild
    {
        Target SetupEnv2021 => _ => _
            .Before(Compile, Restore, Pack, Release, Prerelease, Publish)
            .Executes(() =>
            {
                this.SetupEnvironment(AppVersion.Revit2021);
                this.SetupEnvironment(AppVersion.Autocad2021);
            });

        Target Compile2021 => _ => _.DependsOn(SetupEnv2021, Compile);

        Target Restore2021 => _ => _.DependsOn(SetupEnv2021, Restore);

        Target Pack2021 => _ => _.DependsOn(SetupEnv2021, Pack);

        Target Release2021 => _ => _.DependsOn(SetupEnv2021, Release);

        Target Prerelease2021 => _ => _.DependsOn(SetupEnv2021, Prerelease);

        Target Publish2021 => _ => _.DependsOn(SetupEnv2021, Publish);
    }
}