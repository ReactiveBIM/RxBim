#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
namespace RxBim.Shared
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The resolver of dependent assemblies.
    /// </summary>
    public class AssemblyResolver : IDisposable
    {
        private readonly IEnumerable<Dll> _dlls;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="assembly">assembly.</param>
        public AssemblyResolver(Assembly assembly)
        {
            var dir = Path.GetDirectoryName(assembly.Location) ?? string.Empty;
            _dlls = GetDlls(dir, SearchOption.TopDirectoryOnly);

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomainOnAssemblyResolve;
        }

        private Assembly? CurrentDomainOnAssemblyResolve(object o, ResolveEventArgs args)
        {
            var dll = _dlls.FirstOrDefault(f => f.IsResolve(args.Name));
            if (dll == null)
                return null;

            try
            {
                return dll.LoadAssembly();
            }
            catch
            {
                // ignored
            }

            return null;
        }

        private IEnumerable<Dll> GetDlls(string dllFolder, SearchOption mode)
        {
            return Directory
                .EnumerateFiles(dllFolder, "*.dll", mode)
                .Select(dllFile => new Dll(dllFile));
        }

        private class Dll
        {
            public Dll(string dllFile)
            {
                DllFile = dllFile;
                DllName = Path.GetFileNameWithoutExtension(dllFile);
            }

            private string DllFile { get; }

            private string DllName { get; }

            public bool IsResolve(string dllRequest)
            {
                return dllRequest.StartsWith($"{DllName},", StringComparison.OrdinalIgnoreCase);
            }

            public Assembly LoadAssembly()
            {
                return Assembly.LoadFile(DllFile);
            }
        }
    }
}