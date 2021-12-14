namespace RxBim.Nuke.Revit
{
    using System;
    using System.IO;
    using System.Linq;
    using Builds;
    using Extensions;
    using Generators;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using JetBrains.Annotations;
    using Models;
    using static global::Nuke.Common.IO.FileSystemTasks;
    using static Helpers.AssemblyScanner;

    /// <inheritdoc />
    [PublicAPI]
    public abstract partial class RevitRxBimBuild
        : RxBimBuild<RevitWixBuilder, RevitPackageContentsGenerator, RevitProjectPropertiesGenerator>
    {
        /// <summary>
        /// Revit Version
        /// </summary>
        [Parameter]
        public string RevitVersion { get; set; } = "2019";

        /// <summary>
        /// Копирует Addin файл в папку Revit
        /// </summary>
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

                var project = Solution.AllProjects.FirstOrDefault(x => x.Name == Project);

                if (project == null)
                {
                    return;
                }

                var addinFile = $"{project.Name}.addin";
                var addinPath = project.Directory / addinFile;
                if (!FileExists(addinPath))
                {
                    var dllPath = project.GetTargetPath();

                    new AddInGenerator().GenerateAddInFile(Project,
                        Scan(dllPath)
                            .Where(x => x.IsPluginType())
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

        /// <summary>
        /// Очищает выходную папку
        /// </summary>
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

        /// <summary>
        /// Копирует файл в выходную папку
        /// </summary>
        public Target CopyOutput => _ => _
            .Description("Копирует файл в выходную папку")
            .Requires(() => Project)
            .DependsOn(CleanOutput, CopyDebugAddin)
            .Executes(() =>
            {
                var project = Solution.AllProjects.FirstOrDefault(x => x.Name == Project);
                var outputPath = project.GetTargetDir();
                var revitPath = GetRevitAddinsPath() / Project;

                CopyDirectoryRecursively(outputPath,
                    revitPath,
                    DirectoryExistsPolicy.Merge,
                    FileExistsPolicy.Overwrite);
            });

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