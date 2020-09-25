namespace PikTools.Nuke
{
    using System.Linq;
    using Generators;
    using global::Nuke.Common;
    using global::Nuke.Common.ProjectModel;
    using global::Nuke.Common.Utilities;
    using JetBrains.Annotations;

    /// <summary>
    /// Расширение билд скрипта для сборки MSI
    /// </summary>
    [PublicAPI]
    public abstract class PikToolsBuild : NukeBuild
    {
        private readonly Wix _wix;
        private Project _project;
        private string _config;

        /// <summary>
        /// ctor
        /// </summary>
        protected PikToolsBuild()
        {
            _wix = new Wix();
        }

        /// <summary>
        /// Solution
        /// </summary>
        [Solution]
        public Solution Solution { get; set; }

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

        /// <summary>
        /// BuildMsi
        /// </summary>
        public Target BuildMsi => _ => _
            .Description("Собирает MSi пакет из указанного проекта")
            .Requires(() => Project)
            .Requires(() => Config)
            .Executes(() => { _wix.BuildMsi(Project, Config); });

        /// <summary>
        /// Генерирует необходимые свойства в проекте
        /// </summary>
        public Target GenerateProjectProps => _ => _
            .Requires(() => Project)
            .Requires(() => Config)
            .Executes(() => new ProjectPropertiesGenerator().GenerateProperties(Project, Config));

        [Parameter("Select configuration")]
        private string Config
        {
            get => _config ??=
                ConsoleUtility.PromptForChoice("Select config:", ("Debug", "Debug"), ("Release", "Release"));
            set => _config = value;
        }
    }
}