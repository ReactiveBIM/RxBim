namespace RxBim.Nuke.Generators
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using global::Nuke.Common.ProjectModel;
    using Nuke.Extensions;
    using Nuke.Models;

    /// <summary>
    /// Генератор свойств проекта csproj
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
            var binPath = project.BuildProject(config);
            if (!File.Exists(binPath))
            {
                throw new InvalidOperationException($"File not found: {binPath}");
            }

            var pluginTypes = binPath.GetPluginTypes();
            var properties = GenerateProperties(project, pluginTypes);
            project.AddPropertiesToProject(properties);
        }

        /// <summary>
        /// Возвращает коллекцию XML объектов-свойств проекта с данными о типах для приложений и команд
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="pluginTypes">Типы приложений и команд</param>
        protected virtual IEnumerable<XElement> GenerateAdditionalProperties(
            Project project,
            IEnumerable<AssemblyType> pluginTypes)
        {
            return new List<XElement>();
        }

        private List<XElement> GenerateProperties(Project project, IEnumerable<AssemblyType> pluginTypes)
        {
            var properties = new List<XElement>();
            properties.AddRange(project.GenerateMsiProperties());
            properties.AddRange(GenerateAdditionalProperties(project, pluginTypes));
            return properties;
        }
    }
}