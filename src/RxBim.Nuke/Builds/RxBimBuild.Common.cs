namespace RxBim.Nuke.Builds
{
    using System.Linq;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Tools.DotNet;
    using global::Nuke.Common.Utilities.Collections;
    using static global::Nuke.Common.IO.PathConstruction;
    using static global::Nuke.Common.Tools.DotNet.DotNetTasks;

    /// <content>
    /// Common build targets
    /// </content>
    public abstract partial class RxBimBuild<TWix, TPackGen, TPropGen>
    {
        /// <summary>
        /// Clean bin/, obj/ in solution
        /// </summary>
        public Target Clean => _ => _
            .Description("Clean bin/, obj/")
            .Executes(() =>
            {
                GlobDirectories(Solution.Directory, "**/bin", "**/obj")
                    .Where(x => !IsDescendantPath(BuildProjectDirectory, x))
                    .ForEach(FileSystemTasks.DeleteDirectory);
            });

        /// <summary>
        /// Restore packages
        /// </summary>
        public Target Restore => _ => _
            .Description("Restore packages")
            .DependsOn(Clean)
            .Executes(() =>
            {
                DotNetRestore(s => s
                    .SetProjectFile(Solution.Path));
            });

        /// <summary>
        /// Compile solution
        /// </summary>
        public Target Compile => _ => _
            .Description("Compile solution")
            .DependsOn(Restore)
            .Executes(() =>
            {
                DotNetBuild(settings => settings
                    .SetProjectFile(Solution.Path)
                    .SetConfiguration(Configuration));
            });

        /// <summary>
        /// Run tests
        /// </summary>
        public Target Test => _ => _
            .Description("Run tests")
            .Requires(() => Project)
            .Executes(() =>
            {
                DotNetTest(settings => settings
                    .SetProjectFile(GetProjectPath(Project)));
            });

        private AbsolutePath GetProjectPath(string name)
        {
            return Solution.AllProjects.FirstOrDefault(x => x.Name == name)?.Path ?? Solution.Path;
        }
    }
}