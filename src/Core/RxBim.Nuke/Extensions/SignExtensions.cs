namespace RxBim.Nuke.Extensions
{
    using System;
    using global::Nuke.Common.IO;
    using global::Nuke.Common.Tooling;
    using global::Nuke.Common.Tools.SignTool;
    using Serilog;

    /// <summary>
    /// Extension methods helps add digital sign to files.
    /// </summary>
    public static class SignExtensions
    {
        /// <summary>
        /// Signs files.
        /// </summary>
        /// <param name="filesPath">Files path.</param>
        /// <param name="cert">A certificate path.</param>
        /// <param name="keyContainer">The private key of the certificate.</param>
        /// <param name="csp">CSP containing.</param>
        /// <param name="digestAlgorithm">Digest algorithm.</param>
        /// <param name="timestampServerUrl">Timestamp server URL.</param>
        public static void SignFiles(
            this string[] filesPath,
            AbsolutePath cert,
            string keyContainer,
            string csp,
            string digestAlgorithm,
            string timestampServerUrl)
        {
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

            Log.Information("ToolPath: {Path}", settings.ProcessToolPath);

            SignToolTasks.SignTool(settings);
        }

        /// <summary>
        /// Adds digital sign to a file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="cert">The certificate path.</param>
        /// <param name="keyContainer">The private key of the certificate.</param>
        /// <param name="csp">CSP containing.</param>
        /// <param name="digestAlgorithm">Digest algorithm.</param>
        /// <param name="timestampServerUrl">Timestamp server URL.</param>
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