namespace RxBim.Nuke.Builds
{
    extern alias nc;
    using System.Linq;
    using nc::Nuke.Common;
    using nc::Nuke.Common.IO;
    using nc::Nuke.Common.Tools.DotNet;
    using nc::Nuke.Common.Utilities.Collections;
    using static nc::Nuke.Common.IO.PathConstruction;
    using static nc::Nuke.Common.Tools.DotNet.DotNetTasks;

    /// <content>
    /// Common build targets.
    /// </content>
    public abstract partial class RxBimBuild<TBuilder, TPackGen, TPropGen, TOptsBuilder>
    {
        /// <summary>
        /// Cleans bin/, obj/ directories in solution.
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
        /// Restores packages from a solution.
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
        /// Compiles a solution.
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
        /// Runs tests from a solution.
        /// </summary>
        public Target Test => _ => _
            .Description("Run tests")
            .Requires(() => Project)
            .Executes(() =>
            {
                DotNetTest(settings => settings
                    .SetProjectFile(GetProjectPath(Project)));
            });

        private AbsolutePath? GetProjectPath(string? name)
        {
            return Solution.AllProjects.FirstOrDefault(x => x.Name == name)?.Path ?? Solution.Path;
        }
    }
}