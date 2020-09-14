namespace PikTools.Nuke
{
    using System;
    using System.Linq;
    using global::Nuke.Common;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Utilities;

    /// <summary>
    /// Расширение билд скрипта для сборки MSI
    /// </summary>
    public abstract class PikToolsBuild : NukeBuild
    {
        private Project _project;
        [Solution] public Solution Solution;
        private string _config;
        private Wix _wix;

        /// <summary>
        /// ctor
        /// </summary>
        protected PikToolsBuild()
        {
            _wix = new Wix();
        }

        /// <summary>
        /// Project
        /// </summary>
        [Parameter("Select project")]
        public Project Project
        {
            get
            {
                if (_project == null)
                {
                    var result = ConsoleUtility.PromptForChoice(
                        "Select project:",
                        Solution.AllProjects
                            .Select(x => (x.Name, x.Name))
                            .ToArray());
                    _project = Solution.AllProjects.FirstOrDefault(x => x.Name == result);
                }

                return _project;
            }
            set => _project = value;
        }

        [Parameter("Select configuration")]
        private string Config
        {
            get => _config ??=
                ConsoleUtility.PromptForChoice("Select config:", ("Debug", "Debug"), ("Release", "Release"));
            set => _config = value;
        }

        /// <summary>
        /// BuildMsi
        /// </summary>
        public Target BuildMsi => _ => _
            .Description("Собирает MSi пакет из указанного проекта")
            .Requires(() => Project)
            .Requires(() => Config)
            .Executes(() =>
            {
                _wix.BuildMsi(Project, Config);
            });
    }
}