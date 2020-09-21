namespace PikTools.Nuke
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Application.Api;
    using Command.Api;
    using global::Nuke.Common.ProjectModel;

    /// <summary>
    /// Генератор манифест файлов для Revit
    /// </summary>
    public class AddInGenerator
    {
        private const string CommandTypeName = nameof(PikToolsCommand);
        private const string ApplicationTypeName = nameof(PikToolsApplication);

        /// <summary>
        /// Генерирует addin файл
        /// </summary>
        /// <param name="project">проект</param>
        /// <param name="addInTypes">Типы для регистрации в Revit</param>
        /// <param name="outputDirectory">папка для сохранения addin файла</param>
        public void GenerateAddInFile(
            Project project,
            IReadOnlyCollection<AssemblyType> addInTypes,
            string outputDirectory)
        {
            var pluginTypes = addInTypes.Where(x => x.BaseTypeName == nameof(PikToolsCommand) ||
                                                    x.BaseTypeName == nameof(PikToolsApplication))
                .ToList();

            if (!addInTypes.Any())
            {
                throw new ArgumentException(
                    $"Project {project.Name} should contain any {CommandTypeName} " +
                    $"or {ApplicationTypeName} type!");
            }

            GenerateAddIn(project, pluginTypes, outputDirectory);
        }

        private void GenerateAddIn(
            Project project,
            IReadOnlyCollection<AssemblyType> addinTypes,
            string output)
        {
            var addIns = new List<AddIn>();
            foreach (var assemblyType in addinTypes)
            {
                var guid = GetAddInGuid(project, assemblyType);

                addIns.Add(new AddIn(
                    project.Name,
                    $"{project.Name}/{project.Name}.dll",
                    guid.ToString(),
                    assemblyType.FullName,
                    assemblyType.BaseTypeName.ToPluginType()));
            }

            var revitAddIns = new RevitAddIns()
            {
                AddIn = addIns
            };

            var addInFile = Path.Combine(output, $"{project.Name}.addin");
            revitAddIns.ToXDocument().Save(addInFile);
        }

        private Guid GetAddInGuid(Project project, AssemblyType assemblyType)
        {
            var propertyName = $"{assemblyType.BaseTypeName.ToPluginType()}__{assemblyType.FullName.ToPropertyName()}";
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