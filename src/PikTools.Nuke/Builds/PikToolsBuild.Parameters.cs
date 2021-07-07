namespace PikTools.Nuke.Builds
{
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
        private Regex _releaseBranchRegex;

        /// <summary>
        /// Configuration to build - Default is 'Debug' (local) or 'Release' (server)
        /// </summary>
        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        public Configuration Configuration { get; set; } = IsLocalBuild ? Configuration.Debug : Configuration.Release;

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

        // /// <summary>
        // /// Конфигурация
        // /// </summary>
        // [Parameter("Select configuration")]
        // public string Config
        // {
        //     get => _config ??=
        //         ConsoleUtility.PromptForChoice("Select config:", ("Debug", "Debug"), ("Release", "Release"));
        //     set => _config = value;
        // }

        /// <summary>
        /// Project
        /// </summary>
        [Parameter("Select project")]
        public virtual string Project
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
    }
}