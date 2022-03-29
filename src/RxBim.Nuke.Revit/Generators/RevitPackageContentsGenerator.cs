namespace RxBim.Nuke.Revit.Generators
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Nuke.Common.ProjectModel;
    using Models;
    using Nuke.Generators;
    using Nuke.Models;

    /// <summary>
    /// Generates "PackageContents.xml" file for Revit plugin.
    /// </summary>
    public class RevitPackageContentsGenerator : PackageContentsGenerator
    {
        /// <inheritdoc/>
        protected override IEnumerable<Components> GetComponents(Project project, IEnumerable<string> assembliesNames)
        {
            var revitVersions = Enumerable.Range(2017, 4);
            return revitVersions
                .Select(revitVersion => new RevitComponents
                {
                    Description = $"Revit {revitVersion} part",
                    Platform = "Revit",
                    ModuleName = $"{project.Name}.addin",
                    OS = "Win64",
                    SeriesMax = $"R{revitVersion}",
                    SeriesMin = $"R{revitVersion}",
                });
        }
    }
}