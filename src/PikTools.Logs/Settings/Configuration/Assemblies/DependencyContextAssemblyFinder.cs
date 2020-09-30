#pragma warning disable
namespace PikTools.Logs.Settings.Configuration.Assemblies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyModel;

    sealed class DependencyContextAssemblyFinder : AssemblyFinder
    {
        readonly DependencyContext _dependencyContext;

        public DependencyContextAssemblyFinder(DependencyContext dependencyContext)
        {
            _dependencyContext = dependencyContext ?? throw new ArgumentNullException(nameof(dependencyContext));
        }

        public override IReadOnlyList<AssemblyName> FindAssembliesContainingName(string nameToFind)
        {
            var query = from library in _dependencyContext.RuntimeLibraries
                        where IsReferencingSerilog(library)
                        from assemblyName in library.GetDefaultAssemblyNames(_dependencyContext)
                        where IsCaseInsensitiveMatch(assemblyName.Name, nameToFind)
                        select assemblyName;

            return query.ToList().AsReadOnly();
            
            static bool IsReferencingSerilog(Library library)
            {
                return library.Dependencies.Any(dependency => dependency.Name.Equals("serilog", StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
