namespace RxBim.Di
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Extensions;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Base DI configurator.
    /// </summary>
    public abstract class DiConfigurator<TConfiguration> : IDiConfigurator<TConfiguration>
        where TConfiguration : IPluginConfiguration
    {
        /// <summary>
        /// DI Container.
        /// </summary>
        public IContainer Container { get; private set; } = null!;

        /// <summary>
        /// Configures dependencies in the <see cref="Container"/>.
        /// </summary>
        /// <param name="assembly">An assembly for dependency scanning.</param>
        public virtual void Configure(Assembly assembly)
        {
            Container = CreateContainer(assembly);
            ConfigureBaseDependencies();
            ConfigureAdditionalDependencies(assembly);
            AddConfigurations(assembly);
            AddServiceLocator();
        }

        /// <summary>
        /// Configures base dependencies.
        /// </summary>
        protected abstract void ConfigureBaseDependencies();

        private IContainer CreateContainer(Assembly assembly)
        {
            // try to find a container resolver
            var resolverType = assembly.GetTypes()
                .SingleOrDefault(x => x.GetInterfaces().Any(i => i.Name == nameof(IContainerResolver)));
            if (resolverType != null)
            {
                var resolver = (IContainerResolver)Activator.CreateInstance(resolverType);
                return resolver.Resolve();
            }

            // if a container resolver not found then search an IContainer implementation in the assembly location
            return LoadDefaultContainer(assembly);
        }

        private IContainer LoadDefaultContainer(Assembly assembly)
        {
            var assemblyDir = Path.GetDirectoryName(assembly.Location);
            var paths = Directory.EnumerateFiles(assemblyDir!)
                .Where(x => Path.GetExtension(x).Equals(".dll", StringComparison.OrdinalIgnoreCase) &&
                            Path.GetFileName(x).StartsWith("RxBim.Di.", StringComparison.OrdinalIgnoreCase));

            var containerType = paths
                                    .Select(GetAssemblyOrLoad)
                                    .SelectMany(x => x.GetTypes())
                                    .FirstOrDefault(x => x.GetInterfaces().Any(i => i.Name == nameof(IContainer)))
                                ?? throw new DllNotFoundException("IContainer implementation not found");

            return (IContainer)Activator.CreateInstance(containerType);
        }

        private Assembly GetAssemblyOrLoad(string pathToDllFile)
        {
            var assemblyName = Path.GetFileNameWithoutExtension(pathToDllFile);
            var loadedAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(x => x.FullName.StartsWith($"{assemblyName},"));
            return loadedAssembly ?? Assembly.LoadFrom(pathToDllFile);
        }

        private void ConfigureAdditionalDependencies(Assembly assembly)
        {
            var configs = assembly.GetTypes()
                .Where(x => x.GetInterface(typeof(TConfiguration).Name) != null)
                .Select(Activator.CreateInstance)
                .Cast<IPluginConfiguration>();

            foreach (var cfg in configs)
            {
                cfg.Configure(Container);
            }
        }

        private void AddConfigurations(Assembly assembly)
        {
            var configurationBuilder = GetBaseConfigurationBuilder(assembly);
            AddUserConfigurations(configurationBuilder);
        }

        private void AddServiceLocator()
        {
            Container.AddInstance<IServiceLocator>(new ServiceLocator(Container));
        }

        private IConfigurationBuilder GetBaseConfigurationBuilder(Assembly assembly)
        {
            var basePath = Path.GetDirectoryName(assembly.Location)!;
            var configFile = $"appsettings.{assembly.GetName().Name}.json";
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .SetFileLoadExceptionHandler(ctx => ctx.Ignore = true)
                .AddJsonFile(configFile, true)
                .AddEnvironmentJsonFile(basePath, configFile);
        }

        private void AddUserConfigurations(IConfigurationBuilder configurationBuilder)
        {
            Container.AddSingleton<IConfiguration>(() =>
            {
                if (Container.GetCurrentRegistrations()
                    .Any(x => x.ServiceType == typeof(Func<ConfigurationBuilder, IConfiguration>)))
                {
                    var userConfigurationBuilder = Container.GetService<Func<ConfigurationBuilder, IConfiguration>>();
                    configurationBuilder.AddConfiguration(userConfigurationBuilder(new ConfigurationBuilder()));
                }

                return configurationBuilder.Build();
            });
        }
    }
}