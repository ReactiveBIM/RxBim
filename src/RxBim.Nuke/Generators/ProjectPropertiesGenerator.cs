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
    /// Project properties generator.
    /// </summary>
    public class ProjectPropertiesGenerator
    {
        /// <summary>
        /// Generates a project properties.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="config">A configuration.</param>
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
        /// Generates additional projects properties as <see cref="XElement"/> collection.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="pluginTypes">The <see cref="AssemblyType"/> collection.</param>
        protected virtual IEnumerable<XElement> GenerateAdditionalProperties(
            Project project,
            IEnumerable<AssemblyType> pluginTypes)
        {
            return new List<XElement>();
        }

        private List<XElement> GenerateProperties(Project project, IEnumerable<AssemblyType> pluginTypes)
        {
            var properties = new List<XElement>();
            properties.AddRange(project.GenerateInstallationProperties());
            properties.AddRange(GenerateAdditionalProperties(project, pluginTypes));
            return properties;
        }
    }
}