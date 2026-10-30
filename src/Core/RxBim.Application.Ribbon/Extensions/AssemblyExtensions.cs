namespace RxBim.Application.Ribbon
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
#if NETCOREAPP
    using System.Runtime.Loader;
#endif

    /// <summary>
    /// Extensions for the <see cref="Assembly"/> class.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Returns class type.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="typeName">The class type name.</param>
        /// <exception cref="ArgumentException">The Type name is invalid.</exception>
        public static Type GetTypeByName(this Assembly assembly, string typeName)
        {
            // todo use regex
            var strings = typeName.Split([","], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToArray();

            var type = strings.Length switch
            {
                1 => assembly.GetType(typeName),
                _ => LoadCommandAssembly(assembly, strings[1]).GetType(strings[0])
            };

            if (type is null)
                throw new ArgumentException($"Failed to get type by name: {typeName}", nameof(typeName));

            return type;
        }

        /// <summary>
        /// Returns URI for the support file.
        /// </summary>
        /// <param name="assembly">The base assembly. Used to get the root directory.</param>
        /// <param name="fullOrRelativePath">Full or relative path to support file.</param>
        public static Uri? TryGetSupportFileUri(this Assembly assembly, string fullOrRelativePath)
        {
            var path = Path.IsPathRooted(fullOrRelativePath) && File.Exists(fullOrRelativePath)
                ? fullOrRelativePath
                : Path.Combine(GetAssemblyDirectory(assembly), fullOrRelativePath);

            return File.Exists(path) ? new Uri(path, UriKind.Absolute) : null;
        }

        private static Assembly LoadCommandAssembly(Assembly menuAssembly, string commandAssemblyName)
        {
            var path = Path.Combine(menuAssembly.GetAssemblyDirectory(), commandAssemblyName + ".dll");
#if NETCOREAPP
            // Keep ribbon metadata and WPF windows in the same context as the application.
            var context = AssemblyLoadContext.GetLoadContext(menuAssembly);

            if (context is not null && context != AssemblyLoadContext.Default)
                return context.LoadFromAssemblyPath(path);
#endif

            return Assembly.LoadFrom(path);
        }

        private static string GetAssemblyDirectory(this Assembly assembly)
        {
            return Path.GetDirectoryName(assembly.Location) ??
                   throw new InvalidOperationException($"Can't get path to assembly: {assembly.FullName}!");
        }
    }
}