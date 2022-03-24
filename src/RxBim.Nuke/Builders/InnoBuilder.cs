namespace RxBim.Nuke.Builders
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Text;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Extensions;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Utilities.Collections;
    using InnoSetup.ScriptBuilder;
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
            AbsolutePath outputProjBinDir,
            string setupFileName = null)
        {
            _options = options;
            _outputProjDir = outputProjDir;
            _outputProjBinDir = outputProjBinDir;

            var installDir = options.InstallDir.Replace("%AppDataFolder%", "{userappdata}");
            _projInstallDir = $@"{installDir}\{options.ProjectName}";
            var outputFileName = setupFileName ?? $"{options.OutFileName}_{options.Version}";

            _setupBuilder = Setup.Create(options.ProductProjectName)
                .AppId($"{{{{{options.PackageGuid}}}")
                .AppVersion(options.Version)
                .DefaultDirName(_projInstallDir)
                .UsePreviousAppDir(YesNo.No)
                .PrivilegesRequired(PrivilegesRequired.Lowest)
                .OutputBaseFilename(outputFileName)
                .DisableDirPage(YesNo.Yes);

            Files
                .CreateEntry(
                    outputProjBinDir / "*",
                    InnoConstants.App).Flags(FileFlags.IgnoreVersion | FileFlags.RecurseSubdirs);
            Files
                .CreateEntry(outputProjDir / "*", installDir);
        }

        /// <summary>
        /// Creates an instance of <see cref="InnoBuilder"/>
        /// </summary>
        /// <param name="options">Setup options</param>
        /// <param name="outputProjDir">Output compile project directory</param>
        /// <param name="outputProjBinDir">Output "bin" directory of compile project</param>
        /// <param name="setupFileName">Setup file name</param>
        public static InnoBuilder Create(
            Options options,
            AbsolutePath outputProjDir,
            AbsolutePath outputProjBinDir,
            string setupFileName = null)
            => new(options, outputProjDir, outputProjBinDir, setupFileName);

        /// <summary>
        /// Adds setup and uninstall icons from <see cref="Options"/>
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
        /// Adds uninstall script
        /// </summary>
        public InnoBuilder AddUninstallScript()
        {
            Code.CreateEntry(EmbeddedResourceExtensions.ReadResource("uninstall.pas"));
            return this;
        }

        /// <summary>
        /// Adds fonts files
        /// </summary>
        public InnoBuilder AddFonts()
        {
            FillFonts(_outputProjDir)
                .ForEach(font =>
                {
                    Files.CreateEntry(font, @"{autofonts}")
                        .FontInstall(GetFontName(font))
                        .Flags(FileFlags.OnlyIfDoesntExist | FileFlags.UninsNeverUninstall);
                });

            return this;
        }

        private static IEnumerable<string> FillFonts(string path)
            => Directory.EnumerateFiles(path, "*.ttf", SearchOption.AllDirectories).ToList();

        private static string GetFontName(string fontPath)
        {
            using var fontStream = new FileStream(fontPath, FileMode.Open);

#pragma warning disable CA1416
            return LoadFontFamily(fontStream).Name;
#pragma warning restore CA1416
        }

        /// <summary>
        /// Reads Font file as stream, because PrivateFontCollection.Dispose() does not work
        /// </summary>
        /// <param name="stream">Font file stream</param>
        private static FontFamily LoadFontFamily(Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            var data = Marshal.AllocCoTaskMem((int)stream.Length);

            try
            {
                Marshal.Copy(buffer, 0, data, (int)stream.Length);
#pragma warning disable CA1416
                var prvFontCollection = new PrivateFontCollection();
                prvFontCollection.AddMemoryFont(data, (int)stream.Length);

                return prvFontCollection.Families.First();
#pragma warning restore CA1416
            }
            finally
            {
                Marshal.FreeCoTaskMem(data);
            }
        }
    }
}