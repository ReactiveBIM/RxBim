namespace RxBim.Nuke.Models
{
    extern alias NukeCommon;
    using NukeCommon::Nuke.Common.ProjectModel;

    /// <summary>
    /// Pair of the <see cref="Project"/> and the <see cref="AssemblyType"/>.
    /// </summary>
    public class ProjectWithAssemblyType
    {
        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="project">The <see cref="Project"/>.</param>
        /// <param name="assemblyType">The <see cref="AssemblyType"/>.</param>
        public ProjectWithAssemblyType(
            Project project,
            AssemblyType assemblyType)
        {
            Project = project;
            AssemblyType = assemblyType;
        }

        /// <summary>
        /// The project.
        /// </summary>
        public Project Project { get; }

        /// <summary>
        /// The assembly type.
        /// </summary>
        public AssemblyType AssemblyType { get; }
    }
}