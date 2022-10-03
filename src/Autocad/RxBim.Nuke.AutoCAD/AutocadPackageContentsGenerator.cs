namespace RxBim.Nuke.AutoCAD
{
    using System.Collections.Generic;
    using System.Linq;
    using Generators;
    using global::Nuke.Common.ProjectModel;
    using Models;

    /// <inheritdoc />
    public class AutocadPackageContentsGenerator : PackageContentsGenerator
    {
        /// <inheritdoc />
        protected override IEnumerable<Components> GetComponents(Project project, IEnumerable<string> assembliesNames)
        {
            const string acadMinVersion = "R23.0";

            return assembliesNames.Select(name => new AutocadComponents
            {
                Description = $"Autocad {acadMinVersion}+ part",
                Platform = "AutoCAD*",
                ModuleName = $"{project.Name}\\{name}.dll",
                OS = "Win64",
                SeriesMin = acadMinVersion
            });
        }
    }
}