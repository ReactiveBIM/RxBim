namespace RxBim.Nuke.Builds
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
    using global::Nuke.Common.Tools.DotNet;
    using Helpers;
    using JetBrains.Annotations;
    using Models;
    using static global::Nuke.Common.Tools.DotNet.DotNetTasks;
    using GitTasks = global::Nuke.Common.Tools.Git.GitTasks;

    /// <summary>
    /// Contains tools for MSI packages creating.
    /// </summary>
    /// <typeparam name="TBuilder">WIX-builder.</typeparam>
    /// <typeparam name="TPackGen">PackageContents file generator.</typeparam>
    /// <typeparam name="TPropGen">Project properties generator.</typeparam>
    /// <typeparam name="TOptsBuilder">Builder for <see cref="Options"/>.</typeparam>
    [PublicAPI]
    public abstract partial class RxBimBuild<TBuilder, TPackGen, TPropGen, TOptsBuilder> : NukeBuild
        where TBuilder : InstallerBuilder<TPackGen>, new()
        where TPackGen : PackageContentsGenerator, new()
        where TPropGen : ProjectPropertiesGenerator, new()
        where TOptsBuilder : OptionsBuilder, new()
    {
        /// <summary>
        /// ctor.
        /// </summary>
        protected RxBimBuild()
        {
            _builder = new TBuilder();
            OptionsBuilder = new TOptsBuilder();
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
                BuildMsiInstaller(ProjectForInstallBuild, Configuration);
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
                BuildInnoInstaller(ProjectForInstallBuild, Configuration);
            });

        /// <summary>
        /// Compiles the project defined in <see cref="Project"/> to temporary path.
        /// </summary>
        public virtual Target CompileToTemp => _ => _
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
        /// Builds MSI from tag Testing{ProjectName}.
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
        /// Generates project properties (PackageGuid, UpgradeCode and other).
        /// </summary>
        public Target GenerateProjectProps => _ => _
            .Requires(() => Project)
            .Requires(() => Configuration)
            .Executes(() => new TPropGen().GenerateProperties(ProjectForInstallBuild, Configuration));

        /// <summary>
        /// Installs WixSharp.
        /// </summary>
        public Target InstallWixTools => _ => _
            .Executes(WixHelper.SetupWixTools);

        /// <summary>
        /// Signs assemblies af a given project.
        /// </summary>
        public virtual Target SignAssemblies => _ => _
            .Requires(() => Project)
            .Requires(() => Configuration)
            .DependsOn(CompileToTemp)
            .Executes(SignAssemblyTypes);

        /// <summary>
        /// Generates additional files.
        /// </summary>
        public Target GenerateAdditionalFiles => _ => _
            .Requires(() => Project)
            .Requires(() => Configuration)
            .DependsOn(CompileToTemp)
            .Executes(() =>
            {
                var types = GetAssemblyTypes();
                _builder.GenerateAdditionalFiles(
                    ProjectForInstallBuild.Name,
                    Solution.AllProjects,
                    types,
                    OutputTmpDir);
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
                var types = GetAssemblyTypes();
                _builder.GeneratePackageContentsFile(
                    ProjectForInstallBuild,
                    Configuration,
                    types,
                    OutputTmpDir,
                    SeriesMaxAny);
            });

        /// <summary>
        /// Returns <see cref="Options"/>.
        /// </summary>
        /// <param name="project">Selected project.</param>
        /// <param name="configuration">Configuration.</param>
        protected virtual Options GetBuildOptions(Project project, string configuration)
        {
            var optionsBuilder = new TOptsBuilder();
            optionsBuilder
                .SetDefaultSettings(project)
                .SetDirectorySettings(_builder.GetInstallDir(project, configuration), OutputTmpDir)
                .SetProductVersion(project, configuration)
                .SetEnvironment(RxBimEnvironment)
                .SetVersion(project);

            if (TimestampRevisionVersion)
                optionsBuilder.SetTimestampRevisionVersion();

            return optionsBuilder.Build();
        }

        /// <summary>
        /// Signs assembly types.
        /// </summary>
        protected virtual void SignAssemblyTypes()
        {
            if (!CheckSignAvailable())
                return;

            var types = GetAssemblyTypes();
            var dllNames = types.GetDllNames((AbsolutePath)OutputTmpDirBin);
            dllNames.SignFiles(
                (AbsolutePath)Cert,
                PrivateKey.Ensure(),
                Csp.Ensure(),
                Algorithm.Ensure(),
                ServerUrl.Ensure());
        }

        /// <summary>
        /// File sign logic.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        protected virtual void SignSetupFile(string filePath)
        {
            if (!CheckSignAvailable())
                return;

            filePath.SignFile(
                (AbsolutePath)Cert,
                PrivateKey.Ensure(),
                Csp.Ensure(),
                Algorithm.Ensure(),
                ServerUrl.Ensure());
        }

        /// <summary>
        /// Gets assembly types.
        /// </summary>
        private List<AssemblyType> GetAssemblyTypes()
        {
            return _types ??= ProjectForInstallBuild.GetAssemblyTypes(OutputTmpDirBin,
                GetBuildOptions(ProjectForInstallBuild, Configuration));
        }

        private void CreateOutDirectory()
        {
            var outDir = Solution.Directory / "out";
            if (!Directory.Exists(outDir))
                Directory.CreateDirectory(outDir!);
        }

        private void BuildMsiInstaller(Project project, string configuration)
        {
            var options = GetBuildOptions(project, configuration);
            _builder.BuildMsi(ProjectForInstallBuild, OutputTmpDirBin, options);
            ((AbsolutePath)OutputTmpDir).DeleteDirectory();
        }

        private void BuildInnoInstaller(Project project, string configuration)
        {
            var options = GetBuildOptions(project, configuration);
            var setupFileName = $"{options.OutFileName}_{options.Version}";

            _builder.BuildInno(TemporaryDirectory, OutputTmpDir, OutputTmpDirBin, options);
            ((AbsolutePath)OutputTmpDir).DeleteDirectory();
            SignSetupFile((AbsolutePath)options.OutDir / $"{setupFileName}.exe");
        }

        private bool CheckSignAvailable()
        {
            return !string.IsNullOrWhiteSpace(Cert)
                   && !string.IsNullOrWhiteSpace(PrivateKey)
                   && !string.IsNullOrWhiteSpace(Csp)
                   && !string.IsNullOrWhiteSpace(Algorithm)
                   && !string.IsNullOrWhiteSpace(ServerUrl);
        }
    }
}