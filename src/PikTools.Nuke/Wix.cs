namespace PikTools.Nuke
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Application.Api;
    using Command.Api;
    using Generators;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Tooling;
    using global::Nuke.Common.Tools.DotNet;
    using global::Nuke.Common.Tools.SignTool;
    using Microsoft.Win32;
    using MsiBuilder;
    using Octokit;
    using SharpCompress.Archives;
    using SharpCompress.Common;
    using static global::Nuke.Common.IO.FileSystemTasks;
    using static global::Nuke.Common.Tools.DotNet.DotNetTasks;

    /// <summary>
    /// Wix extension
    /// </summary>
    public class Wix
    {
        private const string TempWixFolder = ".wixSharp";
        private const string WixSourceFileName = "wixSharp.7z";
        private const string WixBin = nameof(WixBin);
        private readonly AssemblyScanner _assemblyScanner;
        private AbsolutePath _wixSharpBinPath;
        private AbsolutePath _wixBin;

        /// <summary>
        /// ctor
        /// </summary>
        public Wix()
        {
            _assemblyScanner = new AssemblyScanner();
        }

        /// <summary>
        /// Упаковывает проект
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="allProject">Все проекты</param>
        /// <param name="configuration">Конфигурация</param>
        /// <param name="cert">Путь к сертификату</param>
        /// <param name="password">пароль к сертификату</param>
        /// <param name="digestAlgorithm">Алгоритм сертификата</param>
        /// <param name="timestampServerUrl">сервер url</param>
        public void BuildMsi(
            global::Nuke.Common.ProjectModel.Project project,
            IReadOnlyCollection<global::Nuke.Common.ProjectModel.Project> allProject,
            string configuration,
            AbsolutePath cert = null,
            string password = null,
            string digestAlgorithm = null,
            string timestampServerUrl = null)
        {
            CheckNetFramework();

            SetupWixTools();

            var output = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var @out = Path.Combine(output, "bin");

            DotNetBuild(settings => settings
                .SetProjectFile(project.Path)
                .SetOutputDirectory(@out)
                .SetConfiguration(configuration));

            if (configuration == "Release")
            {
                SignAssembly(project, (AbsolutePath)@out, cert, password, digestAlgorithm, timestampServerUrl);
            }

            if (Directory.Exists(@out))
            {
                var options = GetBuildMsiOptions(project, output, configuration);

                var types = GetAssemblyTypes(project, @out, options);

                GenerateRevitManifestFile(project.Name, allProject, types, output);

                GeneratePackageContentsFile(project, configuration, output);

                var toolPath = GetMsiBuilderToolPath();

                InnerBuildMsi(project, toolPath, options);

                DeleteDirectory(output);
            }
        }

        private void GeneratePackageContentsFile(
            global::Nuke.Common.ProjectModel.Project project,
            string configuration,
            string output)
        {
            if (configuration == "Release")
            {
                var packageContentsGenerator = new PackageContentsGenerator();
                packageContentsGenerator.Generate(project, output);
            }
        }

        private void CheckNetFramework()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5");
            if (key == null)
            {
                throw new ApplicationException(
                    ".NET Framework v3.5 not find on system!");
            }
        }

        private void InnerBuildMsi(global::Nuke.Common.ProjectModel.Project project, string toolPath, Options options)
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

        private string GetMsiBuilderToolPath()
        {
            var toolPath = Environment.GetEnvironmentVariable("PIKTOOLS_MSIBUILDER_BIN");

            if (toolPath == null)
            {
                throw new ArgumentException($"Environment has no 'PIKTOOLS_MSIBUILDER_BIN' variable!");
            }

            if (!FileExists((AbsolutePath)toolPath))
            {
                throw new ArgumentException(
                    $"PikTools.MsiBuilder tool not found in {toolPath}!");
            }

            return toolPath;
        }

        private Options GetBuildMsiOptions(
            global::Nuke.Common.ProjectModel.Project project,
            string output,
            string configuration)
        {
            var installDir = configuration switch
            {
                "Debug" => "%AppDataFolder%/Autodesk/Revit/Addins/2019",
                "Release" => $"%AppDataFolder%/Autodesk/ApplicationPlugins/{project.Name}.bundle",
                _ => throw new ArgumentException("Configuration not setted!")
            };

            var outputFileName = configuration == "Debug" ? $"PikTools.{project.Name}" : project.Name;

            var version = project.GetProperty("Version") ??
                          throw new ArgumentException(
                              $"Project {project.Name} should contain 'PackageGuid' property with valid guid value!");
            if (configuration == "Debug")
            {
                var unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                version += $".{unixTimestamp}";
            }

            var options = new Options()
            {
                Comments = project.GetProperty("Comments"),
                Description = project.GetProperty("Description"),
                Version = version,
                BundleDir = output,
                InstallDir = installDir,
                ManifestDir = output,
                OutDir = project.Solution.Directory / "out",
                PackageGuid = project.GetProperty("PackageGuid") ??
                              throw new ArgumentException(
                                  $"Project {project.Name} should contain 'PackageGuid' property with valid guid value!"),
                UpgradeCode = project.GetProperty("UpgradeCode") ??
                              throw new ArgumentException(
                                  $"Project {project.Name} should contain 'UpgradeCode' property with valid guid value!"),
                ProjectName = project.Name,
                SourceDir = Path.Combine(output, "bin"),
                OutFileName = outputFileName,
                AddAllAppToManifest = Convert.ToBoolean(project.GetProperty(nameof(Options.AddAllAppToManifest))),
                ProjectsAddingToManifest = project.GetProperty(nameof(Options.ProjectsAddingToManifest))
                    ?.Split(',', StringSplitOptions.RemoveEmptyEntries)
            };
            return options;
        }

        private void GenerateRevitManifestFile(
            string rootProjectName,
            IReadOnlyCollection<global::Nuke.Common.ProjectModel.Project> allProject,
            IReadOnlyList<AssemblyType> addInTypes,
            string output)
        {
            var addInGenerator = new AddInGenerator();
            var addInTypesPerProjects = addInTypes
                .Select(x => new ProjectWithAssemblyType(
                    allProject.FirstOrDefault(proj => proj.Name == x.AssemblyName), x))
                .ToList();
            addInGenerator.GenerateAddInFile(rootProjectName, addInTypesPerProjects, output);
        }

        private List<AssemblyType> GetAssemblyTypes(
            global::Nuke.Common.ProjectModel.Project project,
            string @out,
            Options options)
        {
            var file = Path.Combine(@out, $"{project.Name}.dll");

            var types = GetAssemblyTypes(file, new[] { nameof(PikToolsCommand), nameof(PikToolsApplication) });

            var additionalFiles = new List<string>();
            if (options.AddAllAppToManifest)
            {
                // Добавляем все сборки с Application из out папки
                additionalFiles = Directory.GetFiles(@out, "*.dll")
                    .Except(new[] { file })
                    .ToList();
            }
            else if (options.ProjectsAddingToManifest != null
                     && options.ProjectsAddingToManifest.Any())
            {
                // Добавляе дополнительно Application только из заданных в опции сборок
                additionalFiles = options.ProjectsAddingToManifest
                    .Select(p => Path.Combine(@out, $"{p.Trim()}.dll"))
                    .ToList();
                if (additionalFiles.Any(f => !File.Exists(f)))
                    throw new FileNotFoundException($"Не найдена сборка указанная в параметре {nameof(Options.ProjectsAddingToManifest)}");
            }

            foreach (var f in additionalFiles)
                types.AddRange(GetAssemblyTypes(f, new[] { nameof(PikToolsApplication) }));

            return types;
        }

        private List<AssemblyType> GetAssemblyTypes(string file, string[] typeNames)
        {
            var types = _assemblyScanner.Scan(file)
                .Where(x => typeNames.Contains(x.BaseTypeName))
                .ToList();
            return types;
        }

        private void SetupWixTools()
        {
            var tmpPath = (AbsolutePath)Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            _wixSharpBinPath = tmpPath / WixBin;
            if (!Directory.Exists(_wixSharpBinPath))
            {
                Directory.CreateDirectory(_wixSharpBinPath);
            }

            if (!Directory.EnumerateFiles(_wixSharpBinPath).Any())
            {
                var wixSharp7Z = DownloadWixSharp();
                using Stream stream = File.OpenRead(wixSharp7Z);
                var reader = ArchiveFactory.Open(stream);
                foreach (var entry in reader.Entries.Where(entry => !entry.Key.StartsWith("Samples")))
                {
                    if (entry.IsDirectory || entry.Key.StartsWith("Samples"))
                        continue;
                    entry.WriteToDirectory(_wixSharpBinPath,
                        new ExtractionOptions { ExtractFullPath = true, Overwrite = true });
                }
            }

            _wixBin = _wixSharpBinPath / "Wix_bin" / "bin";
            Environment.SetEnvironmentVariable("WIXSHARP_WIXDIR", _wixBin);
        }

        private string DownloadWixSharp()
        {
            var browserDownloadUrl = GetLatestReleaseUrl();

            var tmpPath = (AbsolutePath)Path.GetTempPath();
            var tempFolder = tmpPath / TempWixFolder;
            var result = tmpPath / TempWixFolder / WixSourceFileName;
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            if (!Directory.EnumerateFiles(tempFolder).Any())
            {
                Console.WriteLine("Downloading " + browserDownloadUrl + "...");
                var webClient = new WebClient();
                webClient.DownloadFile(browserDownloadUrl, result);
                Console.WriteLine("Done.");
            }

            return result;
        }

        private string GetLatestReleaseUrl()
        {
            var gitHubClient = new GitHubClient(new ProductHeaderValue("PikTools"));
            var releases = gitHubClient.Repository.Release.GetAll("oleg-shilo", "wixsharp").GetAwaiter().GetResult();
            var latest = releases[0];
            var browserDownloadUrl = latest.Assets[0].BrowserDownloadUrl;
            return browserDownloadUrl;
        }

        private void SignAssembly(
            global::Nuke.Common.ProjectModel.Project project,
            AbsolutePath outputDirectory,
            AbsolutePath cert,
            string password,
            string digestAlgorithm,
            string timestampServerUrl)
        {
            cert = cert ??
                   throw new ArgumentException("Не указал сертификат");

            password = password ??
                       throw new ArgumentException("Не указан пароль сертификата");
            digestAlgorithm = digestAlgorithm ??
                              throw new ArgumentException("Не указан алгоритм сертификата");
            timestampServerUrl = timestampServerUrl ??
                                 throw new ArgumentException("Не указан сервер проверки сертификата");

            var fileName = outputDirectory / $"{project.GetProperty("AssemblyName")}.dll";

            var settings = new SignToolSettings()
                .SetFileDigestAlgorithm(digestAlgorithm)
                .SetFile(cert)
                .SetFiles(fileName)
                .SetPassword(password)
                .SetTimestampServerDigestAlgorithm(digestAlgorithm)
                .SetRfc3161TimestampServerUrl(timestampServerUrl);

            if (!settings.HasValidToolPath())
            {
                var programFilesPath =
                    (AbsolutePath)Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                settings = settings
                    .SetToolPath(programFilesPath / "Microsoft SDKs" / "ClickOnce" / "SignTool" / "signtool.exe");
            }

            Logger.Info($"ToolPath: {settings.ToolPath}");

            SignToolTasks.SignTool(settings);
        }
    }
}