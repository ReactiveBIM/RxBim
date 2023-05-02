namespace RxBim.Nuke.Builders
{
    extern alias nc;
    using System;
    using System.Collections.Generic;
    using Generators;
    using JetBrains.Annotations;
    using Models;
    using nc::Nuke.Common.ProjectModel;

    /// <summary>
    /// Builder for installer.
    /// </summary>
    public class InstallerBuilder<TPackGen, TOptsBuilder>
        where TPackGen : PackageContentsGenerator, new()
        where TOptsBuilder : OptionsBuilder, new()
    {
        private Options? _options;

        /// <summary>
        /// Gets build MSI options.
        /// </summary>
        /// <param name="project">Selected Project.</param>
        /// <param name="outputDir">Output directory path.</param>
        /// <param name="configuration">Selected configuration.</param>
        /// <param name="environment">Environment variable.</param>
        /// <param name="timestampRevisionVersion">Add timestamp revision version.</param>
        public Options GetBuildOptions(
            Project project,
            string outputDir,
            string configuration,
            string environment,
            bool timestampRevisionVersion)
        {
            var optionsBuilder = new TOptsBuilder();
            optionsBuilder
                .AddDefaultSettings(project)
                .AddDirectorySettings(GetInstallDir(project, configuration), outputDir)
                .AddProductVersion(project, configuration)
                .AddEnvironment(environment)
                .AddVersion(project);

            if (timestampRevisionVersion)
                optionsBuilder.AddTimestampRevisionVersion();

            return _options ??= optionsBuilder.Build(GetOptionsModification());
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
        /// Gets installation directory.
        /// </summary>
        /// <param name="project">Selected project.</param>
        /// <param name="configuration">Selected configuration.</param>
        private string GetInstallDir(
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
        /// Gets Release configuration install directory path.
        /// </summary>
        /// <param name="project">Selected project.</param>
        private string GetReleaseInstallDir(Project project)
        {
            return $"%AppDataFolder%/Autodesk/ApplicationPlugins/{project.Name}.bundle";
        }
    }
}