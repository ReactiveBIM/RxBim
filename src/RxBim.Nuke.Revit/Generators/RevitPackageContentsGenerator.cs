namespace RxBim.Nuke.Revit.Generators
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Nuke.Common.ProjectModel;
    using Models;
    using RxBim.Nuke.Generators;
    using RxBim.Nuke.Generators.Models;

    /// <summary>
    /// Генерирует PackageContents.xml
    /// </summary>
    public class RevitPackageContentsGenerator : PackageContentsGenerator
    {
        /// <summary>
        /// Возвращает компоненты для Revit
        /// </summary>
        /// <param name="project">Проект</param>
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