namespace RxBim.Di
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
        public virtual void Configure(Assembly assembly)
        {
            ConfigureBaseDependencies();
            ConfigureAdditionalDependencies(assembly);
            AddConfigurations(assembly);
        }

        /// <summary>
        /// Configures base dependencies.
        /// </summary>
        protected abstract void ConfigureBaseDependencies();

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
            Container.Services.AddSingleton<IConfiguration>(
                provider =>
                {
                    foreach (var addConfig in provider.GetServices<Action<IServiceProvider, IConfigurationBuilder>>())
                        addConfig(provider, configurationBuilder);

                    return configurationBuilder.Build();
                });
        }
    }
}