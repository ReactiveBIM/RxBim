using Bimlab.Nuke.Components;
using Nuke.Common;

interface IVersion2020Build : IPublish
{
    Target SetupEnv2020 => _ => _
        .Before<IRestore>()
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