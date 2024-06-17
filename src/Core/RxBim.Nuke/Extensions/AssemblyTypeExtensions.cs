namespace RxBim.Nuke.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Nuke.Common.IO;
    using Models;
    using static Constants;
    using static Helpers.AssemblyScanner;

    /// <summary>
    /// The <see cref="AssemblyType"/> class extensions.
    /// </summary>
    public static class AssemblyTypeExtensions
    {
        /// <summary>
        /// Signs assemblies.
        /// </summary>
        /// <param name="assemblyTypes">Assembly types.</param>
        /// <param name="outputDirectory">Output directory path.</param>
        /// <param name="cert">Certificate path.</param>
        /// <param name="keyContainer">Private key.</param>
        /// <param name="csp">CSP containing.</param>
        /// <param name="digestAlgorithm">Digest algorithm.</param>
        /// <param name="timestampServerUrl">Timestamp server URL.</param>
        public static void SignAssemblies(
            this IEnumerable<AssemblyType> assemblyTypes,
            AbsolutePath outputDirectory,
            AbsolutePath cert,
            string keyContainer,
            string csp,
            string digestAlgorithm,
            string timestampServerUrl)
        {
            var filesNames = assemblyTypes
                .Select(t => (outputDirectory / $"{t.AssemblyName}.dll").ToString())
                .Distinct()
                .ToArray();

            filesNames.SignFiles(
                cert,
                keyContainer,
                csp,
                digestAlgorithm,
                timestampServerUrl);
        }

        /// <summary>
        /// Is assembly type plugin.
        /// </summary>
        /// <param name="type">The <see cref="AssemblyType"/>.</param>
        public static bool IsPluginType(this AssemblyType type)
        {
            return type.BaseTypeNames.Contains(RxBimCommand) ||
                   type.BaseTypeNames.Contains(RxBimApplication);
        }

        /// <summary>
        /// Gets <see cref="AssemblyType"/> from build path.
        /// </summary>
        /// <param name="binPath">Build path.</param>
        public static IEnumerable<AssemblyType> GetPluginTypes(this AbsolutePath binPath)
        {
            return Scan(binPath)
                .Where(x => x.IsPluginType())
                .ToList();
        }

        /// <summary>
        /// Gets type name of a <see cref="PluginType"/>.
        /// </summary>
        /// <param name="type">The plugin type.</param>
        public static PluginType ToPluginType(this AssemblyType type)
        {
            if (type.BaseTypeNames.Contains(RxBimCommand))
                return PluginType.Command;

            if (type.BaseTypeNames.Contains(RxBimApplication))
                return PluginType.Application;

            throw new NotSupportedException();
        }
    }
}