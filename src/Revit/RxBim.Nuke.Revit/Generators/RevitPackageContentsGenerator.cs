namespace RxBim.Nuke.Revit.Generators
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Nuke.Common.ProjectModel;
    using Models;
    using Nuke.Models;
    using RxBim.Nuke.Generators;

    /// <summary>
    /// Package contents manifest file generator.
    /// </summary>
    public class RevitPackageContentsGenerator : PackageContentsGenerator
    {
        /// <summary>
        /// Generates components enumeration from a <see cref="Project"/>.
        /// </summary>
        /// <param name="project">The project.</param>
        protected override IEnumerable<Components> GetComponents(Project project)
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