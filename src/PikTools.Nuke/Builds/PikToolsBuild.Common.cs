#pragma warning disable SA1600, CS1591, SA1619
namespace PikTools.Nuke.Builds
{
    using System.Linq;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Tools.DotNet;
    using global::Nuke.Common.Utilities.Collections;
    using static global::Nuke.Common.IO.PathConstruction;

    /// <summary>
    /// Расширение Build-скрипта для сборки MSI. Targets общего назначения.
    /// </summary>
    public abstract partial class PikToolsBuild<TWix, TPackGen, TPropGen>
    {
        public Target Clean => _ => _
            .Description("Очищает решение")
            .Executes(() =>
            {
                GlobDirectories(Solution.Directory, "**/bin", "**/obj")
                    .Where(x => !IsDescendantPath(BuildProjectDirectory, x))
                    .ForEach(FileSystemTasks.DeleteDirectory);
            });

        public Target Restore => _ => _
            .Description("Восстанавливает пакеты")
            .DependsOn(Clean)
            .Executes(() =>
            {
                DotNetTasks.DotNetRestore(s => s
                    .SetProjectFile(Solution.Path));
            });

        public Target Compile => _ => _
            .Description("Собирает проект")
            .DependsOn(Restore)
            .Executes(() =>
            {
                DotNetTasks.DotNetBuild(settings => settings
                    .SetProjectFile(Solution.Path)
                    .SetConfiguration(Configuration));
            });

        public Target Test => _ => _
            .Description("Запускает тесты")
            .Requires(() => Project)
            .Executes(() =>
            {
                DotNetTasks.DotNetTest(settings => settings
                    .SetProjectFile(GetProjectPath(Project)));
            });

        private AbsolutePath GetProjectPath(string name)
        {
            return Solution.AllProjects.FirstOrDefault(x => x.Name == name)?.Path ?? Solution.Path;
        }
    }
}