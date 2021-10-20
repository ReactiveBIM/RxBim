namespace RxBim.Di
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// DiWrapper
    /// </summary>
    public abstract class DiConfigurator<TConfiguration> : IDiConfigurator<TConfiguration>
        where TConfiguration : IPluginConfiguration
    {
        /// <summary>
        /// DI контейнер
        /// </summary>
        public IContainer Container { get; private set; }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="assembly">сборка для поиска зависимостей</param>
        public virtual void Configure(Assembly assembly)
        {
            Container = CreateContainer(assembly);
            ConfigureBaseDependencies();
            ConfigureAdditionalDependencies(assembly);
            AddConfigurations(assembly);
        }

        /// <summary>
        /// Конфигурирование основных зависимостей Revit
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

        private IConfigurationBuilder GetBaseConfigurationBuilder(Assembly assembly)
        {
            var basePath = Path.GetDirectoryName(assembly.Location);
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .SetFileLoadExceptionHandler(ctx => ctx.Ignore = true)
                .AddJsonFile($"appsettings.{assembly.GetName().Name}.json", true);
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