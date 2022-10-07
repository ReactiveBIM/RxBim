using System.Collections.Generic;
using System.Linq;
using Bimlab.Nuke.Components;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;

interface IVersionBuild : IVersion2020Build, IVersion2021Build
{
    Target SetupEnv => _ => _
        .Description("Sets the solution up to work with particular version of CAD/BIM.")
        .Executes(() =>
        {
            var appVersion = Enumeration.GetAll<AppVersion>()
                .SingleOrError(x => x.ToString() == CurrentAppVersion, "Selected application not found");
            this.SetupEnvironment(appVersion);
        });

    Target ResetEnv => _ => _
        .Description("Resets the solution to its defaults.")
        .Executes(() =>
        {
            this.From<IHazSolution>()
                .Solution.Directory.GlobFiles("**/RxBim.Build.Props")
                .ForEach(DeleteFile);
        });

    [Parameter(ValueProviderMember = nameof(AppVersionValues))]
    string CurrentAppVersion { get; set; }

    IEnumerable<string> AppVersionValues => Enumeration.GetAll<AppVersion>().Select(x => x.ToString());
}