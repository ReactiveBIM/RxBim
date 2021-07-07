#pragma warning disable SA1600, CS1591, SA1619
namespace PikTools.Nuke.Builds
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using global::Nuke.Common;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Utilities;

    /// <summary>
    /// Расширение Build-скрипта для сборки MSI. Параметры.
    /// </summary>
    public abstract partial class PikToolsBuild<TWix, TPackGen, TPropGen>
    {
        private readonly TWix _wix;
        private string _project;
        private string _config;
        private Regex _releaseBranchRegex;
        private string _outputTmpDir;
        private string _outputTmpDirBin;

        /// <summary>
        /// Configuration to build - Default is 'Debug' (local) or 'Release' (server)
        /// </summary>
        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        public Configuration Configuration { get; set; } = IsLocalBuild ? Configuration.Debug : Configuration.Release;

        /// <summary>
        /// Certificate path
        /// </summary>
        [Parameter("Certificate path")]
        public string Cert { get; set; }

        /// <summary>
        /// Private key container
        /// </summary>
        [Parameter("Private key container")]
        public string PrivateKey { get; set; }

        /// <summary>
        /// CSP containing for Private key
        /// </summary>
        [Parameter("CSP containing for Private key")]
        public string Csp { get; set; }

        /// <summary>
        /// Digest algorithm
        /// </summary>
        [Parameter("Digest algorithm")]
        public string Algorithm { get; set; }

        /// <summary>
        /// Timestamp server URL
        /// </summary>
        [Parameter("Timestamp server URL")]
        public string ServerUrl { get; set; }

        /// <summary>
        /// Selected configuration
        /// </summary>
        [Parameter("Select configuration")]
        public string Config
        {
            get => _config ??=
                ConsoleUtility.PromptForChoice("Select config:", ("Debug", "Debug"), ("Release", "Release"));
            set => _config = value;
        }

        /// <summary>
        /// Selected project
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
                            .Append((nameof(Solution), "All"))
                            .ToArray());

                    _project = result == nameof(Solution)
                        ? Solution.Name
                        : Solution.AllProjects.FirstOrDefault(x => x.Name == result)?.Name;
                }

                return _project;
            }
            set => _project = value;
        }

        /// <summary>
        /// Solution
        /// </summary>
        [Solution]
        public Solution Solution { get; set; }

        private Project ProjectForMsiBuild => Solution.AllProjects.FirstOrDefault(x => x.Name == _project);

        private string OutputTmpDir => _outputTmpDir ??= Path.Combine(Path.GetTempPath(), $"piktools_build_{Guid.NewGuid()}");

        private string OutputTmpDirBin => _outputTmpDirBin ??= Path.Combine(OutputTmpDir, "bin");
    }
}