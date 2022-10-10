namespace RxBim.Nuke.Versions
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using Bimlab.Nuke.Components;
    using global::Nuke.Common;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Utilities.Collections;
    using Serilog;

    /// <summary>
    /// Extensions for <see cref="IVersionBuild"/>.
    /// </summary>
    internal static class VersionBuildExtensions
    {
        /// <summary>
        /// Returns <see cref="INukeBuild"/>-based interface from <see cref="IVersionBuild"/> object.
        /// </summary>
        /// <param name="versionBuild">Source <see cref="IVersionBuild"/> object.</param>
        /// <typeparam name="T">Type of <see cref="INukeBuild"/>-based interface.</typeparam>
        public static T From<T>(this IVersionBuild versionBuild)
            where T : INukeBuild =>
            (T)versionBuild;

        /// <summary>
        /// Sets up the build for the specified version.
        /// </summary>
        /// <param name="versionBuild"><see cref="IVersionBuild"/> object.</param>
        /// <param name="appVersion"><see cref="AppVersion"/> object.</param>
        public static void SetupEnvironment(this IVersionBuild versionBuild, AppVersion appVersion)
        {
            versionBuild.From<IHazSolution>()
                .Solution.AllProjects
                .Where(appVersion.IsApplicableFor)
                .ForEach(p =>
                {
                    File.WriteAllText(p.Directory / "RxBim.Build.Props", appVersion.ToProjectProps(), Encoding.UTF8);
                    Log.Information("Project {Project} set up for {App}", p.Name, appVersion.Description);
                });
        }
    }
}