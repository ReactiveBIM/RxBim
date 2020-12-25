namespace PikTools.Di
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using SimpleInjector;

    /// <summary>
    /// DiWrapper
    /// </summary>
    public abstract class DiConfigurator<T> : IDiConfigurator<T>
        where T : IPluginConfiguration
    {
        /// <summary>
        /// ctor
        /// </summary>
        protected DiConfigurator()
        {
            Container = new Container();
            Container.Options.EnableAutoVerification = false;
        }

        /// <summary>
        /// DI контейнер
        /// </summary>
        public Container Container { get; }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="assembly">сборка для поиска зависимостей</param>
        public virtual void Configure(Assembly assembly)
        {
            ConfigureBaseDependencies();
            ConfigureAdditionalDependencies(assembly);
            AddConfigurations(assembly);
        }

        /// <summary>
        /// Конфигурирование основных зависимостей Revit
        /// </summary>
        protected abstract void ConfigureBaseDependencies();

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
            Container.Register<IConfiguration>(() =>
            {
                if (Container.GetCurrentRegistrations()
                    .Any(x => x.ServiceType == typeof(Func<ConfigurationBuilder, IConfiguration>)))
                {
                    var userConfigurationBuilder = Container.GetInstance<Func<ConfigurationBuilder, IConfiguration>>();
                    configurationBuilder.AddConfiguration(userConfigurationBuilder(new ConfigurationBuilder()));
                }

                return configurationBuilder.Build();
            }, Lifestyle.Singleton);
        }
    }
}