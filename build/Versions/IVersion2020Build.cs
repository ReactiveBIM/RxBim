using Bimlab.Nuke.Components;
using Nuke.Common;

interface IVersion2020Build : IPublish
{
    Target Compile2020 => _ => _
        .Unlisted()
        .Executes(SetupEnv2020)
        .Triggers(Compile);

    Target Restore2020 => _ => _
        .Unlisted()
        .Executes(SetupEnv2020)
        .Triggers(Restore);

    Target Pack2020 => _ => _
        .Unlisted()
        .Executes(SetupEnv2020)
        .Triggers(Pack);

    Target Release2020 => _ => _
        .Unlisted()
        .Executes(SetupEnv2020)
        .Triggers(Release);

    Target Prerelease2020 => _ => _
        .Unlisted()
        .Executes(SetupEnv2020)
        .Triggers(Prerelease);

    Target Publish2020 => _ => _
        .Unlisted()
        .Executes(SetupEnv2020)
        .Triggers(Publish);
    
    void SetupEnv2020()
    {
        this.SetupEnvironment(AppVersion.Revit2020);
        this.SetupEnvironment(AppVersion.Autocad2020);
    }
}