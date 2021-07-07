namespace PikTools.Nuke.Revit
{
    using System.Collections.Generic;
    using System.Linq;
    using Generators;
    using global::Nuke.Common.ProjectModel;
    using Models;
    using static Constants;

    /// <inheritdoc />
    public class RevitWixBuilder : WixBuilder<RevitPackageContentsGenerator>
    {
        /// <inheritdoc />
        public override void GenerateAdditionalFiles(
            string rootProjectName,
            IEnumerable<Project> allProject,
            IEnumerable<AssemblyType> addInTypes,
            string outputDir)
        {
            var addInGenerator = new AddInGenerator();
            var addInTypesPerProjects = addInTypes
                .Select(x => new ProjectWithAssemblyType(
                    allProject.FirstOrDefault(proj => proj.Name == x.AssemblyName), x))
                .ToList();
            addInGenerator.GenerateAddInFile(rootProjectName, addInTypesPerProjects, outputDir);
        }

        /// <inheritdoc />
        protected override bool NeedGeneratePackageContents(string configuration)
        {
            return configuration == Release;
        }

        /// <inheritdoc />
        protected override string GetDebugInstallDir(Project project)
        {
            return "%AppDataFolder%/Autodesk/Revit/Addins/2019";
        }
    }
}