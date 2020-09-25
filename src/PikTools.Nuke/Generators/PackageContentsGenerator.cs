namespace PikTools.Nuke.Generators
{
    using System.IO;
    using Extensions;
    using global::Nuke.Common.ProjectModel;

    /// <summary>
    /// Генерирует PackageContents.xml
    /// </summary>
    public class PackageContentsGenerator
    {
        /// <summary>
        /// Генерирует PackageContents.xml
        /// </summary>
        /// <param name="project">проект</param>
        /// <param name="outputDirectory">путь к файлу</param>
        public void Generate(Project project, string outputDirectory)
        {
            var outputFilePath = Path.Combine(outputDirectory, "PackageContents.xml");
            project
                .ToApplicationPackage()
                .ToXElement()
                .Save(outputFilePath);
        }
    }
}