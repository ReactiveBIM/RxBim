namespace RxBim.Di
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using RxBim.Di.Abstraction;

    /// <summary>
    /// Base DI configurator.
    /// </summary>
    public abstract class DiConfigurator<TConfiguration>
        where TConfiguration : IPluginConfiguration
    {
        /// <summary>
        /// DI container.
        /// </summary>
        public IContainer Container { get; } = new DiContainer();

        /// <summary>
        /// Configures dependencies in the <see cref="IContainer.Services"/>.
        /// </summary>
        /// <param name="assembly">An assembly for dependency scanning.</param>
        public void Configure(Assembly assembly)
        {
            ConfigureBaseDependencies();
            ConfigureAdditionalDependencies(assembly);
            AddConfigurations(assembly);
            AddServiceLocator();
            InitializeCriticalServices();
        }

        /// <summary>
        /// Configures base dependencies.
        /// </summary>
        protected abstract void ConfigureBaseDependencies();

        /// <summary>
        /// Configure additional assembly based dependencies.
        /// </summary>
        /// <param name="assembly">An assembly for dependency scanning.</param>
        protected virtual void ConfigureAdditionalDependencies(Assembly assembly)
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
            var basePath = Path.GetDirectoryName(assembly.Location)
                           ?? throw new InvalidOperationException(
                               $"Can't find directory for assembly '{assembly.FullName}'!");
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
                var serviceLocator = Container.GetService<IServiceLocator>();
                foreach (var addConfig in serviceLocator.GetServices<Action<IContainer, IConfigurationBuilder>>())
                    addConfig(Container, configurationBuilder);

                return configurationBuilder.Build();
            });
        }

        private void InitializeCriticalServices()
        {
            // Ensure that critical services that must be launched as soon as possible are created
            _ = Container.ServiceProvider.GetServices<ICriticalInitializationService>();
        }
    }
}