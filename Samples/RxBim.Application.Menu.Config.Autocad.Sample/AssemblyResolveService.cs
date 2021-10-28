namespace RxBim.Application.Menu.Config.Autocad.Sample
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Сервис для исправления проблем подгрузки сборок
    /// </summary>
    public static class AssemblyResolveService
    {
        private static readonly HashSet<string> AssemblyNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static bool _eventsHandlerAdded;

        /// <summary>
        /// Добавляет сборки в сервис
        /// </summary>
        /// <param name="assemblyNames">Названия сборок</param>
        public static void AddAssemblies(params string[] assemblyNames)
        {
            foreach (var assemblyName in assemblyNames)
            {
                AssemblyNames.Add(assemblyName);
            }

            AddEventHandler();
        }

        private static void AddEventHandler()
        {
            if (_eventsHandlerAdded)
                return;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
            _eventsHandlerAdded = true;
        }

        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = GetAssemblyNameForResolve(args.Name);
            return assemblyName != null ? ResolveAssembly(assemblyName) : null;
        }

        private static string GetAssemblyNameForResolve(string missingAssemblyName)
        {
            if (missingAssemblyName.IndexOf("resources", StringComparison.OrdinalIgnoreCase) >= 0)
                return null;

            return AssemblyNames
                .FirstOrDefault(x => missingAssemblyName.StartsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        private static Assembly ResolveAssembly(string assemblyName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyLocation = Path.GetDirectoryName(assembly.Location);

            if (assemblyLocation == null)
                return null;

            var fileName = Path.Combine(assemblyLocation, assemblyName + ".dll");
            return File.Exists(fileName) ? Assembly.LoadFrom(fileName) : null;
        }
    }
}