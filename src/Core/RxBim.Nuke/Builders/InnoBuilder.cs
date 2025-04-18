namespace RxBim.Nuke.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Text;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Di;
    using Extensions;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Utilities.Collections;
    using Helpers;
    using InnoSetup.ScriptBuilder;
    using JetBrains.Annotations;

    /// <summary>
    /// The Inno Setup builder.
    /// </summary>
    public class InnoBuilder : IssBuilder
    {
        private readonly Options _options;
        private readonly string _projInstallDir;

        private readonly SetupBuilder _setupBuilder;

        private InnoBuilder(Options options, string? setupFileName = null)
        {
            _options = options;

            var installDir = options.InstallDir.Ensure().Replace("%AppDataFolder%", "{userappdata}");
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
        }

        /// <summary>
        /// Creates an instance of <see cref="InnoBuilder"/>.
        /// </summary>
        /// <param name="options">Setup options.</param>
        /// <param name="setupFileName">Setup file name.</param>
        public static InnoBuilder Create(Options options, string? setupFileName = null) => new(options, setupFileName);

        /// <summary>
        /// Adds setup and uninstall icons from <see cref="Options"/>.
        /// </summary>
        /// <param name="outputDirectory">Output directory of compile project.</param>
        public InnoBuilder AddIcons(AbsolutePath outputDirectory)
        {
            if (!string.IsNullOrWhiteSpace(_options.SetupIcon))
                _setupBuilder.SetupIconFile(outputDirectory / _options.SetupIcon);
            if (!string.IsNullOrWhiteSpace(_options.UninstallIcon))
                _setupBuilder.UninstallDisplayIcon($@"{_projInstallDir}\{_options.UninstallIcon}");

            return this;
        }

        /// <summary>
        /// Adds files entry.
        /// </summary>
        /// <param name="sourceDirectory">Source directory.</param>
        /// <param name="destinationDirectory">Destination directory.</param>
        /// <param name="fileFlags"><see cref="FileFlags"/>.</param>
        public InnoBuilder AddFilesEntry(
            string sourceDirectory,
            string destinationDirectory,
            FileFlags? fileFlags = null)
        {
            var builder = Files.CreateEntry((AbsolutePath)sourceDirectory / "*", destinationDirectory);
            if (fileFlags != null)
            {
                builder.Flags(fileFlags.Value);
            }

            return this;
        }

        /// <summary>
        /// Adds custom setting to <see cref="SetupBuilder"/>.
        /// </summary>
        /// <param name="customSettings">Action to add settings.</param>
        [UsedImplicitly]
        public InnoBuilder AddCustomSettings(Action<SetupBuilder> customSettings)
        {
            customSettings.Invoke(_setupBuilder);
            return this;
        }

        /// <summary>
        /// Adds uninstall script.
        /// </summary>
        public InnoBuilder AddUninstallScript()
        {
            Code.CreateEntry(EmbeddedResourceExtensions.ReadResource("uninstall.pas"));
            return this;
        }

        /// <summary>
        /// Adds environment variable.
        /// </summary>
        /// <param name="environment">Environment value.</param>
        public InnoBuilder AddRxBimEnvironment(string environment)
        {
            var environmentRegKey = @$"{EnvironmentRegistryConstants.RxBimEnvironmentRegPath}\{{{{{_options.PackageGuid}}}";

            Registry.CreateEntry(RegistryKeys.HKCU, environmentRegKey)
                .ValueName(EnvironmentRegistryConstants.EnvironmentRegKeyName)
                .ValueType(ValueTypes.String)
                .ValueData(environment)
                .Flags(RegistryFlags.UninsDeleteKey);
            Registry.CreateEntry(RegistryKeys.HKCU, environmentRegKey)
                .ValueName(EnvironmentRegistryConstants.PluginNameRegKeyName)
                .ValueType(ValueTypes.String)
                .ValueData(_options.ProductProjectName)
                .Flags(RegistryFlags.UninsDeleteKey);

            return this;
        }

        /// <summary>
        /// Adds fonts files.
        /// </summary>
        /// <param name="outputProjDir">Output compile project directory.</param>
        public InnoBuilder AddFonts(AbsolutePath outputProjDir)
        {
            FillFonts(outputProjDir)
                .ForEach(font =>
                {
                    Files.CreateEntry(font, @"{autofonts}")
                        .FontInstall(GetFontName(font))
                        .Flags(FileFlags.OnlyIfDoesntExist | FileFlags.UninsNeverUninstall);
                });

            return this;
        }

        private static IEnumerable<string> FillFonts(string path) =>
            Directory.EnumerateFiles(path, "*.ttf", SearchOption.AllDirectories).ToList();

        private static string GetFontName(string fontPath)
        {
            using var fontStream = new FileStream(fontPath, FileMode.Open);

#pragma warning disable CA1416
            return LoadFontFamily(fontStream).Name;
#pragma warning restore CA1416
        }

        /// <summary>
        /// Reads Font file as stream, because PrivateFontCollection.Dispose() does not work.
        /// </summary>
        /// <param name="stream">Font file stream.</param>
        private static FontFamily LoadFontFamily(Stream stream)
        {
            var buffer = new byte[stream.Length];
            _ = stream.Read(buffer, 0, buffer.Length);
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