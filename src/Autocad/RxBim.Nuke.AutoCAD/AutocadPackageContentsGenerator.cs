namespace RxBim.Nuke.AutoCAD
{
    using System.Collections.Generic;
    using Generators;
    using global::Nuke.Common.ProjectModel;
    using Models;

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