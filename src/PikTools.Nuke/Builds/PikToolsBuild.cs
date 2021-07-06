#pragma warning disable
namespace PikTools.Nuke.Builds
{
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Bimlab.Nuke.Nuget;
    using Generators;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Tools.Git;
    using JetBrains.Annotations;

    /// <summary>
    /// Расширение билд скрипта для сборки MSI
    /// </summary>
    [PublicAPI]
    public abstract partial class PikToolsBuild : NukeBuild
    {
        /// <summary>
        /// ctor
        /// </summary>
        protected PikToolsBuild()
        {
            _wix = new Wix();
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

                _wix.BuildMsi(ProjectForMsiBuild,
                    Solution.AllProjects,
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
                    _wix.BuildMsi(project, Solution.AllProjects, "Debug");
                }
            });

        /// <summary>
        /// Генерирует необходимые свойства в проекте
        /// </summary>
        public Target GenerateProjectProps => _ => _
            .Requires(() => Project)
            .Requires(() => Configuration)
            .Executes(() => new ProjectPropertiesGenerator().GenerateProperties(ProjectForMsiBuild, Configuration));

        private void CreateOutDirectory()
        {
            var @out = Solution.Directory / "out";
            if (!Directory.Exists(@out))
            {
                Directory.CreateDirectory(@out);
            }
        }
    }
}