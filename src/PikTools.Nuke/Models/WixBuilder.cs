namespace PikTools.Nuke.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Extensions;
    using Generators;
    using global::Nuke.Common.ProjectModel;
    using MsiBuilder;
    using static Constants;
    using static Helpers.WixHelper;

    /// <summary>
    /// Wix extension
    /// </summary>
    public class WixBuilder<T>
        where T : PackageContentsGenerator, new()
    {
        private string _installDir;
        private Options _options;
        private List<AssemblyType> _types;

        /// <summary>
        /// Build MSI
        /// </summary>
        /// <param name="project">Selected project</param>
        /// <param name="configuration">Selected configuration</param>
        /// <param name="outputDir">Output directory</param>
        /// <param name="outputBinDir">Output assemblies directory</param>
        public void BuildMsi(
            Project project,
            string configuration,
            string outputDir,
            string outputBinDir)
        {
            if (!Directory.Exists(outputBinDir))
                return;

            var options = GetBuildMsiOptions(project, outputDir, configuration);
            var toolPath = GetMsiBuilderToolPath();

            project.BuildMsiWithTool(toolPath, options);
        }

        /// <summary>
        /// Get installation directory
        /// </summary>
        /// <param name="project">Selected Project</param>
        /// <param name="configuration">Selected configuration</param>
        public string GetInstallDir(
            Project project,
            string configuration)
        {
            return _installDir ??= configuration switch
            {
                Debug => GetDebugInstallDir(project),
                Release => GetReleaseInstallDir(project),
                _ => throw new ArgumentException("Configuration not set!")
            };
        }

        /// <summary>
        /// Get build MSI options
        /// </summary>
        /// <param name="project">Selected Project</param>
        /// <param name="outputDir">Output directory</param>
        /// <param name="configuration">Selected configuration</param>
        public Options GetBuildMsiOptions(
            Project project,
            string outputDir,
            string configuration)
        {
            var installDir = GetInstallDir(project, configuration);
            return _options ??= project.GetBuildMsiOptions(
                installDir, outputDir, configuration);
        }

        /// <summary>
        /// Get assembly types
        /// </summary>
        /// <param name="project">Selected Project</param>
        /// <param name="outputBinDir">Output assembly directory</param>
        /// <param name="outputDir">Output directory</param>
        /// <param name="configuration">Selected configuration</param>
        public List<AssemblyType> GetAssemblyTypes(
            Project project,
            string outputBinDir,
            string outputDir,
            string configuration)
        {
            var options = GetBuildMsiOptions(project, outputDir, configuration);
            return _types ??= project.GetAssemblyTypes(outputBinDir, options);
        }

        /// <summary>
        /// Generate additional files
        /// </summary>
        /// <param name="rootProjectName">Root project name</param>
        /// <param name="allProject">All projects</param>
        /// <param name="addInTypes">Assembly types</param>
        /// <param name="outputDir">Output directory</param>
        public virtual void GenerateAdditionalFiles(
            string rootProjectName,
            IEnumerable<Project> allProject,
            IEnumerable<AssemblyType> addInTypes,
            string outputDir)
        {
        }

        /// <summary>
        /// Generate package contents file
        /// </summary>
        /// <param name="project">Selected project</param>
        /// <param name="configuration">Selected configuration</param>
        /// <param name="outputDir">Output directory</param>
        public void GeneratePackageContentsFile(
            Project project,
            string configuration,
            string outputDir)
        {
            if (!NeedGeneratePackageContents(configuration))
                return;

            var packageContentsGenerator = new T();
            packageContentsGenerator.Generate(project, outputDir);
        }

        /// <summary>
        /// Return True, if need generate PackageContents
        /// </summary>
        /// <param name="configuration">Selected configuration</param>
        protected virtual bool NeedGeneratePackageContents(string configuration) => true;

        /// <summary>
        /// Get Debug configuration install directory
        /// </summary>
        /// <param name="project">Selected project</param>
        protected virtual string GetDebugInstallDir(Project project)
        {
            return GetReleaseInstallDir(project);
        }

        /// <summary>
        /// Get Release configuration install directory
        /// </summary>
        /// <param name="project">Selected project</param>
        private string GetReleaseInstallDir(Project project)
        {
            return $"%AppDataFolder%/Autodesk/ApplicationPlugins/{project.Name}.bundle";
        }
    }
}