namespace RxBim.Nuke.Revit.Generators
{
    using System;
    using System.Collections.Generic;
    using global::Nuke.Common.ProjectModel;
    using Models;
    using Nuke.Extensions;
    using Nuke.Generators;
    using Nuke.Models;

    /// <summary>
    /// Generates PackageContents.xml file for Revit plugin.
    /// </summary>
    public class RevitPackageContentsGenerator : PackageContentsGenerator
    {
        /// <inheritdoc/>
        protected override IEnumerable<Components> GetComponents(
            Project project,
            IEnumerable<string> assembliesNames,
            bool seriesMaxAny)
        {
            if (project.TryGetAppVersionNumber(out var revitVersion))
            {
                yield return new RevitComponents
                {
                    Description = $"Revit {revitVersion} part",
                    Platform = "Revit",
                    ModuleName = $"{project.Name}.addin",
                    OS = "Win64",
                    SeriesMax = $"R{revitVersion}",
                    SeriesMin = seriesMaxAny ? null : $"R{revitVersion}"
                };
            }
            else
            {
                throw new InvalidOperationException(
                    $"Revit version number not found for project: {project.Name}");
            }
        }
    }
}