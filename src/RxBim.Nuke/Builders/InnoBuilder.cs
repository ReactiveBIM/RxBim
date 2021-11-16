namespace RxBim.Nuke.Builders
{
    using System.Collections.Generic;
    using System.Drawing.Text;
    using System.IO;
    using System.Linq;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Utilities.Collections;
    using InnoSetup.ScriptBuilder;
    using InnoSetup.ScriptBuilder.Model.SetupSection;
    using MsiBuilder;

    /// <summary>
    /// Inno Setup builder
    /// </summary>
    public class InnoBuilder : IssBuilder
    {
        private readonly Options _options;
        private readonly AbsolutePath _outputProjDir;
        private readonly AbsolutePath _outputProjBinDir;
        private readonly string _projInstallDir;

        private readonly SetupBuilder _setupBuilder;

        private InnoBuilder(
            Options options,
            AbsolutePath outputProjDir,
            AbsolutePath outputProjBinDir)
        {
            _options = options;
            _outputProjDir = outputProjDir;
            _outputProjBinDir = outputProjBinDir;

            var installDir = options.InstallDir.Replace("%AppDataFolder%", "{userappdata}");
            _projInstallDir = $@"{installDir}\{options.ProjectName}";

            _setupBuilder = Setup.Create(options.ProductProjectName)
                .AppId(options.PackageGuid)
                .AppVersion(options.Version)
                .DefaultDirName(_projInstallDir)
                .PrivilegesRequired(PrivilegesRequired.Lowest)
                .OutputBaseFilename($"{options.OutFileName}_{options.Version}")
                .DisableDirPage(YesNo.Yes);

            Files
                .CreateEntry(
                    outputProjBinDir / "*",
                    InnoConstants.App).Flags(FileFlags.IgnoreVersion | FileFlags.RecurseSubdirs);
            Files
                .CreateEntry(outputProjDir / "*", installDir);
        }

        /// <summary>
        /// Create instance of <see cref="InnoBuilder"/>
        /// </summary>
        /// <param name="options">Setup options</param>
        /// <param name="outputProjDir">Output compile project directory</param>
        /// <param name="outputProjBinDir">Output "bin" directory of compile project</param>
        public static InnoBuilder Create(
            Options options,
            AbsolutePath outputProjDir,
            AbsolutePath outputProjBinDir)
            => new (options, outputProjDir, outputProjBinDir);

        /// <summary>
        /// Add setup and uninstall icons from <see cref="Options"/>
        /// </summary>
        public InnoBuilder AddIcons()
        {
            if (!string.IsNullOrWhiteSpace(_options.SetupIcon))
                _setupBuilder.SetupIconFile(_outputProjBinDir / _options.SetupIcon);
            if (!string.IsNullOrWhiteSpace(_options.UninstallIcon))
                _setupBuilder.UninstallDisplayIcon($@"{_projInstallDir}\{_options.UninstallIcon}");

            return this;
        }

        /// <summary>
        /// Add fonts files
        /// </summary>
        public InnoBuilder AddFonts()
        {
            FillFonts(_outputProjDir)
                .ForEach(font =>
                {
                    Files.CreateEntry(font, @"{autofonts}")
                        .FontInstall(GetFontName(font))
                        .Flags(FileFlags.OnlyIfDestFileExists | FileFlags.UninsNeverUninstall);
                });

            return this;
        }

        private static IEnumerable<string> FillFonts(string path)
            => Directory.EnumerateFiles(path, "*.ttf", SearchOption.AllDirectories).ToList();

        private static string GetFontName(string fontPath)
        {
#pragma warning disable CA1416
            var fontCol = new PrivateFontCollection();
            fontCol.AddFontFile(fontPath);
            return fontCol.Families.FirstOrDefault()?.Name;
#pragma warning restore CA1416
        }
    }
}