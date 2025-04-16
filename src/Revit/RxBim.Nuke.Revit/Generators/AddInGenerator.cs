namespace RxBim.Nuke.Revit.Generators
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;
    using global::Nuke.Common.ProjectModel;
    using JetBrains.Annotations;
    using Models;
    using Nuke.Extensions;
    using Nuke.Models;
    using static Constants;

    /// <summary>
    /// The Generator for Revit addin manifest files.
    /// </summary>
    [PublicAPI]
    public class AddInGenerator
    {
        /// <summary>
        /// Generate a new addin file.
        /// </summary>
        /// <param name="rootProjectName">The root project name.</param>
        /// <param name="addInTypesPerProjects">Addin types for registration in Revit.</param>
        /// <param name="outputDirectory">The output directory path.</param>
        public void GenerateAddInFile(
            string? rootProjectName,
            IReadOnlyList<ProjectWithAssemblyType> addInTypesPerProjects,
            string outputDirectory)
        {
            var pluginTypes = addInTypesPerProjects
                .Where(x => x.AssemblyType.IsPluginType())
                .ToList();

            if (!addInTypesPerProjects.Any())
            {
                throw new ArgumentException(
                     $"Project {rootProjectName} should contain any {RxBimCommand} or {RxBimApplication} type!");
            }

            GenerateAddIn(rootProjectName, pluginTypes, outputDirectory);
        }

        /// <summary>
        /// Generates a new addin file.
        /// </summary>
        /// <param name="rootProjectName">The root project name.</param>
        /// <param name="addinTypesPerProjects">Addin types for registration in Revit.</param>
        /// <param name="output">The output directory path.</param>
        protected virtual void GenerateAddIn(
            string? rootProjectName,
            IEnumerable<ProjectWithAssemblyType> addinTypesPerProjects,
            string output)
        {
            var addIns = new List<AddIn>();
            foreach (var addinTypesPerProject in addinTypesPerProjects)
            {
                var project = addinTypesPerProject.Project;
                var assemblyType = addinTypesPerProject.AssemblyType;

                var guid = GetAddInGuid(project, assemblyType);

                addIns.Add(new AddIn(
                    project.Name,
                    $"{rootProjectName}/{project.Name}.dll",
                    guid.ToString(),
                    assemblyType.FullName,
                    assemblyType.ToPluginType()));
            }

            var revitAddIns = new RevitAddIns
            {
                AddIn = addIns
            };

            var addInFile = Path.Combine(output, $"{rootProjectName}.addin");
            revitAddIns.ToXDocument().Save(addInFile);
        }

        /// <summary>
        /// Returns addin guid.
        /// </summary>
        /// <param name="project">Project for generate guid.</param>
        /// <param name="assemblyType"><see cref="AssemblyType"/>.</param>
        protected Guid GetAddInGuid(Project project, AssemblyType assemblyType)
        {
            var propertyName = assemblyType.ToPropertyName();
            var value = project.GetProperty(propertyName);
            if (!Guid.TryParse(value, out var guid))
            {
                throw new ArgumentException(
                    $"Property '{propertyName}' should contain valid guid value!");
            }

            return guid;
        }
    }
}