namespace RxBim.Nuke.Versions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Bimlab.Nuke.Components;
    using global::Nuke.Common;
    using global::Nuke.Common.Utilities.Collections;
    using Serilog;

    /// <summary>
    /// Extensions for <see cref="IVersionBuild"/>.
    /// </summary>
    public static class VersionBuildExtensions
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
        public static void SetupEnvironment(this IVersionBuild versionBuild)
        {
            if (string.IsNullOrEmpty(versionBuild.CurrentAppVersionNumber))
                throw new InvalidOperationException("CurrentAppVersionNumber must be set!");

            foreach (var appVersion in AppVersion.GetAll()
                         .GroupBy(x => x.Type)
                         .Select(x =>
                             x.FirstOrDefault(av => av.Settings.ContainsAppVersionSetting(versionBuild.CurrentAppVersionNumber))))
            {
                if (appVersion is null)
                    continue;

                versionBuild.SetupEnvironment(appVersion);
            }
        }

        /// <summary>
        /// Sets up the build for the specified version.
        /// </summary>
        /// <param name="versionBuild"><see cref="IVersionBuild"/> object.</param>
        /// <param name="appVersionNumber">Application version number.</param>
        public static void SetupEnvironment(this IVersionBuild versionBuild, string appVersionNumber)
        {
            if (string.IsNullOrEmpty(versionBuild.CurrentAppVersionNumber))
            {
                var versionNumber = VersionNumber.GetAll()
                    .FirstOrDefault(x => appVersionNumber.Equals(x, StringComparison.Ordinal));

                if (versionNumber is not null)
                    versionBuild.CurrentAppVersionNumber = versionNumber;
            }

            versionBuild.SetupEnvironment();
        }

        /// <summary>
        /// Sets up the build for the specified version.
        /// </summary>
        /// <param name="versionBuild"><see cref="IVersionBuild"/> object.</param>
        /// <param name="appVersion"><see cref="AppVersion"/> object.</param>
        public static void SetupEnvironment(this IVersionBuild versionBuild, AppVersion appVersion)
        {
            if (string.IsNullOrEmpty(versionBuild.CurrentAppVersion))
                versionBuild.CurrentAppVersion = appVersion;

            versionBuild.From<IHazSolution>()
                .Solution.AllProjects
                .Where(appVersion.IsApplicableFor)
                .ForEach(p =>
                {
                    File.WriteAllText(p.Directory / "RxBim.Build.Props", appVersion.ToProjectProps(), Encoding.UTF8);
                    Log.Information("Project {Project} set up for {App}", p.Name, appVersion.Description);
                });
        }

        private static bool ContainsAppVersionSetting(
            this IEnumerable<ProjectSetting> settings,
            string appVersionNumber)
        {
            return settings.Any(x => x is ApplicationVersion appVer && appVer.Value.Equals(appVersionNumber));
        }
    }
}