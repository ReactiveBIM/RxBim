namespace RxBim.Nuke.AutoCAD
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Generators;
    using global::Nuke.Common.ProjectModel;
    using Models;
    using Versions;

    /// <inheritdoc />
    public class AutocadPackageContentsGenerator : PackageContentsGenerator
    {
        /// <inheritdoc />
        protected override IEnumerable<Components> GetComponents(
            Project project,
            IEnumerable<string> assembliesNames,
            bool seriesMaxAny)
        {
            var acadVersion = GetAcadRuntimeVersion(project);

            if (!string.IsNullOrWhiteSpace(acadVersion))
            {
                return assembliesNames.Select(name => new AutocadComponents
                {
                    Description = $"Autocad {acadVersion} part",
                    Platform = "AutoCAD*",
                    ModuleName = $"{project.Name}\\{name}.dll",
                    OS = "Win64",
                    SeriesMin = $"R{acadVersion}",
                    SeriesMax = seriesMaxAny ? null : $"R{acadVersion}"
                });
            }

            return Array.Empty<Components>();
        }

        private static string GetAcadRuntimeVersion(Project project)
        {
            var appVersion = GetAppVersion(project, AppType.Autocad);

            if (appVersion == null)
            {
                throw new InvalidOperationException(
                    $"CAD application version settings not found for project: {project.Name}");
            }

            var rxVersion = (RuntimeVersion)appVersion.Settings.Single(x => x is RuntimeVersion);
            return rxVersion.Value;
        }

        private static AppVersion? GetAppVersion(Project project, AppType type)
        {
            return project.TryGetAppVersionNumber(out var number)
                ? AppVersion.GetAll().FirstOrDefault(x =>
                    x.Type == type &&
                    x.Settings.Any(s => s is ApplicationVersion appVer && appVer.Value == number))
                : null;
        }
    }
}