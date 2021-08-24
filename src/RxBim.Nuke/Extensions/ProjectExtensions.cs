namespace RxBim.Nuke.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Builds;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Tooling;
    using global::Nuke.Common.Tools.DotNet;
    using global::Nuke.Common.Tools.Git;
    using global::Nuke.Common.Utilities;
    using Models;
    using MsiBuilder;
    using static Constants;
    using static global::Nuke.Common.Tools.DotNet.DotNetTasks;
    using static Helpers.AssemblyScanner;

    /// <summary>
    /// Расширения для проекта
    /// </summary>
    public static class ProjectExtensions
    {
        /// <summary>
        /// Возвращает опции для установки
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="installDir">Папка установки</param>
        /// <param name="sourceDir">Папка исходников</param>
        /// <param name="configuration">Конфигурация</param>
        public static Options GetBuildMsiOptions(
            this Project project,
            string installDir,
            string sourceDir,
            string configuration)
        {
            var productVersion = project.GetProperty(nameof(Options.ProductVersion));
            if (string.IsNullOrWhiteSpace(productVersion)
                && configuration.Equals(Configuration.Release))
            {
                throw new ArgumentException(
                    $"Project {project.Name} should contain '{nameof(Options.ProductVersion)}' property with product version value!");
            }

            var outputFileName = project.Name.StartsWith(MsiFilePrefix)
                ? project.Name
                : $"{MsiFilePrefix}{project.Name}";

            if (!string.IsNullOrWhiteSpace(productVersion))
                outputFileName += $"_{productVersion}";

            var version = project.GetProperty(nameof(Options.Version)) ??
                          throw new ArgumentException(
                              $"Project {project.Name} should contain '{nameof(Options.Version)}' property with valid version value!");

            if (configuration == Configuration.Debug)
            {
                var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                version += $".{unixTimestamp}";
            }

            var options = new Options
            {
                Comments = project.GetProperty(nameof(Options.Comments)),
                Description = project.GetProperty(nameof(Options.Description)),
                Version = version,
                ProductVersion = productVersion,
                BundleDir = sourceDir,
                InstallDir = installDir,
                ManifestDir = sourceDir,
                OutDir = project.Solution.Directory / "out",
                PackageGuid = project.GetProperty(nameof(Options.PackageGuid)) ??
                              throw new ArgumentException(
                                  $"Project {project.Name} should contain '{nameof(Options.PackageGuid)}' property with valid guid value!"),
                UpgradeCode = project.GetProperty(nameof(Options.UpgradeCode)) ??
                              throw new ArgumentException(
                                  $"Project {project.Name} should contain '{nameof(Options.UpgradeCode)}' property with valid guid value!"),
                ProjectName = project.Name,
                ProductProjectName = outputFileName,
                SourceDir = Path.Combine(sourceDir, "bin"),
                OutFileName = outputFileName,
                AddAllAppToManifest = Convert.ToBoolean(project.GetProperty(nameof(Options.AddAllAppToManifest))),
                ProjectsAddingToManifest = project.GetProperty(nameof(Options.ProjectsAddingToManifest))
                    ?.Split(',', StringSplitOptions.RemoveEmptyEntries)
            };
            return options;
        }

        /// <summary>
        /// Собирает Msi
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="toolPath">Путь к инструменту сборки</param>
        /// <param name="options">Опции</param>
        public static void BuildMsiWithTool(
            this Project project,
            string toolPath,
            Options options)
        {
            var p = ProcessTasks.StartProcess(
                toolPath,
                options.ToString(),
                project.Solution.Directory / "out");

            p.WaitForExit();

            if (p.ExitCode != 0)
            {
                throw new ApplicationException("Building MSI package failed!!!");
            }
        }

        /// <summary>
        /// Собирает проект с помощью dotnet и возвращает путь к сборке
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="config">Конфигурация</param>
        public static AbsolutePath BuildProject(this Project project, string config)
        {
            DotNetBuild(settings => settings
                .SetConfiguration(config)
                .SetProjectFile(project));

            var binPath = project.GetTargetPath();
            return binPath;
        }

        /// <summary>
        /// Добавляет свойства в проект
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="properties">Свойства в виде XML-элементов</param>
        public static void AddPropertiesToProject(this Project project, IReadOnlyCollection<XElement> properties)
        {
            if (properties.Any())
            {
                var projectXml = XElement.Load(project.Path);

                projectXml.Add(new XElement("PropertyGroup", properties));
                projectXml.Save(project.Path);

                Logger.Info(
                    $"Properties {properties.Select(x => x.Name.ToString()).ToList().JoinComma()} for {project.Name} project added\"");

                project.CommitChanges();
            }
        }

        /// <summary>
        /// Генерирует свойства проекта для MSI
        /// </summary>
        /// <param name="project">Проект</param>
        public static IEnumerable<XElement> GenerateMsiProperties(this Project project)
        {
            if (project.GetProperty(nameof(Options.PackageGuid)) == null)
            {
                yield return new XElement(nameof(Options.PackageGuid), Guid.NewGuid());
            }

            if (project.GetProperty(nameof(Options.UpgradeCode)) == null)
            {
                yield return new XElement(nameof(Options.UpgradeCode), Guid.NewGuid());
            }
        }

        /// <summary>
        /// Возвращает типы сборок
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="output">Папка со сборками</param>
        /// <param name="options">Опции</param>
        public static List<AssemblyType> GetAssemblyTypes(
            this Project project,
            string output,
            Options options)
        {
            var file = Path.Combine(output, $"{project.Name}.dll");

            var types = GetAssemblyTypes(file, new[] { RxBimCommand, RxBimApplication });

            var additionalFiles = new List<string>();
            if (options.AddAllAppToManifest)
            {
                // Добавляем все сборки с Application из out папки
                additionalFiles = Directory.GetFiles(output, "*.dll")
                    .Except(new[] { file })
                    .ToList();
            }
            else if (options.ProjectsAddingToManifest != null
                     && options.ProjectsAddingToManifest.Any())
            {
                // Добавляет дополнительно Application только из заданных в опции сборок
                additionalFiles = options.ProjectsAddingToManifest
                    .Select(p => Path.Combine(output, $"{p.Trim()}.dll"))
                    .ToList();
                if (additionalFiles.Any(f => !File.Exists(f)))
                {
                    throw new FileNotFoundException(
                        $"Не найдена сборка указанная в параметре {nameof(Options.ProjectsAddingToManifest)}");
                }
            }

            foreach (var f in additionalFiles)
            {
                types.AddRange(GetAssemblyTypes(f, new[] { RxBimApplication }));
            }

            return types;
        }

        /// <summary>
        /// Возвращает целевую папку для проекта
        /// </summary>
        /// <param name="project">Проект</param>
        public static AbsolutePath GetTargetDir(this Project project)
        {
            var targetFx = project.GetTargetFramework(out var multiple);

            var targetDir = project.Directory / project.GetProperty("OutputPath");

            if (multiple)
            {
                targetDir /= targetFx;
            }

            return targetDir;
        }

        /// <summary>
        /// Возвращает путь к сборке проекта
        /// </summary>
        /// <param name="project">Проект</param>
        public static AbsolutePath GetTargetPath(this Project project)
        {
            return project.GetTargetDir() / $"{project.GetProperty("AssemblyName")}.dll";
        }

        /// <summary>
        /// Зафиксировать изменения проекта в GIT
        /// </summary>
        /// <param name="project">Проект</param>
        private static void CommitChanges(this Project project)
        {
            var commit = ConsoleUtility.PromptForChoice("Commit changes?", ("Yes", "Yes"), ("No", "No"));

            if (commit switch
            {
                "Yes" => true,
                "No" => false,
                _ => false
            })
            {
                GitTasks.Git($"add \"{project.Path}\"", project.Solution.Directory);
                GitTasks.Git(
                    $"commit -m \"Generated properties for {project.Name} project\"",
                    project.Solution.Directory);
            }
        }

        /// <summary>
        /// Возвращает название целевого фреймворка
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="multiple">Задано несколько (свойством TargetFrameworks)</param>
        private static string GetTargetFramework(this Project project, out bool multiple)
        {
            var fxNameSingle = project.GetProperty("TargetFramework");
            if (!string.IsNullOrWhiteSpace(fxNameSingle))
            {
                multiple = false;
                return string.Empty;
            }

            var fxNameFirst = project.GetTargetFrameworks().FirstOrDefault();
            if (string.IsNullOrEmpty(fxNameFirst))
            {
                throw new InvalidOperationException($"Can't find target framework for project {project.Name}");
            }

            multiple = true;
            return fxNameFirst;
        }

        /// <summary>
        /// Возвращает типы сборок
        /// </summary>
        /// <param name="file">Файл</param>
        /// <param name="typeNames">Названия типов</param>
        /// <returns></returns>
        private static List<AssemblyType> GetAssemblyTypes(
            string file,
            string[] typeNames)
        {
            var types = Scan(file)
                .Where(x => typeNames.Contains(x.BaseTypeName))
                .ToList();
            return types;
        }
    }
}