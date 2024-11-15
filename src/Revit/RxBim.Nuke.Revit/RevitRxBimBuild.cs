namespace RxBim.Nuke.Revit
{
    using System;
    using System.IO;
    using System.Linq;
    using Builders;
    using Builds;
    using Extensions;
    using Generators;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using JetBrains.Annotations;
    using Models;
    using static Helpers.AssemblyScanner;

    /// <inheritdoc />
    [PublicAPI]
    public abstract class RevitRxBimBuild
        : RxBimBuild<RevitInstallerBuilder, RevitPackageContentsGenerator, RevitProjectPropertiesGenerator, OptionsBuilder>
    {
        /// <summary>
        /// Revit Version.
        /// </summary>
        [Parameter]
        public string RevitVersion { get; set; } = "2019";

        /// <summary>
        /// Copies an addin file to the manifests directory.
        /// </summary>
        public Target CopyDebugAddin => _ => _
            .Description("Copies an addin file to the manifests directory.")
            .Requires(() => Project)
            .Requires(() => RevitVersion)
            .DependsOn(Compile)
            .Executes(() =>
            {
                if (Project == nameof(Solution))
                {
                    throw new ArgumentException("Select a project!!!");
                }

                var project = Solution.AllProjects.FirstOrDefault(x => x.Name == Project);

                if (project == null)
                {
                    return;
                }

                var addinFile = $"{project.Name}.addin";
                var addinPath = project.Directory / addinFile;
                if (!addinPath.FileExists())
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
                    addinPath.Copy(revitPath, ExistsPolicy.FileOverwrite);
                }
            });

        /// <summary>
        /// Cleans the output directory.
        /// </summary>
        public Target CleanOutput => _ => _
            .Description("Cleans the output directory.")
            .Requires(() => Project)
            .Executes(() =>
            {
                var revitPath = GetRevitAddinsPath() / Project;
                if (Directory.Exists(revitPath))
                {
                    foreach (var file in Directory.EnumerateFiles(revitPath, "*", SearchOption.AllDirectories))
                    {
                        ((AbsolutePath)file).DeleteFile();
                    }
                }
            });

        /// <summary>
        /// Copies a plugin files to the Revit addins directory.
        /// </summary>
        public Target CopyOutput => _ => _
            .Description("Copies a plugin files to the Revit addins directory.")
            .Requires(() => Project)
            .DependsOn(CleanOutput, CopyDebugAddin)
            .Executes(() =>
            {
                var project = Solution.AllProjects.First(x => x.Name == Project);
                var outputPath = project.GetTargetDir();
                var revitPath = GetRevitAddinsPath() / Project;

                outputPath.CopyToDirectory(revitPath, ExistsPolicy.FileOverwrite | ExistsPolicy.DirectoryMerge);
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