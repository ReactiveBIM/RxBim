﻿namespace RxBim.Nuke.Revit
{
    using System.Collections.Generic;
    using System.Linq;
    using Builders;
    using Builds;
    using Generators;
    using global::Nuke.Common.ProjectModel;
    using Models;

    /// <inheritdoc />
    public class RevitInstallerBuilder : InstallerBuilder<RevitPackageContentsGenerator>
    {
        /// <inheritdoc />
        public override void GenerateAdditionalFiles(
            string? rootProjectName,
            IEnumerable<Project> allProject,
            IEnumerable<AssemblyType> allAssembliesTypes,
            string outputDir)
        {
            var addInGenerator = new AddInGenerator();
            var addInTypesPerProjects = allAssembliesTypes
                .Select(x => new ProjectWithAssemblyType(
                    allProject.First(proj => proj.Name == x.AssemblyName), x))
                .ToList();
            addInGenerator.GenerateAddInFile(rootProjectName, addInTypesPerProjects, outputDir);
        }

        /// <inheritdoc />
        protected override bool NeedGeneratePackageContents(string configuration)
        {
            return configuration == Configuration.Release;
        }

        /// <inheritdoc />
        protected override string GetDebugInstallDir(Project project)
        {
            return "%AppDataFolder%/Autodesk/Revit/Addins/2019";
        }
    }
}