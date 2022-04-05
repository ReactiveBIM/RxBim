namespace RxBim.Application.Ribbon.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extensions for the <see cref="Assembly"/> class.
    /// </summary>
    internal static class AssemblyExtensions
    {
        /// <summary>
        /// Returns class type.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="typeName">The class type name.</param>
        /// <exception cref="ArgumentException">The Type name is invalid.</exception>
        public static Type GetTypeFromName(this Assembly assembly, string typeName)
        {
            var strings = typeName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();

            var type = strings.Length switch
            {
                1 => assembly.GetType(typeName),
                _ => Assembly
                    .LoadFrom(Path.Combine(assembly.GetAssemblyDirectory(), strings[1] + ".dll"))
                    .GetType(strings[0])
            };

            if (type is null)
                throw new ArgumentException($"Failed to get type from name: {typeName}", nameof(typeName));

            return type;
        }

        /// <summary>
        /// Returns URI for the support file.
        /// </summary>
        /// <param name="assembly">The base assembly. Used to get the root directory.</param>
        /// <param name="fullOrRelativePath">Full or relative path to support file.</param>
        public static Uri? TryGetSupportFileUri(this Assembly assembly, string fullOrRelativePath)
        {
            string path = Path.IsPathRooted(fullOrRelativePath) && File.Exists(fullOrRelativePath)
                ? fullOrRelativePath
                : Path.Combine(GetAssemblyDirectory(assembly), fullOrRelativePath);

            return File.Exists(path) ? new Uri(path, UriKind.Absolute) : null;
        }

        private static string GetAssemblyDirectory(this Assembly assembly)
        {
            return Path.GetDirectoryName(assembly.Location) ??
                   throw new InvalidOperationException($"Can't get path to assembly: {assembly.FullName}!");
        }
    }
}