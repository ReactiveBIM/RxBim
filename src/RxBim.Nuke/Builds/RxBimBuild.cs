#pragma warning disable
namespace RxBim.Nuke.Builds
{
    using System.Collections.Generic;
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

    /// <summary>
    /// Расширение Build-скрипта для сборки MSI
    /// </summary>
    /// <typeparam name="TWix">Тип WIX-сборщика</typeparam>
    /// <typeparam name="TPackGen">Тип генератора файла PackageContents</typeparam>
    /// <typeparam name="TPropGen">Тип генератора свойств проекта</typeparam>
    [PublicAPI]
    public abstract partial class RxBimBuild<TWix, TPackGen, TPropGen> : NukeBuild
        where TWix : WixBuilder<TPackGen>, new()
        where TPackGen : PackageContentsGenerator, new()
        where TPropGen : ProjectPropertiesGenerator, new()
    {
        /// <summary>
        /// ctor
        /// </summary>
        protected RxBimBuild()
        {
            _wix = new TWix();
        }

        /// <summary>
        /// BuildMsi
        /// </summary>
        public Target BuildMsi => _ => _
            .Description("Build MSI from selected project")
            .Requires(() => Project)
            .Requires(() => Configuration)
            .DependsOn(InstallWixTools)
            .DependsOn(SignAssemblies)
            .DependsOn(GenerateAdditionalFiles)
            .DependsOn(GeneratePackageContentsFile)
            .Executes(() =>
            {
                CreateOutDirectory();
                BuildInstaller(ProjectForMsiBuild, Configuration);
            });

        public Target CompileToTemp => _ => _
            .Description("Build project to temp output")
            .Requires(() => Project)
            .DependsOn(Restore)
            .Executes(() =>
            {
                DotNetBuild(settings => settings
                    .SetProjectFile(GetProjectPath(Project))
                    .SetOutputDirectory(OutputTmpDirBin)
                    .SetConfiguration(Configuration));
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

        /// <summary>
        /// Install WixSharp
        /// </summary>
        public Target InstallWixTools => _ => _
            .Executes(SetupWixTools);

        public virtual Target SignAssemblies => _ => _
            .Requires(() => Project)
            .Requires(() => Configuration)
            .DependsOn(CompileToTemp)
            .Executes(() =>
            {
                if (Configuration != Configuration.Release)
                    return;
                
                var types = GetAssemblyTypes(
                    ProjectForMsiBuild, OutputTmpDirBin, OutputTmpDir, Configuration);

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
            .Requires(() => Configuration)
            .DependsOn(CompileToTemp)
            .Executes(() =>
            {
                var types = GetAssemblyTypes(
                    ProjectForMsiBuild, OutputTmpDirBin, OutputTmpDir, Configuration);

                _wix.GenerateAdditionalFiles
                    (ProjectForMsiBuild.Name, Solution.AllProjects, types, OutputTmpDir);
            });

        public Target GeneratePackageContentsFile => _ => _
            .Requires(() => Project)
            .Requires(() => Configuration)
            .DependsOn(CompileToTemp)
            .Executes(() =>
            {
                _wix.GeneratePackageContentsFile(ProjectForMsiBuild, Configuration, OutputTmpDir);
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

        /// <summary>
        /// Get assembly types
        /// </summary>
        /// <param name="project">Selected Project</param>
        /// <param name="outputBinDir">Output assembly directory</param>
        /// <param name="outputDir">Output directory</param>
        /// <param name="configuration">Selected configuration</param>
        private List<AssemblyType> GetAssemblyTypes(
            Project project,
            string outputBinDir,
            string outputDir,
            string configuration)
        {
            var options = _wix.GetBuildMsiOptions(project, outputDir, configuration);
            return _types ??= project.GetAssemblyTypes(outputBinDir, options);
        }
    }
}