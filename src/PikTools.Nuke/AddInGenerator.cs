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
        public string GenerateAddInFile(
            Project project,
            IReadOnlyCollection<AssemblyType> addInTypes,
            string outputDirectory)
        {
            if (addInTypes.Any(x => x.BaseTypeName == ApplicationTypeName))
            {
                return GenerateAddIn(project, addInTypes, ApplicationTypeName, PluginType.Application, outputDirectory);
            }

            if (addInTypes.Any(x => x.BaseTypeName == CommandTypeName))
            {
                return GenerateAddIn(project, addInTypes, CommandTypeName, PluginType.Command, outputDirectory);
            }

            throw new ArgumentException(
                $"Project {project.Name} should contain any {CommandTypeName} " +
                $"or {ApplicationTypeName} type!");
        }

        private string GenerateAddIn(
            Project project,
            IReadOnlyCollection<AssemblyType> addinTypes,
            string typeName,
            PluginType pluginType,
            string output)
        {
            var addInId = project.GetProperty("AddInId");
            if (addInId == null)
            {
                throw new ArgumentException(
                    $"Project {project.Name} should contain 'AddIn' property with valid guid value!");
            }

            var pluginTypes = addinTypes.Where(x => x.BaseTypeName == typeName).ToList();

            var revitAddIns = new RevitAddIns()
            {
                AddIn = pluginTypes.Select(x => new AddIn(
                        project.Name,
                        $"{project.Name}/{project.Name}.dll",
                        addInId,
                        x.FullName,
                        pluginType))
                    .ToList()
            };

            var addInFile = Path.Combine(output, $"{project.Name}.addin");
            revitAddIns.ToXDocument().Save(addInFile);

            return addInFile;
        }
    }
}