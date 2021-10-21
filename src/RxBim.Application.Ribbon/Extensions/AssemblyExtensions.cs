namespace RxBim.Application.Ribbon.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extensions for <see cref="Assembly"/>
    /// </summary>
    internal static class AssemblyExtensions
    {
        /// <summary>
        /// Returns class type
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <param name="typeName">Class type name</param>
        /// <exception cref="ArgumentException">Type name is invalid</exception>
        public static Type GetType(this Assembly assembly, string typeName)
        {
            var strings = typeName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();

            return strings.Length switch
            {
                1 => assembly.GetType(typeName), 2 => Assembly
                    .LoadFrom(Path.Combine(assembly.GetAssemblyDirectory(), strings[1] + ".dll"))
                    .GetType(strings[0]),
                _ => throw new ArgumentException()
            };
        }

        /// <summary>
        /// Computes the full path to the support file
        /// </summary>
        /// <param name="assembly">Base assembly. Used to get the root directory if the path is relative</param>
        /// <param name="fullOrRelativePath">Full or relative path to support file</param>
        /// <param name="fullPath">Full path to the support file</param>
        /// <returns>Returns true if the path was found, otherwise returns false</returns>
        public static bool TryGetSupportFileFullPath(this Assembly assembly, string fullOrRelativePath, out string fullPath)
        {
            if (File.Exists(fullOrRelativePath))
            {
                fullPath = fullOrRelativePath;
                return true;
            }

            fullPath = Path.Combine(GetAssemblyDirectory(assembly), fullOrRelativePath);
            return File.Exists(fullPath);
        }

        private static string GetAssemblyDirectory(this Assembly assembly)
        {
            return Path.GetDirectoryName(assembly.Location) ??
                   throw new InvalidOperationException($"Can't get path to assembly: {assembly.FullName}!");
        }
    }
}