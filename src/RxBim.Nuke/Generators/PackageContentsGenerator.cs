namespace RxBim.Nuke.Generators
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;
    using global::Nuke.Common.ProjectModel;
    using Models;

    /// <summary>
    /// Generator of PackageContents.xml
    /// </summary>
    public abstract class PackageContentsGenerator
    {
        /// <summary>
        /// Generates PackageContents.xml
        /// </summary>
        /// <param name="project">Project</param>
        /// <param name="outputDirectory">Output path</param>
        public void Generate(Project project, string outputDirectory)
        {
            var outputFilePath = Path.Combine(outputDirectory, "PackageContents.xml");
            project
                .ToApplicationPackage(GetComponents(project).ToList())
                .ToXElement()
                .Save(outputFilePath);
        }

        /// <summary>
        /// Gets <see cref="Components"/> collection
        /// </summary>
        /// <param name="project">Project</param>
        protected abstract IEnumerable<Components> GetComponents(Project project);
    }
}