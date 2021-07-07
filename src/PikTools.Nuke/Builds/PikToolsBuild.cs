#pragma warning disable
namespace PikTools.Nuke.Builds
{
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Extensions;
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
    using static Helpers.WixHelper;
    using static Constants;

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
            .Description("Build MSI from selected project")
            .Requires(() => Project)
            .Requires(() => Config)
            .DependsOn(InstallWixTools)
            .DependsOn(SignAssemblies)
            .DependsOn(GenerateAdditionalFiles)
            .DependsOn(GeneratePackageContentsFile)
            .Executes(() =>
            {
                CreateOutDirectory();

                BuildInstaller(ProjectForMsiBuild, Config);
            });

        /// <summary>
        /// Build selected project
        /// </summary>
        public Target BuildProject => _ => _
            .Requires(() => Project)
            .Requires(() => Config)
            .Executes(() =>
            {
                DotNetBuild(settings => settings
                    .SetProjectFile(ProjectForMsiBuild.Path)
                    .SetOutputDirectory(OutputTmpDirBin)
                    .SetConfiguration(Config));
            });

        /// <summary>
        /// Собирает msi для тестирования
        /// </summary>
        public Target BuildMsiForTesting => _ => _
            .Requires(() => Project)
            .DependsOn(CheckStageVersion)
            .Executes(() => { Config = Debug; })
            .Triggers(BuildMsi);

        /// <summary>
        /// Собирает msi для тестирования
        /// </summary>
        public Target BuildMsiForProduction => _ => _
            .Requires(() => Project)
            .DependsOn(CheckProductionVersion)
            .Executes(() => { Config = Release; })
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
                    BuildInstaller(project, Debug);
                }
            });

        /// <summary>
        /// Генерирует необходимые свойства в проекте
        /// </summary>
        public Target GenerateProjectProps => _ => _
            .Requires(() => Project)
            .Requires(() => Config)
            .Executes(() => new TPropGen().GenerateProperties(ProjectForMsiBuild, Config));

        /// <summary>
        /// Install WixSharp
        /// </summary>
        public Target InstallWixTools => _ => _
            .Executes(SetupWixTools);

        public virtual Target SignAssemblies => _ => _
            .Requires(() => Project)
            .Requires(() => Config)
            .DependsOn(BuildProject)
            .Executes(() =>
            {
                if (Config != Release)
                    return;
                
                var types = _wix.GetAssemblyTypes(
                    ProjectForMsiBuild, OutputTmpDirBin, OutputTmpDir, Config);

                types.SignAssemblies(
                    (AbsolutePath)OutputTmpDirBin,
                    (AbsolutePath)Cert,
                    PrivateKey,
                    Csp, 
                    Algorithm,
                    ServerUrl);
            });

        public Target GenerateAdditionalFiles => _ => _
            .Requires(() => Project)
            .Requires(() => Config)
            .DependsOn(BuildProject)
            .Executes(() =>
            {
                var types = _wix.GetAssemblyTypes(
                    ProjectForMsiBuild, OutputTmpDirBin, OutputTmpDir, Config);

                _wix.GenerateAdditionalFiles
                    (ProjectForMsiBuild.Name, Solution.AllProjects, types, OutputTmpDir);
            });

        public Target GeneratePackageContentsFile => _ => _
            .Requires(() => Project)
            .Requires(() => Config)
            .DependsOn(BuildProject)
            .Executes(() =>
            {
                _wix.GeneratePackageContentsFile(ProjectForMsiBuild, Config, OutputTmpDir);
            });

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
            string configuration)
        {
            _wix.BuildMsi(
                project,
                configuration,
                OutputTmpDir,
                OutputTmpDirBin);
            DeleteDirectory(OutputTmpDir);
        }
    }
}