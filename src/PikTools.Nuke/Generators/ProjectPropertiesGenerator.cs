namespace PikTools.Nuke.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Application.Api;
    using Command.Api;
    using Extensions;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Tools.DotNet;
    using global::Nuke.Common.Tools.Git;
    using global::Nuke.Common.Utilities;
    using static global::Nuke.Common.Tools.DotNet.DotNetTasks;

    /// <summary>
    /// Генератор свойств проектв csproj
    /// </summary>
    public class ProjectPropertiesGenerator
    {
        /// <summary>
        /// Генерирует свойства проекта
        /// </summary>
        /// <param name="project">проект</param>
        /// <param name="config">конфигурация</param>
        public void GenerateProperties(Project project, string config)
        {
            var binPath = BuildProject(project, config);
            var assemblyTypes = GetPluginTypes(binPath);
            var properties = GenerateProperties(project, assemblyTypes);
            ModifyProject(project, properties);
        }

        private List<XElement> GenerateProperties(Project project, List<AssemblyType> assemblyTypes)
        {
            var properties = new List<XElement>();
            properties.AddRange(GenerateAddInProperties(project, assemblyTypes));
            properties.AddRange(GenerateMsiProperties(project));
            return properties;
        }

        private void ModifyProject(Project project, IReadOnlyCollection<XElement> properties)
        {
            if (properties.Any())
            {
                var projectXml = XElement.Load(project.Path);

                projectXml.Add(new XElement("PropertyGroup", properties));
                projectXml.Save(project.Path);

                Logger.Info(
                    $"Properties {properties.Select(x => x.Name.ToString()).ToList().JoinComma()} for {project.Name} project generated\"");

                CommitChanges(project);
            }
        }

        private void CommitChanges(Project project)
        {
            var commit = ConsoleUtility.PromptForChoice("Commit changes?", ("Yes", "Yes"), ("No", "No"));

            if (commit switch
            {
                "Yes" => true,
                "No" => false,
                _ => false
            })
            {
                GitTasks.Git($"add \"{project.Path}\"", project.Solution.Directory);
                GitTasks.Git($"commit -m \"Generated Revit AddIn properties for {project.Name} project\"",
                    project.Solution.Directory);
            }
        }

        private IEnumerable<XElement> GenerateMsiProperties(Project project)
        {
            if (project.GetProperty(nameof(MsiBuilder.Options.PackageGuid)) == null)
            {
                yield return new XElement(nameof(MsiBuilder.Options.PackageGuid), Guid.NewGuid());
            }

            if (project.GetProperty(nameof(MsiBuilder.Options.UpgradeCode)) == null)
            {
                yield return new XElement(nameof(MsiBuilder.Options.UpgradeCode), Guid.NewGuid());
            }
        }

        private IEnumerable<XElement> GenerateAddInProperties(
            Project project,
            List<AssemblyType> assemblyTypes)
        {
            foreach (var type in assemblyTypes)
            {
                var propertyName = $"{type.BaseTypeName.ToPluginType()}__{type.FullName.ToPropertyName()}";
                var property = project.GetProperty(propertyName);
                if (property == null)
                {
                    yield return new XElement(propertyName, Guid.NewGuid());
                }
            }
        }

        private List<AssemblyType> GetPluginTypes(AbsolutePath binPath)
        {
            var assemblyScanner = new AssemblyScanner();
            var assemblyTypes = assemblyScanner.Scan(binPath)
                .Where(x => x.BaseTypeName == nameof(PikToolsCommand) ||
                            x.BaseTypeName == nameof(PikToolsApplication))
                .ToList();
            return assemblyTypes;
        }

        private AbsolutePath BuildProject(Project project, string config)
        {
            DotNetBuild(settings => settings
                .SetConfiguration(config)
                .SetProjectFile(project));

            var binPath = project.Directory /
                          "bin" /
                          config /
                          project.GetProperty("TargetFramework") /
                          $"{project.Name}.dll";
            return binPath;
        }
    }
}