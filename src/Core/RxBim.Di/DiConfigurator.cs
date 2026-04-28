namespace RxBim.Di
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Extensions;
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
        protected IServiceCollection Services { get; } = new ServiceCollection();

        /// <summary>
        /// Configures dependencies in the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="assembly">An assembly for dependency scanning.</param>
        public void Configure(Assembly assembly)
        {
            ConfigureBaseDependencies();
            AddConfigurations(assembly);
            ConfigureAdditionalDependencies(assembly);
        }

        /// <summary>
        /// Builds container.
        /// </summary>
        public IServiceProvider Build()
        {
            return Services.BuildServiceProvider();
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
                .Where(x => typeof(TConfiguration).IsAssignableFrom(x))
                .Select(Activator.CreateInstance)
                .Cast<TConfiguration>();

            foreach (var cfg in configs)
            {
                cfg.Configure(Services);
            }
        }

        private void AddConfigurations(Assembly assembly)
        {
            AddUserConfigurations(() => CreateBaseConfigurationBuilder(assembly));
        }

        private IConfigurationBuilder CreateBaseConfigurationBuilder(Assembly assembly)
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

        private void AddUserConfigurations(Func<IConfigurationBuilder> builderFactory)
        {
            Services.AddSingleton<IConfiguration>(sp =>
            {
                var configurationBuilder = builderFactory();

                foreach (var addConfig in sp.GetServices<Action<IServiceProvider, IConfigurationBuilder>>())
                    addConfig(sp, configurationBuilder);

                return configurationBuilder.Build();
            });
        }
    }
}