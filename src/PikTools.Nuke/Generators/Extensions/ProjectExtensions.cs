namespace PikTools.Nuke.Generators.Extensions
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using global::Nuke.Common.ProjectModel;
    using Models;

    /// <summary>
    /// Расширения для проектов Nuke
    /// </summary>
    public static class ProjectExtensions
    {
        /// <summary>
        /// Маппит <see cref="Project"/> в <see cref="ApplicationPackage"/>
        /// </summary>
        /// <param name="project">проект</param>
        public static ApplicationPackage ToApplicationPackage(this Project project)
        {
            var revitVersions = Enumerable.Range(2017, 4);
            return new ApplicationPackage
            {
                Description = project.GetProperty(nameof(ApplicationPackage.Description)) ?? string.Empty,
                Name = project.Name,
                AppVersion = project.GetProperty("Version"),
                FriendlyVersion = project.GetProperty("Version"),
                ProductCode = project.GetProperty("PackageGuid"),
                UpgradeCode = project.GetProperty("UpgradeCode"),
                Components = revitVersions
                    .Select(revitVersion => new Components
                    {
                        Description = $"Revit {revitVersion} part",
                        Platform = "Revit",
                        ModuleName = $"{project.Name}.addin",
                        OS = "Win64",
                        SeriesMax = $"R{revitVersion}",
                        SeriesMin = $"R{revitVersion}",
                    })
                    .ToList()
            };
        }

        /// <summary>
        /// Маппит <see cref="ApplicationPackage"/>  в <see cref="XElement"/>
        /// </summary>
        /// <param name="package">ApplicationPackage</param>
        public static XElement ToXElement(this ApplicationPackage package)
        {
            return new XElement(nameof(ApplicationPackage),
                new XAttribute(nameof(ApplicationPackage.Description), package.Description),
                new XAttribute(nameof(ApplicationPackage.SchemaVersion), package.SchemaVersion),
                new XAttribute(nameof(ApplicationPackage.Name), package.Name),
                new XAttribute(nameof(ApplicationPackage.AppVersion), package.AppVersion),
                new XAttribute(nameof(ApplicationPackage.FriendlyVersion), package.FriendlyVersion),
                new XAttribute(nameof(ApplicationPackage.ProductType), package.ProductType),
                new XAttribute(nameof(ApplicationPackage.ProductCode), $"{{{package.ProductCode}}}"),
                new XAttribute(nameof(ApplicationPackage.UpgradeCode), $"{{{package.UpgradeCode}}}"),
                package.Components.Select(x => x.ToXElement()));
        }

        /// <summary>
        /// Маппит <see cref="Components"/>  в <see cref="XElement"/>
        /// </summary>
        /// <param name="components">Components</param>
        public static XElement ToXElement(this Components components)
        {
            return new XElement(nameof(Components),
                new XAttribute(nameof(Components.Description), components.Description),
                new XElement("RuntimeRequirements",
                    new XAttribute(nameof(Components.OS), components.OS),
                    new XAttribute(nameof(Components.Platform), components.Platform),
                    new XAttribute(nameof(Components.SeriesMax), components.SeriesMax),
                    new XAttribute(nameof(Components.SeriesMin), components.SeriesMin)),
                new XElement("ComponentEntry",
                    new XAttribute(nameof(Components.ModuleName), components.ModuleName)));
        }
    }
}