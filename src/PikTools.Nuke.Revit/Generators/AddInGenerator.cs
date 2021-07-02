namespace PikTools.Nuke.Revit.Generators
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using global::Nuke.Common.ProjectModel;
    using Nuke.Extensions;
    using Nuke.Models;
    using Revit.Generators.Extensions;
    using Revit.Generators.Models;
    using static Constants;

    /// <summary>
    /// Генератор манифест файлов для Revit
    /// </summary>
    public class AddInGenerator
    {
        /// <summary>
        /// Генерирует addin файл
        /// </summary>
        /// <param name="rootProjectName">Название основного проекта</param>
        /// <param name="addInTypesPerProjects">Типы для регистрации в Revit</param>
        /// <param name="outputDirectory">папка для сохранения addin файла</param>
        public void GenerateAddInFile(
            string rootProjectName,
            IReadOnlyList<ProjectWithAssemblyType> addInTypesPerProjects,
            string outputDirectory)
        {
            var pluginTypes = addInTypesPerProjects
                .Where(x => x.AssemblyType.IsPluginType())
                .ToList();

            if (!addInTypesPerProjects.Any())
            {
                throw new ArgumentException(
                    $"Project {rootProjectName} should contain any {PikToolsCommand} or {PikToolsApplication} type!");
            }

            GenerateAddIn(rootProjectName, pluginTypes, outputDirectory);
        }

        private void GenerateAddIn(
            string rootProjectName,
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
                    assemblyType.BaseTypeName.ToPluginType()));
            }

            var revitAddIns = new RevitAddIns
            {
                AddIn = addIns
            };

            var addInFile = Path.Combine(output, $"{rootProjectName}.addin");
            revitAddIns.ToXDocument().Save(addInFile);
        }

        private Guid GetAddInGuid(Project project, AssemblyType assemblyType)
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