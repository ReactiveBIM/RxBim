namespace PikTools.Nuke
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Generators;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Tooling;
    using global::Nuke.Common.Tools.Git;
    using global::Nuke.Common.Tools.SignTool;
    using global::Nuke.Common.Utilities;
    using JetBrains.Annotations;

    /// <summary>
    /// Расширение билд скрипта для сборки MSI
    /// </summary>
    [PublicAPI]
    public abstract class PikToolsBuild : NukeBuild
    {
        private readonly Wix _wix;
        private string _project;
        private string _config;
        private Regex _releaseBranchRegex;

        /// <summary>
        /// ctor
        /// </summary>
        protected PikToolsBuild()
        {
            _wix = new Wix();
        }

        /// <summary>
        /// Solution
        /// </summary>
        [Solution]
        public Solution Solution { get; set; }

        /// <summary>
        /// Project
        /// </summary>
        [Parameter("Select project")]
        public string Project
        {
            get
            {
                if (_project == null)
                {
                    var result = ConsoleUtility.PromptForChoice(
                        "Select project:",
                        Solution.AllProjects
                            .Select(x => (x.Name, x.Name))
                            .ToArray());
                    _project = Solution.AllProjects.FirstOrDefault(x => x.Name == result)?.Name;
                }

                return _project;
            }
            set => _project = value;
        }

        /// <summary>
        /// BuildMsi
        /// </summary>
        public Target BuildMsi => _ => _
            .Description("Собирает MSi пакет из указанного проекта")
            .Requires(() => Project)
            .Requires(() => Config)
            .Executes(() =>
            {
                CreateOutDirectory();

                _wix.BuildMsi(ProjectForMsiBuild,
                    Solution.AllProjects,
                    Config,
                    (AbsolutePath)Cert,
                    Password,
                    Algorithm,
                    ServerUrl);
            });

        /// <summary>
        /// Проверяет текущюю ветку
        /// </summary>
        public Target CheckCurrentBranch => _ => _
            .Executes(() =>
            {
                var gitCurrentBranch = GitTasks.GitCurrentBranch();
                _releaseBranchRegex = new Regex("release/(?<version>[0-9.]*)");
                if (!_releaseBranchRegex.IsMatch(gitCurrentBranch))
                {
                    throw new ArgumentException("Current branch name is not valid!!!");
                }
            });

        /// <summary>
        /// Проверяет текущюю версию проектв и версию релиза
        /// </summary>
        public Target CheckStageVersion => _ => _
            .DependsOn(CheckCurrentBranch)
            .Executes(() =>
            {
                var gitCurrentBranch = GitTasks.GitCurrentBranch();
                var gitVersion = _releaseBranchRegex.Match(gitCurrentBranch).Groups["version"].Value;
                var projectVersion = ProjectForMsiBuild.GetProperty("Version");
                if (gitVersion != projectVersion)
                {
                    throw new ArgumentException("Project version is not equals git version!!!");
                }
            });

        /// <summary>
        /// Проверяет версию на продакшене
        /// </summary>
        public Target CheckProductionVersion => _ => _
            .Executes(() =>
            {
                if (GitTasks.GitCurrentBranch() != "master")
                {
                    throw new InvalidOperationException("Current branch should be master!");
                }

                var currentCommitHash = GitTasks.Git("log --pretty=format:%H -n 1")
                    .FirstOrDefault().Text;
                if (currentCommitHash == null)
                {
                    throw new InvalidOperationException();
                }

                var tag = GitTasks.Git("for-each-ref --sort=-creatordate refs/tags")
                    .Select(x => x.Text)
                    .FirstOrDefault(x => !x.Contains("-rc"));
                if (tag == null)
                {
                    throw new InvalidDataException("No any tag");
                }

                var pattern = @"\d+\.\d+\.\d+";
                var regex = new Regex($@"(?<hash>[0-9a-f]+)\s+\w+\s+(refs\/tags\/)(?<tag>{pattern})");

                if (!regex.IsMatch(tag))
                {
                    throw new InvalidDataException("Version is not valid");
                }

                var match = regex.Match(tag);
                var tagValue = match.Groups["tag"].Value;

                var hash = GitTasks.Git($"show {tagValue} --pretty=format:\"%H\" --quiet")
                    .Select(x => x.Text)
                    .Last();

                if (hash != currentCommitHash)
                {
                    throw new InvalidDataException("Tag hash != current commit hash");
                }

                var projectVersion = ProjectForMsiBuild.GetProperty("Version");
                if (tagValue != projectVersion)
                {
                    throw new ArgumentException("Project version is not equals git version!!!");
                }
            });

        /// <summary>
        /// Собирает msi для тестирования
        /// </summary>
        public Target BuildMsiForTesting => _ => _
            .Requires(() => Project)
            .DependsOn(CheckStageVersion)
            .Executes(() => { Config = "Debug"; })
            .Triggers(BuildMsi);

        /// <summary>
        /// Собирает msi для тестирования
        /// </summary>
        public Target BuildMsiForProduction => _ => _
            .Requires(() => Project)
            .DependsOn(CheckProductionVersion)
            .Executes(() => { Config = "Release"; })
            .Triggers(BuildMsi);

        /// <summary>
        /// Генерирует необходимые свойства в проекте
        /// </summary>
        public Target GenerateProjectProps => _ => _
            .Requires(() => Project)
            .Requires(() => Config)
            .Executes(() => new ProjectPropertiesGenerator().GenerateProperties(ProjectForMsiBuild, Config));

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
        /// Ставит тэг проекта который необходимо собрать для тестирования
        /// </summary>
        public Target TagProject => _ => _
            .Requires(() => Project)
            .Executes(() =>
            {
                var tag = $"Testing{Project}";
                GitTasks.Git($"tag {tag}");
            });

        /// <summary>
        /// Конфигурация
        /// </summary>
        [Parameter("Select configuration")]
        public string Config
        {
            get => _config ??=
                ConsoleUtility.PromptForChoice("Select config:", ("Debug", "Debug"), ("Release", "Release"));
            set => _config = value;
        }

        /// <summary>
        /// путь к сертификату
        /// </summary>
        [Parameter("Путь до сертификата")]
        public string Cert { get; set; }

        /// <summary>
        /// Пароль от сертификату
        /// </summary>
        [Parameter("Пароль от сертификата")]
        public string Password { get; set; }

        /// <summary>
        /// Алгоритм сертификата
        /// </summary>
        [Parameter("Алгоритм сертификата")]
        public string Algorithm { get; set; }

        /// <summary>
        /// сервер url
        /// </summary>
        [Parameter("Сервер проверки сертификата")]
        public string ServerUrl { get; set; }

        private Project ProjectForMsiBuild => Solution.AllProjects.FirstOrDefault(x => x.Name == _project);

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