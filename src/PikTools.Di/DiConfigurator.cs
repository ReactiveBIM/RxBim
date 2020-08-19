namespace PikTools.Di
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using SimpleInjector;

    /// <summary>
    /// DiWrapper
    /// </summary>
    public abstract class DiConfigurator : IDiConfigurator
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
        public void Configure(Assembly assembly)
        {
            ConfigureBaseDependencies();
            ConfigureAdditionalDependencies(assembly);
        }

        /// <summary>
        /// Конфигурирование основных зависимостей Revit
        /// </summary>
        protected abstract void ConfigureBaseDependencies();

        private void ConfigureAdditionalDependencies(Assembly assembly)
        {
            var configs = assembly.GetTypes()
                .Where(x => x.GetInterface(nameof(IPluginConfiguration)) != null)
                .Select(Activator.CreateInstance)
                .Cast<IPluginConfiguration>();

            foreach (var cfg in configs)
            {
                cfg.Configure(Container);
            }
        }
    }
}