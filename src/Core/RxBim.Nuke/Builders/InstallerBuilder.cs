namespace RxBim.Nuke.Builders
{
    extern alias nc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Extensions;
    using Generators;
    using InnoSetup.ScriptBuilder;
    using JetBrains.Annotations;
    using Models;
    using nc::Nuke.Common.IO;
    using nc::Nuke.Common.ProjectModel;
    using nc::Nuke.Common.Tooling;
    using nc::Nuke.Common.Tools.InnoSetup;

    /// <summary>
    /// Builder for installer.
    /// </summary>
    public class InstallerBuilder<TPackGen>
        where TPackGen : PackageContentsGenerator, new()
    {
        /// <summary>
        /// Builds MSI.
        /// </summary>
        /// <param name="project">Selected project.</param>
        /// <param name="outputBinDir">Output assemblies directory path.</param>
        /// <param name="options">Options.</param>
        public void BuildMsi(Project project, string outputBinDir, Options options)
        {
            if (!Directory.Exists(outputBinDir))
                return;

            const string toolPath = "rxbim.msi.builder";
            project.BuildMsiWithTool(toolPath, options);
        }

        /// <summary>
        /// Builds inno exe.
        /// </summary>
        /// <param name="project">Selected project.</param>
        /// <param name="temporaryDirectory">Temp directory.</param>
        /// <param name="outputDir">Output directory.</param>
        /// <param name="outputBinDir">Output assembly directory.</param>
        /// <param name="options">Options.</param>
        public void BuildInno(
            Project project,
            AbsolutePath temporaryDirectory,
            string outputDir,
            string outputBinDir,
            Options options)
        {
            var iss = temporaryDirectory / "package.iss";
            var setupFileName = $"{options.OutFileName}_{options.Version}";

            InnoBuilder
                .Create(
                    options,
                    (AbsolutePath)outputDir,
                    (AbsolutePath)outputBinDir,
                    setupFileName)
                .AddIcons()
                .AddFonts()
                .AddUninstallScript()
                .AddRxBimEnvironment(options.Environment ?? string.Empty)
                .Build(iss);

            var outDir = project.Solution.Directory / "out";
            InnoSetupTasks.InnoSetup(config => config
                .SetProcessToolPath(ToolPathResolver.GetPackageExecutable("Tools.InnoSetup", "ISCC.exe"))
                .SetScriptFile(iss)
                .SetOutputDir(outDir));
        }

        /// <summary>
        /// Generates additional files.
        /// </summary>
        /// <param name="rootProjectName">Root project name.</param>
        /// <param name="allProject">All projects.</param>
        /// <param name="allAssembliesTypes">All assemblies types.</param>
        /// <param name="outputDir">Output directory path.</param>
        public virtual void GenerateAdditionalFiles(
            string? rootProjectName,
            IEnumerable<Project> allProject,
            IEnumerable<AssemblyType> allAssembliesTypes,
            string outputDir)
        {
        }

        /// <summary>
        /// Generates package contents file.
        /// </summary>
        /// <param name="project">Selected project.</param>
        /// <param name="configuration">Selected configuration.</param>
        /// <param name="allAssembliesTypes">All assemblies types.</param>
        /// <param name="outputDir">Output directory path.</param>
        public void GeneratePackageContentsFile(
            Project project,
            string configuration,
            IEnumerable<AssemblyType> allAssembliesTypes,
            string outputDir)
        {
            if (!NeedGeneratePackageContents(configuration))
                return;

            var packageContentsGenerator = new TPackGen();
            packageContentsGenerator.Generate(project, outputDir, allAssembliesTypes);
        }

        /// <summary>
        /// Gets installation directory.
        /// </summary>
        /// <param name="project">Selected project.</param>
        /// <param name="configuration">Selected configuration.</param>
        public string GetInstallDir(
            Project project,
            string configuration)
        {
            return configuration switch
            {
                "Debug" => GetDebugInstallDir(project),
                "Release" => GetReleaseInstallDir(project),
                _ => throw new ArgumentException("Configuration not set!")
            };
        }

        /// <summary>
        /// Returns True, if need generate PackageContents.
        /// </summary>
        /// <param name="configuration">Selected configuration.</param>
        protected virtual bool NeedGeneratePackageContents(string configuration) => true;

        /// <summary>
        /// Returns action for modification <see cref="Options"/>.
        /// </summary>
        [UsedImplicitly]
        protected virtual Action<Options>? GetOptionsModification() => null;

        /// <summary>
        /// Gets Debug configuration install directory.
        /// </summary>
        /// <param name="project">Selected project.</param>
        protected virtual string GetDebugInstallDir(Project project)
        {
            return GetReleaseInstallDir(project);
        }

        /// <summary>
        /// Gets Release configuration install directory path.
        /// </summary>
        /// <param name="project">Selected project.</param>
        private string GetReleaseInstallDir(Project project)
        {
            return $"%AppDataFolder%/Autodesk/ApplicationPlugins/{project.Name}.bundle";
        }
    }
}