namespace RxBim.Nuke.Extensions
{
    using System;
    using global::Nuke.Common;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Tooling;
    using global::Nuke.Common.Tools.SignTool;

    /// <summary>
    /// Sign files extensions
    /// </summary>
    public static class SignExtensions
    {
        /// <summary>
        /// Sign files
        /// </summary>
        /// <param name="filesPath">Files path</param>
        /// <param name="cert">Certificate path</param>
        /// <param name="keyContainer">Private key</param>
        /// <param name="csp">CSP containing</param>
        /// <param name="digestAlgorithm">Digest algorithm</param>
        /// <param name="timestampServerUrl">Timestamp server URL</param>
        public static void SignFiles(
            this string[] filesPath,
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

            var settings = new SignToolSettings()
                .SetCsp(csp)
                .SetKeyContainer(keyContainer)
                .SetFile(cert)
                .SetFiles(filesPath)
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
        /// Sign file
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="cert">Certificate path</param>
        /// <param name="keyContainer">Private key</param>
        /// <param name="csp">CSP containing</param>
        /// <param name="digestAlgorithm">Digest algorithm</param>
        /// <param name="timestampServerUrl">Timestamp server URL</param>
        public static void SignFile(
            this string filePath,
            AbsolutePath cert,
            string keyContainer,
            string csp,
            string digestAlgorithm,
            string timestampServerUrl)
        {
            SignFiles(
                new[] { filePath },
                cert,
                keyContainer,
                csp,
                digestAlgorithm,
                timestampServerUrl);
        }
    }
}