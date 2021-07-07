namespace PikTools.Nuke.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Builds;
    using Extensions;
    using Generators;
    using global::Nuke.Common.IO;
    using Octokit;
    using static Constants;
    using static Helpers.WixHelper;

    /// <summary>
    /// Wix extension
    /// </summary>
    public class WixBuilder<T>
        where T : PackageContentsGenerator, new()
    {
        /// <summary>
        /// Упаковывает проект
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="allProject">Все проекты</param>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="outputDir">Путь к папке со всеми файлами, которые нужно упаковать в установщик</param>
        /// <param name="outputBinDir">Путь к папке, содержащей сборки, которые войдут в пакет установки</param>
        /// <param name="cert">Путь к сертификату</param>
        /// <param name="password">Пароль к сертификату</param>
        /// <param name="digestAlgorithm">Алгоритм сертификата</param>
        /// <param name="timestampServerUrl">Сервер url</param>
        public void BuildMsi(
            global::Nuke.Common.ProjectModel.Project project,
            IEnumerable<global::Nuke.Common.ProjectModel.Project> allProject,
            string configuration,
            string outputDir,
            string outputBinDir,
            AbsolutePath cert = null,
            string password = null,
            string digestAlgorithm = null,
            string timestampServerUrl = null)
        {
            SetupWixTools();

            if (Directory.Exists(outputBinDir))
            {
                var installDir = GetInstallDir(project, configuration);

                var options = project.GetBuildMsiOptions(installDir, outputDir, configuration);

                var types = project.GetAssemblyTypes(outputBinDir, options);

                if (configuration == Configuration.Release)
                {
                    types.SignAssemblies(
                        (AbsolutePath)outputBinDir,
                        cert,
                        password,
                        digestAlgorithm,
                        timestampServerUrl);
                }

                GenerateAdditionalFiles(project.Name, allProject, types, outputDir);

                GeneratePackageContentsFile(project, configuration, outputDir);

                var toolPath = GetMsiBuilderToolPath();

                project.BuildMsiWithTool(toolPath, options);
            }
        }

        /// <summary>
        /// Генерация дополнительных файлов
        /// </summary>
        /// <param name="rootProjectName">Название основного проекта</param>
        /// <param name="allProject">Проекты</param>
        /// <param name="addInTypes">Типы для инструментов</param>
        /// <param name="outputDirectory">Путь к папке, куда нужно поместить сгенерированные файлы</param>
        protected virtual void GenerateAdditionalFiles(
            string rootProjectName,
            IEnumerable<global::Nuke.Common.ProjectModel.Project> allProject,
            IEnumerable<AssemblyType> addInTypes,
            string outputDirectory)
        {
        }

        /// <summary>
        /// Возвращает true, если надо генерировать PackageContents
        /// </summary>
        /// <param name="configuration">Текущая конфигурация</param>
        protected virtual bool NeedGeneratePackageContents(string configuration) => true;

        /// <summary>
        /// Возвращает путь к папке установки для конфигурации Debug
        /// </summary>
        /// <param name="project">Проект</param>
        protected virtual string GetDebugInstallDir(global::Nuke.Common.ProjectModel.Project project)
        {
            return GetReleaseInstallDir(project);
        }

        /// <summary>
        /// Возвращает путь к папке установки для конфигурации Release
        /// </summary>
        /// <param name="project">Проект</param>
        private string GetReleaseInstallDir(global::Nuke.Common.ProjectModel.Project project)
        {
            return $"%AppDataFolder%/Autodesk/ApplicationPlugins/{project.Name}.bundle";
        }

        private string GetInstallDir(
            global::Nuke.Common.ProjectModel.Project project,
            string configuration)
        {
            return configuration switch
            {
                "Debug" => GetDebugInstallDir(project),
                "Release" => GetReleaseInstallDir(project),
                _ => throw new ArgumentException("Configuration not set!")
            };
        }

        private void GeneratePackageContentsFile(
            global::Nuke.Common.ProjectModel.Project project,
            string configuration,
            string output)
        {
            if (NeedGeneratePackageContents(configuration))
            {
                var packageContentsGenerator = new T();
                packageContentsGenerator.Generate(project, output);
            }
        }
    }
}