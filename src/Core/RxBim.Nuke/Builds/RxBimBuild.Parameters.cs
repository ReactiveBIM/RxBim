namespace RxBim.Nuke.Builds
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using global::Nuke.Common;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Utilities;
    using Models;

    /// <content>
    /// Расширение Build-скрипта для сборки MSI. Параметры.
    /// </content>
    public abstract partial class RxBimBuild<TWix, TPackGen, TPropGen>
    {
        private readonly TWix _wix;
        private string? _project;
        private Regex? _releaseBranchRegex;
        private string? _outputTmpDir;
        private string? _outputTmpDirBin;
        private List<AssemblyType>? _types;

        /// <summary>
        /// Configuration to build - Default is 'Debug' (local) or 'Release' (server).
        /// </summary>
        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        public Configuration Configuration { get; set; } = IsLocalBuild ? Configuration.Debug : Configuration.Release;

        /// <summary>
        /// Certificate path.
        /// </summary>
        [Parameter("Certificate path")]
        public string? Cert { get; set; }

        /// <summary>
        /// Private key container.
        /// </summary>
        [Parameter("Private key container")]
        public string? PrivateKey { get; set; }

        /// <summary>
        /// CSP containing for Private key.
        /// </summary>
        [Parameter("CSP containing for Private key")]
        public string? Csp { get; set; }

        /// <summary>
        /// Digest algorithm.
        /// </summary>
        [Parameter("Digest algorithm")]
        public string? Algorithm { get; set; }

        /// <summary>
        /// Timestamp server URL.
        /// </summary>
        [Parameter("Timestamp server URL")]
        public string? ServerUrl { get; set; }

        /// <summary>
        /// Selected project.
        /// </summary>
        [Parameter("Select project")]
        public virtual string? Project
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
        /// Solution.
        /// </summary>
        [Solution]
        public Solution Solution { get; set; } = null!;

        /// <summary>
        /// Output temp directory path.
        /// </summary>
        protected virtual string OutputTmpDir =>
            _outputTmpDir ??= Path.Combine(Path.GetTempPath(), $"RxBim_build_{Guid.NewGuid()}");

        /// <summary>
        /// Output "bin" temp directory path.
        /// </summary>
        protected virtual string OutputTmpDirBin => _outputTmpDirBin ??= Path.Combine(OutputTmpDir, "bin");

        private Project ProjectForMsiBuild => Solution.AllProjects.First(x => x.Name == _project);
    }
}