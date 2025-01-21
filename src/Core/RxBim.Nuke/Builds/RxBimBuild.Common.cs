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
            .Executes(CleanInternal);

        /// <summary>
        /// Restores packages from a solution.
        /// </summary>
        public Target Restore => _ => _
            .Description("Restore packages")
            .DependsOn(Clean)
            .Executes(RestoreInternal);

        /// <summary>
        /// Compiles a solution.
        /// </summary>
        public Target Compile => _ => _
            .Description("Compile solution")
            .DependsOn(Restore)
            .Executes(CompileInternal);

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

        /// <inheritdoc cref="Compile"/>
        protected virtual void CompileInternal()
        {
            DotNetBuild(settings => settings
                .SetProjectFile(Solution.Path)
                .SetConfiguration(Configuration));
        }

        /// <inheritdoc cref="Restore"/>
        protected virtual void RestoreInternal()
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution.Path));
        }

        /// <inheritdoc cref="Clean"/>
        protected virtual void CleanInternal()
        {
            Solution.Directory.GlobDirectories("**/bin", "**/obj")
                .Where(x => !BuildProjectDirectory.Contains(x))
                .ForEach(x => x.DeleteDirectory());
        }

        private AbsolutePath? GetProjectPath(string? name)
        {
            return Solution.AllProjects.FirstOrDefault(x => x.Name == name)?.Path ?? Solution.Path;
        }
    }
}