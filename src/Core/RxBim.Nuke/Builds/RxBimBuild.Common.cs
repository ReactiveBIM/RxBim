namespace RxBim.Nuke.Builds
{
    using System.Linq;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Tools.DotNet;
    using global::Nuke.Common.Utilities.Collections;
    using static global::Nuke.Common.Tools.DotNet.DotNetTasks;

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
                Solution.Directory.GlobDirectories("**/bin", "**/obj")
                    .Where(x => !BuildProjectDirectory.Contains(x))
                    .ForEach(x => x.DeleteDirectory());
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