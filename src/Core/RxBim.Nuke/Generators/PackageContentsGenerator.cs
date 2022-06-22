namespace RxBim.Nuke.Generators
{
    extern alias NukeCommon;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensions;
    using Models;
    using NukeCommon::Nuke.Common.ProjectModel;

    /// <summary>
    /// Generator of PackageContents.xml.
    /// </summary>
    public abstract class PackageContentsGenerator
    {
        /// <summary>
        /// Generates PackageContents.xml file.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="outputDirectory">The output path.</param>
        public void Generate(Project project, string outputDirectory)
        {
            var outputFilePath = Path.Combine(outputDirectory, "PackageContents.xml");
            project
                .ToApplicationPackage(GetComponents(project).ToList())
                .ToXElement()
                .Save(outputFilePath);
        }

        /// <summary>
        /// Gets <see cref="Components"/> collection.
        /// </summary>
        /// <param name="project">The project.</param>
        protected abstract IEnumerable<Components> GetComponents(Project project);
    }
}