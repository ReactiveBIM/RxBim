namespace RxBim.Nuke.Revit.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Extensions;
    using global::Nuke.Common.ProjectModel;
    using RxBim.Nuke.Generators;
    using RxBim.Nuke.Models;

    /// <inheritdoc />
    public class RevitProjectPropertiesGenerator : ProjectPropertiesGenerator
    {
        /// <inheritdoc />
        protected override IEnumerable<XElement> GenerateAdditionalProperties(
            Project project,
            IEnumerable<AssemblyType> pluginTypes)
        {
            foreach (var type in pluginTypes)
            {
                var propertyName = type.ToPropertyName();
                var property = project.GetProperty(propertyName);
                if (property == null)
                {
                    yield return new XElement(propertyName, Guid.NewGuid());
                }
            }
        }
    }
}