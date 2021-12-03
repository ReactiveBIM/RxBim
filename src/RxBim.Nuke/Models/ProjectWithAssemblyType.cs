namespace RxBim.Nuke.Models
{
    using global::Nuke.Common.ProjectModel;

    /// <summary>
    /// Pair of <see cref="Project"/> and <see cref="AssemblyType"/>
    /// </summary>
    public class ProjectWithAssemblyType
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="project"><see cref="Project"/></param>
        /// <param name="assemblyType"><see cref="AssemblyType"/></param>
        public ProjectWithAssemblyType(
            Project project,
            AssemblyType assemblyType)
        {
            Project = project;
            AssemblyType = assemblyType;
        }

        /// <summary>
        /// Project
        /// </summary>
        public Project Project { get; }

        /// <summary>
        /// Assembly type
        /// </summary>
        public AssemblyType AssemblyType { get; }
    }
}