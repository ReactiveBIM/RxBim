namespace PikTools.Nuke
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using global::Nuke.Common.ProjectModel;

    public class AddInGenerator
    {
        private string _commandTypeName = "PikToolsCommand";
        private string _applicationTypeName = "PikToolsApplication";

        public string GenerateAddInFile(Project project, IReadOnlyCollection<AssemblyType> addInTypes, string output)
        {
            if (addInTypes.Any(x => x.BaseTypeName == _applicationTypeName))
            {
                return GenerateAddIn(project, addInTypes, _applicationTypeName, PluginType.Application, output);
            }

            if (addInTypes.Any(x => x.BaseTypeName == _commandTypeName))
            {
                return GenerateAddIn(project, addInTypes, _commandTypeName, PluginType.Command, output);
            }

            throw new ArgumentException(
                $"Project {project.Name} should contain any {_commandTypeName} " +
                $"or {_applicationTypeName} type!");
        }

        private string GenerateAddIn(
            Project project,
            IReadOnlyCollection<AssemblyType> addinTypes,
            string typeName,
            PluginType pluginType,
            string output)
        {
            var addInId = project.GetProperty("AddInId");
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