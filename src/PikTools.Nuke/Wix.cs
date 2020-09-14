namespace PikTools.Nuke
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Tooling;
    using global::Nuke.Common.Tools.DotNet;
    using MsiBuilder;
    using SharpCompress.Archives;
    using SharpCompress.Common;
    using static global::Nuke.Common.Tools.DotNet.DotNetTasks;

    /// <summary>
    /// Wix extension
    /// </summary>
    public class Wix
    {
        private const string WixSourceUrl =
            "https://github.com/oleg-shilo/wixsharp/releases/download/v1.12.0.0/WixSharp.1.12.0.0.7z";

        private const string TempWixFolder = ".wixSharp";
        private const string WixSourceFileName = "wixSharp.7z";
        private const string WixBin = nameof(WixBin);
        private AbsolutePath _wixSharpBinPath;
        private AbsolutePath _wixBin;
        private readonly AssemblyScanner _assemblyScanner;

        public Wix()
        {
            _assemblyScanner = new AssemblyScanner();
        }

        /// <summary>
        /// Упаковывает проект
        /// </summary>
        /// <param name="project">Проект</param>
        /// <param name="configuration">Конфигурация</param>
        public void BuildMsi(Project project, string configuration)
        {
            Setup();

            var output = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var @out = Path.Combine(output, "bin");

            DotNetBuild(settings => settings
                .SetProjectFile(project.Path)
                .SetOutputDirectory(@out)
                .SetConfiguration(configuration));

            if (Directory.Exists(@out))
            {
                var file = Path.Combine(@out, $"{project.Name}.dll");

                var types = _assemblyScanner.Scan(file)
                    .Where(x => x.BaseTypeName == "PikToolsCommand" ||
                                x.BaseTypeName == "PikToolsApplication")
                    .ToList();

                var addInGenerator = new AddInGenerator();
                addInGenerator.GenerateAddInFile(project, types, output);

                var options = new Options()
                {
                    Comments = project.GetProperty("Comments"),
                    Description = project.GetProperty("Description"),
                    Version = project.GetProperty("Version") ?? string.Empty,
                    BundleDir = output,
                    InstallDir = "%AppDataFolder%/Autodesk/ApplicationPlugins",
                    ManifestDir = output,
                    OutDir = project.Solution.Directory / "out",
                    PackageGuid = project.GetProperty("PackageGuid") ?? Guid.NewGuid().ToString(),
                    UpgradeCode = project.GetProperty("UpgradeCode") ?? Guid.NewGuid().ToString(),
                    ProjectName = project.Name,
                    SourceDir = Path.Combine(output, "bin"),
                    OutFileName = project.Name
                };

                var toolPath = Environment.GetEnvironmentVariable("PIKTOOLS_MSIBUILDER_BIN");
                var p = ProcessTasks.StartProcess(
                    toolPath,
                    options.ToString(),
                    project.Solution.Directory / "out");
                p.WaitForExit();

                if (p.ExitCode != 0)
                {
                    throw new ApplicationException("Не удалось собрать msi пакет!!!");
                }

                FileSystemTasks.DeleteDirectory(output);
            }
        }

        private void Setup()
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
            var tmpPath = (AbsolutePath)Path.GetTempPath();
            var tempFolder = tmpPath / TempWixFolder;
            var result = tmpPath / TempWixFolder / WixSourceFileName;
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            if (!Directory.EnumerateFiles(tempFolder).Any())
            {
                Console.WriteLine("Downloading " + WixSourceUrl + "...");
                var webClient = new WebClient();
                webClient.DownloadFile(WixSourceUrl, result);
                Console.WriteLine("Done.");
            }

            return result;
        }
    }
}