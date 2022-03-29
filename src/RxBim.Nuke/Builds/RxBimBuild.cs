﻿namespace RxBim.Nuke.Builds
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Builders;
    using Extensions;
    using Generators;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Tooling;
    using global::Nuke.Common.Tools.DotNet;
    using global::Nuke.Common.Tools.Git;
    using global::Nuke.Common.Tools.InnoSetup;
    using InnoSetup.ScriptBuilder;
    using JetBrains.Annotations;
    using Models;
    using static global::Nuke.Common.IO.FileSystemTasks;
    using static global::Nuke.Common.Tools.DotNet.DotNetTasks;
    using static Helpers.WixHelper;

    /// <summary>
    /// Contains tools for install packages creating.
    /// </summary>
    /// <typeparam name="TBuilder">Installer builder</typeparam>
    /// <typeparam name="TPackGen">PackageContents file generator</typeparam>
    /// <typeparam name="TPropGen">Project properties generator</typeparam>
    [PublicAPI]
    public abstract partial class RxBimBuild<TBuilder, TPackGen, TPropGen> : NukeBuild
        where TBuilder : InstallerBuilder<TPackGen>, new()
        where TPackGen : PackageContentsGenerator, new()
        where TPropGen : ProjectPropertiesGenerator, new()
    {
        /// <summary>
        /// ctor
        /// </summary>
        protected RxBimBuild()
        {
            _builder = new TBuilder();
        }

        /// <summary>
        /// Builds an MSI package.
        /// </summary>
        public Target BuildMsi => _ => _
            .Description("Build MSI from selected project (if Release - sign assemblies)")
            .Requires(() => Project)
            .Requires(() => Configuration)
            .DependsOn(InstallWixTools)
            .DependsOn(SignAssemblies)
            .DependsOn(GenerateAdditionalFiles)
            .DependsOn(GeneratePackageContentsFile)
            .Executes(() =>
            {
                CreateOutDirectory();
                BuildMsiInstaller(ProjectForMsiBuild, Configuration);
            });

        /// <summary>
        /// Builds an EXE package.
        /// </summary>
        public virtual Target BuildInnoExe => _ => _
            .Description("Build installation EXE from selected project (if Release - sign assemblies)")
            .DependsOn(SignAssemblies)
            .DependsOn(GenerateAdditionalFiles)
            .DependsOn(GeneratePackageContentsFile)
            .Executes(() =>
            {
                CreateOutDirectory();
                BuildInnoInstaller(ProjectForMsiBuild, Configuration);
            });

        /// <summary>
        /// Compiles the project defined in <see cref="Project"/> to temporary path.
        /// </summary>
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
        /// Builds MSI from tag Testing{ProjectName}
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
                    BuildMsiInstaller(project, Configuration.Debug);
                }
            });

        /// <summary>
        /// Generates project properties (PackageGuid, UpgradeCode and other)
        /// </summary>
        public Target GenerateProjectProps => _ => _
            .Requires(() => Project)
            .Requires(() => Configuration)
            .Executes(() => new TPropGen().GenerateProperties(ProjectForMsiBuild, Configuration));

        /// <summary>
        /// Installs WixSharp.
        /// </summary>
        public Target InstallWixTools => _ => _
            .Executes(SetupWixTools);

        /// <summary>
        /// Signs assemblies af a given project.
        /// </summary>
        public virtual Target SignAssemblies => _ => _
            .Requires(() => Project)
            .Requires(() => Configuration)
            .DependsOn(CompileToTemp)
            .Executes(() =>
            {
                if (Configuration != Configuration.Release)
                    return;

                var types = GetAssemblyTypes(ProjectForMsiBuild, OutputTmpDirBin, OutputTmpDir, Configuration);

                types.SignAssemblies(
                    (AbsolutePath)OutputTmpDirBin,
                    (AbsolutePath)Cert,
                    PrivateKey,
                    Csp,
                    Algorithm,
                    ServerUrl);
            });

        /// <summary>
        /// Generates additional files.
        /// </summary>
        public Target GenerateAdditionalFiles => _ => _
            .Requires(() => Project)
            .Requires(() => Configuration)
            .DependsOn(CompileToTemp)
            .Executes(() =>
            {
                var types = GetAssemblyTypes(ProjectForMsiBuild, OutputTmpDirBin, OutputTmpDir, Configuration);
                _builder.GenerateAdditionalFiles(ProjectForMsiBuild.Name, Solution.AllProjects, types, OutputTmpDir);
            });

        /// <summary>
        /// Generates a package contents file.
        /// </summary>
        public Target GeneratePackageContentsFile => _ => _
            .Requires(() => Project)
            .Requires(() => Configuration)
            .DependsOn(CompileToTemp)
            .Executes(() =>
            {
                var types = GetAssemblyTypes(ProjectForMsiBuild, OutputTmpDirBin, OutputTmpDir, Configuration);
                _builder.GeneratePackageContentsFile(ProjectForMsiBuild, Configuration, types, OutputTmpDir);
            });

        private void CreateOutDirectory()
        {
            var outDir = Solution.Directory / "out";
            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir!);
        }

        private void BuildMsiInstaller(
            Project project,
            string configuration)
        {
            _builder.BuildMsi(project, configuration, OutputTmpDir, OutputTmpDirBin);
            DeleteDirectory(OutputTmpDir);
        }

        private void BuildInnoInstaller(
            Project project,
            string configuration)
        {
            var iss = TemporaryDirectory / "package.iss";
            var options = _builder.GetBuildMsiOptions(project, OutputTmpDir, configuration);
            var setupFileName = $"{options.OutFileName}_{options.Version}";

            InnoBuilder.Create(
                    options,
                    (AbsolutePath)OutputTmpDir,
                    (AbsolutePath)OutputTmpDirBin,
                    setupFileName)
                .AddIcons()
                .AddFonts()
                .AddUninstallScript()
                .Build(iss);

            var outDir = project.Solution.Directory / "out";
            InnoSetupTasks.InnoSetup(config => config
                .SetProcessToolPath(ToolPathResolver.GetPackageExecutable("Tools.InnoSetup", "ISCC.exe"))
                .SetScriptFile(iss)
                .SetOutputDir(outDir));

            DeleteDirectory(OutputTmpDir);
            SignSetupFile(outDir / $"{setupFileName}.exe");
        }

        private void SignSetupFile(string filePath)
        {
            if (Configuration != Configuration.Release)
                return;

            filePath.SignFile((AbsolutePath)Cert, PrivateKey, Csp, Algorithm, ServerUrl);
        }

        /// <summary>
        /// Gets assembly types
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
            return _types ??=
                project.GetAssemblyTypes(outputBinDir, _builder.GetBuildMsiOptions(project, outputDir, configuration));
        }
    }
}