namespace RxBim.Nuke.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Nuke.Common.IO;
    using Models;
    using static Constants;
    using static Helpers.AssemblyScanner;

    /// <summary>
    /// <see cref="AssemblyType"/> extensions
    /// </summary>
    public static class AssemblyTypeExtensions
    {
        /// <summary>
        /// Sign assemblies
        /// </summary>
        /// <param name="assemblyTypes">Assembly types</param>
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
        /// Is assembly type plugin
        /// </summary>
        /// <param name="type"><see cref="AssemblyType"/></param>
        public static bool IsPluginType(this AssemblyType type)
        {
            return type.BaseTypeName == RxBimCommand ||
                   type.BaseTypeName == RxBimApplication;
        }

        /// <summary>
        /// Get <see cref="AssemblyType"/> from build path
        /// </summary>
        /// <param name="binPath">Build path</param>
        public static List<AssemblyType> GetPluginTypes(this AbsolutePath binPath)
        {
            var assemblyTypes = Scan(binPath)
                .Where(x => x.IsPluginType())
                .ToList();
            return assemblyTypes;
        }
    }
}