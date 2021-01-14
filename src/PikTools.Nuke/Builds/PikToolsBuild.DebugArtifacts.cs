#pragma warning disable SA1600, CS1591
namespace PikTools.Nuke.Builds
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Application.Api;
    using Command.Api;
    using Generators;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Utilities;
    using static global::Nuke.Common.IO.FileSystemTasks;

    public abstract partial class PikToolsBuild
    {
        public Target CopyDebugAddin => _ => _
            .Description("Копирует Addin файл в папку Revit")
            .Requires(() => Project)
            .Requires(() => RevitVersion)
            .DependsOn(Compile)
            .Executes(() =>
            {
                if (Project == nameof(Solution))
                {
                    throw new ArgumentException("Выберите проект!!!");
                }

                var addinFile = $"{Project}.addin";
                var addinPath = Solution.Directory / "examples" / Project / addinFile;
                if (!FileExists(addinPath))
                {
                    var project = Solution.AllProjects.FirstOrDefault(x => x.Name == Project);

                    var targetFramework = GetTargetFramework(project);

                    var dllPath = project.Directory / "bin" / Configuration / targetFramework /
                                  $"{project.GetProperty<string>("AssemblyName")}.dll";

                    new AddInGenerator().GenerateAddInFile(Project,
                        new AssemblyScanner()
                            .Scan(dllPath)
                            .Where(x => x.BaseTypeName == nameof(PikToolsCommand) ||
                                        x.BaseTypeName == nameof(PikToolsApplication))
                            .Select(x => new ProjectWithAssemblyType(project, x))
                            .ToArray(),
                        GetRevitAddinsPath());
                }
                else
                {
                    var revitPath = GetRevitAddinsPath() / addinFile;
                    CopyFile(addinPath, revitPath, FileExistsPolicy.Overwrite);
                }
            });

        public Target CleanOutput => _ => _
            .Description("Очищает выходную папку")
            .Requires(() => Project)
            .Executes(() =>
            {
                var revitPath = GetRevitAddinsPath() / Project;
                if (Directory.Exists(revitPath))
                {
                    foreach (var file in Directory.EnumerateFiles(revitPath, "*", SearchOption.AllDirectories))
                    {
                        DeleteFile(file);
                    }
                }
            });

        public Target CopyOutput => _ => _
            .Description("Копирует файл в выходную папку")
            .Requires(() => Project)
            .DependsOn(CleanOutput, CopyDebugAddin)
            .Executes(() =>
            {
                var project = Solution.AllProjects.FirstOrDefault(x => x.Name == Project);
                var tfw = GetTargetFramework(project);
                var outputPath = Solution.Directory / "examples" / Project / "bin" / Configuration / tfw;
                var revitPath = GetRevitAddinsPath() / Project;

                CopyDirectoryRecursively(
                    outputPath,
                    revitPath,
                    DirectoryExistsPolicy.Merge,
                    FileExistsPolicy.Overwrite);
            });

        private string GetTargetFramework(Project project)
        {
            var frameworks = project.GetTargetFrameworks();

            var tfw = frameworks.Count > 1
                ? ConsoleUtility.PromptForChoice("Select framework:", frameworks.Select(x => (x, x)).ToArray())
                : frameworks.First();
            return tfw;
        }

        private AbsolutePath GetRevitAddinsPath()
        {
            var revitPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Autodesk",
                "Revit",
                "Addins",
                RevitVersion);
            return (AbsolutePath)revitPath;
        }
    }
}