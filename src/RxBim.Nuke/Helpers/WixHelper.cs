namespace RxBim.Nuke.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using global::Nuke.Common.IO;
    using Octokit;
    using SharpCompress.Archives;
    using SharpCompress.Common;
    using static global::Nuke.Common.IO.FileSystemTasks;

    /// <summary>
    /// Wix extensions
    /// </summary>
    public static class WixHelper
    {
        private const string TempWixFolder = ".wixSharp";
        private const string WixSourceFileName = "wixSharp.7z";
        private const string WixBin = nameof(WixBin);

        /// <summary>
        /// Installs WixSharp
        /// </summary>
        public static void SetupWixTools()
        {
            var toolsDir = (AbsolutePath)Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var wixSharpBinPath = toolsDir / WixBin;
            if (!Directory.Exists(wixSharpBinPath))
            {
                Directory.CreateDirectory(wixSharpBinPath);
            }

            if (!Directory.EnumerateFiles(wixSharpBinPath).Any())
            {
                var wixSharp7Z = DownloadWixSharp();
                using Stream stream = File.OpenRead(wixSharp7Z);
                var reader = ArchiveFactory.Open(stream);
                foreach (var entry in reader.Entries.Where(entry => !entry.Key.StartsWith("Samples")))
                {
                    if (entry.IsDirectory || entry.Key.StartsWith("Samples"))
                        continue;
                    entry.WriteToDirectory(
                        wixSharpBinPath,
                        new ExtractionOptions { ExtractFullPath = true, Overwrite = true });
                }
            }

            var wixBin = wixSharpBinPath / "Wix_bin" / "bin";
            Environment.SetEnvironmentVariable("WIXSHARP_WIXDIR", wixBin);
        }

        private static string GetLatestReleaseUrl()
        {
            var gitHubClient = new GitHubClient(new ProductHeaderValue("RxBim"));
            var releases = gitHubClient.Repository.Release.GetAll("oleg-shilo", "wixsharp").GetAwaiter().GetResult();
            var latest = releases[0];
            var browserDownloadUrl = latest.Assets[0].BrowserDownloadUrl;
            return browserDownloadUrl;
        }

        /// <summary>
        /// Downloads WixSharp
        /// </summary>
        private static string DownloadWixSharp()
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
    }
}