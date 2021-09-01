#pragma warning disable SA1600
namespace RxBim.Logs.Settings.Configuration.Assemblies
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.Extensions.DependencyModel;

    internal abstract class AssemblyFinder
    {
        public static AssemblyFinder Auto()
        {
            // Need to check `Assembly.GetEntryAssembly()` first because 
            // `DependencyContext.Default` throws an exception when `Assembly.GetEntryAssembly()` returns null
            if (Assembly.GetEntryAssembly() != null && DependencyContext.Default != null)
            {
                return new DependencyContextAssemblyFinder(DependencyContext.Default);
            }
            return new DllScanningAssemblyFinder();
        }

        public static AssemblyFinder ForSource(ConfigurationAssemblySource configurationAssemblySource)
        {
            return configurationAssemblySource switch
            {
                ConfigurationAssemblySource.UseLoadedAssemblies => Auto(),
                ConfigurationAssemblySource.AlwaysScanDllFiles => new DllScanningAssemblyFinder(),
                _ => throw new ArgumentOutOfRangeException(nameof(configurationAssemblySource), configurationAssemblySource, null),
            };
        }

        public static AssemblyFinder ForDependencyContext(DependencyContext dependencyContext)
        {
            return new DependencyContextAssemblyFinder(dependencyContext);
        }

        public abstract IReadOnlyList<AssemblyName> FindAssembliesContainingName(string nameToFind);

        protected static bool IsCaseInsensitiveMatch(string text, string textToFind)
        {
            return text != null && text.ToLowerInvariant().Contains(textToFind.ToLowerInvariant());
        }
    }
}
