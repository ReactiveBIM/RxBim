using Bimlab.Nuke.Components;
using Nuke.Common;

interface IVersion2021Build : IPublish
{
    Target Compile2021 => _ => _
        .Unlisted()
        .Executes(SetupEnv2021)
        .Triggers(Compile);

    Target Restore2021 => _ => _
        .Unlisted()
        .Executes(SetupEnv2021)
        .Triggers(Restore);

    Target Pack2021 => _ => _
        .Unlisted()
        .Executes(SetupEnv2021)
        .Triggers(Pack);

    Target Release2021 => _ => _
        .Unlisted()
        .Executes(SetupEnv2021)
        .Triggers(Release);

    Target Prerelease2021 => _ => _
        .Unlisted()
        .Executes(SetupEnv2021)
        .Triggers(Prerelease);

    Target Publish2021 => _ => _
        .Unlisted()
        .Executes(SetupEnv2021)
        .Triggers(Publish);
    
    void SetupEnv2021()
    {
        this.SetupEnvironment(AppVersion.Revit2021);
        this.SetupEnvironment(AppVersion.Autocad2021);
    }
}