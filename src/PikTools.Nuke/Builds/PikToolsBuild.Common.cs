namespace PikTools.Nuke.Builds
{
    using System.Linq;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Tools.DotNet;
    using global::Nuke.Common.Utilities.Collections;
    using static global::Nuke.Common.IO.PathConstruction;
    using static global::Nuke.Common.Tools.DotNet.DotNetTasks;

    /// <content>
    /// Расширение Build-скрипта для сборки MSI. Targets общего назначения.
    /// </content>
    public abstract partial class PikToolsBuild<TWix, TPackGen, TPropGen>
    {
        /// <summary>
        /// Очишает bin/, obj/ в решении.
        /// </summary>
        public Target Clean => _ => _
            .Description("Очишает bin/, obj/")
            .Executes(() =>
            {
                GlobDirectories(Solution.Directory, "**/bin", "**/obj")
                    .Where(x => !IsDescendantPath(BuildProjectDirectory, x))
                    .ForEach(FileSystemTasks.DeleteDirectory);
            });

        /// <summary>
        /// Восстанавливает пакеты.
        /// </summary>
        public Target Restore => _ => _
            .Description("Восстанавливает пакеты")
            .DependsOn(Clean)
            .Executes(() =>
            {
                DotNetRestore(s => s
                    .SetProjectFile(Solution.Path));
            });

        /// <summary>
        /// Собирает проект
        /// </summary>
        public Target Compile => _ => _
            .Description("Собирает решение")
            .DependsOn(Restore)
            .Executes(() =>
            {
                DotNetBuild(settings => settings
                    .SetProjectFile(Solution.Path)
                    .SetConfiguration(Configuration));
            });

        /// <summary>
        /// Запускает тесты
        /// </summary>
        public Target Test => _ => _
            .Description("Запускает тесты")
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