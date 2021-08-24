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
    public abstract class DiConfigurator<T> : IDiConfigurator<T>
        where T : IPluginConfiguration
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
            var resolverType = assembly.GetTypes()
                .SingleOrDefault(x => x.GetInterfaces().Any(i => i.Name == nameof(IContainerResolver)));
            if (resolverType != null)
            {
                var resolver = (IContainerResolver)Activator.CreateInstance(resolverType);
                return resolver.Resolve();
            }

            return new DefaultContainer();
        }

        private void ConfigureAdditionalDependencies(Assembly assembly)
        {
            var configs = assembly.GetTypes()
                .Where(x => x.GetInterface(typeof(T).Name) != null)
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