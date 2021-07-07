#pragma warning disable
namespace PikTools.Nuke.Builds
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Generators;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Tools.DotNet;
    using global::Nuke.Common.Tools.Git;
    using JetBrains.Annotations;
    using Models;
    using static global::Nuke.Common.Tools.DotNet.DotNetTasks;
    using static global::Nuke.Common.IO.FileSystemTasks;

    /// <summary>
    /// Расширение Build-скрипта для сборки MSI
    /// </summary>
    /// <typeparam name="TWix">Тип WIX-сборщика</typeparam>
    /// <typeparam name="TPackGen">Тип генератора файла PackageContents</typeparam>
    /// <typeparam name="TPropGen">Тип генератора свойств проекта</typeparam>
    [PublicAPI]
    public abstract partial class PikToolsBuild<TWix, TPackGen, TPropGen> : NukeBuild
        where TWix : WixBuilder<TPackGen>, new()
        where TPackGen : PackageContentsGenerator, new()
        where TPropGen : ProjectPropertiesGenerator, new()
    {
        /// <summary>
        /// ctor
        /// </summary>
        protected PikToolsBuild()
        {
            _wix = new TWix();
        }

        /// <summary>
        /// BuildMsi
        /// </summary>
        public Target BuildMsi => _ => _
            .Description("Собирает MSi пакет из указанного проекта")
            .Requires(() => Project)
            .Requires(() => Configuration)
            .Executes(() =>
            {
                CreateOutDirectory();

                BuildInstaller(ProjectForMsiBuild,
                    Configuration,
                    (AbsolutePath)Cert,
                    Password,
                    Algorithm,
                    ServerUrl);
            });

        /// <summary>
        /// Собирает msi для тестирования
        /// </summary>
        public Target BuildMsiForTesting => _ => _
            .Requires(() => Project)
            .DependsOn(CheckStageVersion)
            .Executes(() => { Configuration = Configuration.Debug; })
            .Triggers(BuildMsi);

        /// <summary>
        /// Собирает msi для тестирования
        /// </summary>
        public Target BuildMsiForProduction => _ => _
            .Requires(() => Project)
            .DependsOn(CheckProductionVersion)
            .Executes(() => { Configuration = Configuration.Release; })
            .Triggers(BuildMsi);

        /// <summary>
        /// Собирает проект из тэга Testing{ProjectName}
        /// </summary>
        public Target BuildFromTag => _ => _
            .Executes(() =>
            {
                CreateOutDirectory();

                var regex = new Regex("Testing(?<projectName>.*)");
                var projectsForBuild = GitTasks.Git("tag --points-at HEAD")
                    .Select(x => x.Text)
                    .Where(x => regex.IsMatch(x))
                    .Select(x => regex.Match(x).Groups["projectName"].Value);

                foreach (var projectName in projectsForBuild)
                {
                    var project = Solution.AllProjects.Single(x => x.Name == projectName);
                    BuildInstaller(project, Configuration.Debug);
                }
            });

        /// <summary>
        /// Генерирует необходимые свойства в проекте
        /// </summary>
        public Target GenerateProjectProps => _ => _
            .Requires(() => Project)
            .Requires(() => Configuration)
            .Executes(() => new TPropGen().GenerateProperties(ProjectForMsiBuild, Configuration));

        private void CreateOutDirectory()
        {
            var outDir = Solution.Directory / "out";
            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }
        }

        private void BuildInstaller(
            Project project,
            string configuration,
            AbsolutePath cert = null,
            string password = null,
            string digestAlgorithm = null,
            string timestampServerUrl = null)
        {
            GetOutputTmpDirectories(out var outputTmpDir, out var outputTmpDirBin);
            BuildProject(project, configuration, outputTmpDirBin);
            _wix.BuildMsi(
                project,
                Solution.AllProjects,
                configuration,
                outputTmpDir,
                outputTmpDirBin,
                cert,
                password,
                digestAlgorithm,
                timestampServerUrl);
            DeleteDirectory(outputTmpDir);
        }

        private void GetOutputTmpDirectories(out string outputTmpDir, out string outputTmpDirBin)
        {
            outputTmpDir = Path.Combine(Path.GetTempPath(), $"piktools_build_{Guid.NewGuid().ToString()}");
            outputTmpDirBin = Path.Combine(outputTmpDir, "bin");
        }

        private void BuildProject(
            Project project,
            string configuration,
            string outputTmpDirBin)
        {
            DotNetBuild(settings => settings
                .SetProjectFile(project.Path)
                .SetOutputDirectory(outputTmpDirBin)
                .SetConfiguration(configuration));
        }
    }
}