namespace RxBim.Nuke.Generators
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;
    using global::Nuke.Common.ProjectModel;
    using Models;

    /// <summary>
    /// Генерирует PackageContents.xml
    /// </summary>
    public abstract class PackageContentsGenerator
    {
        /// <summary>
        /// Генерирует PackageContents.xml
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="outputDirectory">Путь к папке, куда нужно сохранить сгенерированный файл</param>
        public void Generate(Project project, string outputDirectory)
        {
            var outputFilePath = Path.Combine(outputDirectory, "PackageContents.xml");
            project
                .ToApplicationPackage(GetComponents(project).ToList())
                .ToXElement()
                .Save(outputFilePath);
        }

        /// <summary>
        /// Возвращает компоненты
        /// </summary>
        /// <param name="project">Проект</param>
        protected abstract IEnumerable<Components> GetComponents(Project project);
    }
}