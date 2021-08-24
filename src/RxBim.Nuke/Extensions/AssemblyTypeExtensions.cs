namespace RxBim.Nuke.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Tooling;
    using global::Nuke.Common.Tools.SignTool;
    using Models;
    using static Constants;
    using static Helpers.AssemblyScanner;

    /// <summary>
    /// Расширения для типов сборок
    /// </summary>
    public static class AssemblyTypeExtensions
    {
        /// <summary>
        /// Sign assemblies
        /// </summary>
        /// <param name="assemblyTypes">Assemly types</param>
        /// <param name="outputDirectory">Output directory</param>
        /// <param name="cert">Certificate path</param>
        /// <param name="keyContainer">Private key</param>
        /// <param name="csp">CSP containing</param>
        /// <param name="digestAlgorithm">Digest algorithm</param>
        /// <param name="timestampServerUrl">Timestamp server URL</param>
        public static void SignAssemblies(
            this IEnumerable<AssemblyType> assemblyTypes,
            AbsolutePath outputDirectory,
            AbsolutePath cert,
            string keyContainer,
            string csp,
            string digestAlgorithm,
            string timestampServerUrl)
        {
            cert = cert ??
                   throw new ArgumentException("Didn't set certificate");

            keyContainer = keyContainer ??
                       throw new ArgumentException("Didn't set private key container");
            csp = csp ??
                    throw new ArgumentException("Didn't set CSP containing");
            digestAlgorithm = digestAlgorithm ??
                              throw new ArgumentException("Didn't set digest algorithm");
            timestampServerUrl = timestampServerUrl ??
                                 throw new ArgumentException("Didn't set timestamp server URL");

            var filesNames = assemblyTypes
                .Select(t => (outputDirectory / $"{t.AssemblyName}.dll").ToString())
                .Distinct()
                .ToArray();

            var settings = new SignToolSettings()
                .SetCsp(csp)
                .SetKeyContainer(keyContainer)
                .SetFile(cert)
                .SetFiles(filesNames)
                .SetTimestampServerDigestAlgorithm(digestAlgorithm)
                .SetRfc3161TimestampServerUrl(timestampServerUrl);

            if (!settings.HasValidToolPath())
            {
                var programFilesPath =
                    (AbsolutePath)Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                settings = settings
                    .SetProcessToolPath(
                        programFilesPath / "Microsoft SDKs" / "ClickOnce" / "SignTool" / "signtool.exe");
            }

            Logger.Info($"ToolPath: {settings.ProcessToolPath}");

            SignToolTasks.SignTool(settings);
        }

        /// <summary>
        /// Тип является типом плагина
        /// </summary>
        /// <param name="type">Тип</param>
        public static bool IsPluginType(this AssemblyType type)
        {
            return type.BaseTypeName == RxBimCommand ||
                   type.BaseTypeName == RxBimApplication;
        }

        /// <summary>
        /// Возвращает типы плагинов для сборки
        /// </summary>
        /// <param name="binPath">Путь к сборке</param>
        public static List<AssemblyType> GetPluginTypes(this AbsolutePath binPath)
        {
            var assemblyTypes = Scan(binPath)
                .Where(x => x.IsPluginType())
                .ToList();
            return assemblyTypes;
        }
    }
}