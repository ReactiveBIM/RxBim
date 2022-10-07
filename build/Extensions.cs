using System.IO;
using System.Linq;
using System.Text;
using Bimlab.Nuke.Components;
using Nuke.Common;
using Nuke.Common.Utilities.Collections;
using Serilog;

public static class Extensions
{
    public static T From<T>(this object obj)
        where T : INukeBuild =>
        (T)obj;
    
    public static void SetupEnvironment(this object obj, AppVersion appVersion)
    {
        obj.From<IHazSolution>()
            .Solution.AllProjects
            .Where(x => x.Directory.ToString().Contains(appVersion.AppName) || x.Name.Contains(appVersion.AppName))
            .ForEach(p =>
            {
                File.WriteAllText(p.Directory / "RxBim.Build.Props", appVersion.ToProjectProps(), Encoding.UTF8);
                Log.Information("Project {Project} set up for {App}", p.Name, appVersion.AppFullName);
            });
    }
}