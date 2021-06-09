namespace PikTools.Nuke.AutoCAD
{
    using System.Collections.Generic;
    using System.Linq;
    using Generators;
    using Generators.Models;
    using global::Nuke.Common.ProjectModel;

    /// <inheritdoc />
    public class AutocadPackageContentsGenerator : PackageContentsGenerator
    {
        /// <inheritdoc />
        protected override IEnumerable<Components> GetComponents(Project project)
        {
            const string acadMinVersion = "R23.0";

            return new[]
            {
                new AutocadComponents
                {
                    Description = $"Autocad {acadMinVersion}+ part",
                    Platform = "AutoCAD*",
                    ModuleName = $"{project.Name}\\{project.Name}.dll",
                    OS = "Win64",
                    SeriesMin = acadMinVersion
                }
            };
        }
    }
}