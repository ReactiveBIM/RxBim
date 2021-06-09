namespace PikTools.Nuke.Generators.Extensions
{
    using System.Collections.Generic;
    using global::Nuke.Common.ProjectModel;
    using Models;

    /// <summary>
    /// Расширения для проекта
    /// </summary>
    public static class ProjectExtensions
    {
        /// <summary>
        /// Маппит <see cref="Project"/> в <see cref="ApplicationPackage"/>
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="components">Компоненты</param>
        public static ApplicationPackage ToApplicationPackage(this Project project, List<Components> components)
        {
            return new ApplicationPackage
            {
                Description = project.GetProperty(nameof(ApplicationPackage.Description)) ?? string.Empty,
                Name = project.Name,
                AppVersion = project.GetProperty("Version"),
                FriendlyVersion = project.GetProperty("Version"),
                ProductCode = project.GetProperty("PackageGuid"),
                UpgradeCode = project.GetProperty("UpgradeCode"),
                Components = components
            };
        }
    }
}