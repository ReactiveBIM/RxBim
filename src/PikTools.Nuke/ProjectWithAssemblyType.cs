namespace PikTools.Nuke
{
    using global::Nuke.Common.ProjectModel;

    /// <summary>
    /// Проект и тип сборки
    /// </summary>
    public class ProjectWithAssemblyType
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="assemblyType">Тип сборки</param>
        public ProjectWithAssemblyType(
            Project project,
            AssemblyType assemblyType)
        {
            Project = project;
            AssemblyType = assemblyType;
        }

        /// <summary>
        /// Проект
        /// </summary>
        public Project Project { get; }

        /// <summary>
        /// Тип сборки
        /// </summary>
        public AssemblyType AssemblyType { get; }
    }
}
