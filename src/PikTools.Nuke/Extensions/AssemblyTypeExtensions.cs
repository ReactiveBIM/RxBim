namespace PikTools.Nuke.Extensions
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
        /// Подписывает сборки
        /// </summary>
        /// <param name="assemblyTypes">Типы сборок</param>
        /// <param name="outputDirectory">Папка, где расположены сборки</param>
        /// <param name="cert">Путь к сертификату</param>
        /// <param name="password">Пароль</param>
        /// <param name="digestAlgorithm">Алгоритм</param>
        /// <param name="timestampServerUrl">Сервер url</param>
        public static void SignAssemblies(
            this IEnumerable<AssemblyType> assemblyTypes,
            AbsolutePath outputDirectory,
            AbsolutePath cert,
            string password,
            string digestAlgorithm,
            string timestampServerUrl)
        {
            cert = cert ??
                   throw new ArgumentException("Не указал сертификат");

            password = password ??
                       throw new ArgumentException("Не указан пароль сертификата");
            digestAlgorithm = digestAlgorithm ??
                              throw new ArgumentException("Не указан алгоритм сертификата");
            timestampServerUrl = timestampServerUrl ??
                                 throw new ArgumentException("Не указан сервер проверки сертификата");

            var filesNames = assemblyTypes
                .Select(t => (outputDirectory / $"{t.AssemblyName}.dll").ToString())
                .Distinct()
                .ToArray();

            var settings = new SignToolSettings()
                .SetFileDigestAlgorithm(digestAlgorithm)
                .SetFile(cert)
                .SetFiles(filesNames)
                .SetPassword(password)
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
            return type.BaseTypeName == PikToolsCommand ||
                   type.BaseTypeName == PikToolsApplication;
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