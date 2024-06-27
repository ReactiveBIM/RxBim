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
        /// Return dll names of given assemblies.
        /// </summary>
        /// <param name="assemblyTypes">Assembly types.</param>
        /// <param name="outputDirectory">Output directory path.</param>
        public static string[] GetDllNames(this IEnumerable<AssemblyType> assemblyTypes, AbsolutePath outputDirectory)
        {
            return assemblyTypes
                .Select(t => (outputDirectory / $"{t.AssemblyName}.dll").ToString())
                .Distinct()
                .ToArray();
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