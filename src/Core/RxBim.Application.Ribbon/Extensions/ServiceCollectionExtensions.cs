namespace RxBim.Application.Ribbon
{
    using System;
    using System.Linq;
    using System.Reflection;
    using ConfigurationBuilders;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <see cref="IServiceCollection"/> Extensions for Ribbon Menu.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a plugin ribbon menu from an action.
        /// </summary>
        /// <param name="container">DI container.</param>
        /// <param name="builder">The ribbon menu builder.</param>
        /// <param name="assembly">
        /// Menu definition assembly.
        /// Used to get the command type from the command type name
        /// and to define the root directory for relative icon paths.
        /// </param>
        public static void AddMenu<TBuilder>(
            this IContainer container,
            Action<IRibbonBuilder> builder,
            Assembly assembly)
            where TBuilder : class, IRibbonMenuBuilder
        {
            container.AddBuilder<TBuilder>(assembly);
            container.Services.AddSingleton(_ =>
            {
                var ribbon = new RibbonBuilder();
                builder(ribbon);
                return ribbon.Build();
            });
            container.DecorateContainer();
        }

        /// <summary>
        /// Adds a plugin ribbon menu from configuration.
        /// </summary>
        /// <param name="container">DI container.</param>
        /// <param name="config">Plugin configuration.</param>
        /// <param name="assembly">
        /// Menu definition assembly.
        /// Used to get the command type from the command type name
        /// and to define the root directory for relative icon paths.
        /// </param>
        public static void AddMenu<TBuilder>(
            this IContainer container,
            IConfiguration? config,
            Assembly assembly)
            where TBuilder : class, IRibbonMenuBuilder
        {
            container.AddBuilder<TBuilder>(assembly);
            container.Services.AddSingleton(provider => GetMenuConfiguration(provider, config));
            container.DecorateContainer();
        }

        private static void AddBuilder<T>(this IContainer container, Assembly assembly)
            where T : class, IRibbonMenuBuilder
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            container
                .AddSingleton(() => new MenuData { MenuAssembly = assembly })
                .RegisterTypes<IItemFromConfigStrategy>(Lifetime.Singleton, thisAssembly)
                .RegisterTypes<IItemStrategy>(Lifetime.Singleton, thisAssembly)
                .AddSingleton<IRibbonMenuBuilder, T>();
        }

        private static void DecorateContainer(this IContainer services)
        {
            services.Decorate(typeof(IMethodCaller<>), typeof(MenuBuilderMethodCaller<>));
        }

        private static Ribbon GetMenuConfiguration(IServiceProvider provider, IConfiguration? cfg)
        {
            cfg ??= provider.GetRequiredService<IConfiguration>();
            var strategies = provider.GetServices<IItemFromConfigStrategy>().ToList();

            var builder = new RibbonBuilder();
            builder.LoadFromConfig(cfg, strategies);

            return builder.Build();
        }
    }
}