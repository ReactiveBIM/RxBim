#pragma warning disable SA1619
namespace PikTools.Nuke.Builds
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using global::Nuke.Common;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Tools.Git;

    /// <summary>
    /// Расширение Build-скрипта для сборки MSI. Targets для работы с GIT.
    /// </summary>
    public abstract partial class PikToolsBuild<TWix, TPackGen, TPropGen>
    {
        /// <summary>
        /// Проверяет текущую ветку
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
        /// Проверяет текущую версию проекта и версию релиза
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
        /// Ставит тэг проекта который необходимо собрать для тестирования
        /// </summary>
        public Target TagProject => _ => _
            .Requires(() => Project)
            .Executes(() =>
            {
                var tag = $"Testing{Project}";
                GitTasks.Git($"tag {tag}");
            });
    }
}