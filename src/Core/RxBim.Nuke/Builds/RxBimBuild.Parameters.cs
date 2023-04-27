namespace RxBim.Nuke.Builds
{
    extern alias nc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Di;
    using Models;
    using nc::Nuke.Common;
    using nc::Nuke.Common.ProjectModel;
    using nc::Nuke.Common.Utilities;

    /// <content>
    /// Build-script extension for installer. Parameters.
    /// </content>
    public abstract partial class RxBimBuild<TBuilder, TPackGen, TPropGen>
    {
        private readonly TBuilder _builder;
        private string? _project;
        private Regex? _releaseBranchRegex;
        private string? _outputTmpDir;
        private string? _outputTmpDirBin;
        private List<AssemblyType>? _types;
        private bool _timestampRevisionVersion;

        /// <summary>
        /// Configuration to build - Default is 'Debug' (local) or 'Release' (server).
        /// </summary>
        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        public Configuration Configuration { get; set; } = IsLocalBuild ? Configuration.Debug : Configuration.Release;

        /// <summary>
        /// Environment variable.
        /// </summary>
        [Parameter("Environment variable")]
        public string RxBimEnvironment { get; set; } = EnvironmentRegistryConstants.DefaultEnvironment;

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
        /// Add timestamp revision version.
        /// </summary>
        [Parameter("Adds timestamp revision version")]
        public bool TimestampRevisionVersion
        {
            get => Configuration == Configuration.Debug || _timestampRevisionVersion;
            set => _timestampRevisionVersion = value;
        }

        /// <summary>
        /// Add project version from last tag.
        /// </summary>
        [Parameter("Adds project version from last tag")]
        public virtual bool VersionFromTag { get; set; }

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

        /// <summary>
        /// Selected project.
        /// </summary>
        protected virtual Project ProjectForInstallBuild => Solution.AllProjects.First(x => x.Name == Project);
    }
}