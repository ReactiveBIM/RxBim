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
    /// Contains DI Container Extensions for Ribbon Menu.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a plugin ribbon menu from an action.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="builder">The ribbon menu builder.</param>
        /// <param name="assembly">
        /// Menu definition assembly.
        /// Used to get the command type from the command type name
        /// and to define the root directory for relative icon paths.
        /// </param>
        public static void AddMenu<TBuilder>(
            this IServiceCollection services,
            Action<IRibbonBuilder> builder,
            Assembly assembly)
            where TBuilder : class, IRibbonMenuBuilder
        {
            services.AddBuilder<TBuilder>(assembly);
            services.AddSingleton(() =>
            {
                var ribbon = new RibbonBuilder();
                builder(ribbon);
                return ribbon.Build();
            });
            services.AddMenuBuilder();
        }

        /// <summary>
        /// Adds a plugin ribbon menu from configuration.
        /// </summary>
        /// <param name="services">DI container.</param>
        /// <param name="config">Plugin configuration.</param>
        /// <param name="assembly">
        /// Menu definition assembly.
        /// Used to get the command type from the command type name
        /// and to define the root directory for relative icon paths.
        /// </param>
        public static void AddMenu<TBuilder>(
            this IServiceCollection services,
            IConfiguration? config,
            Assembly assembly)
            where TBuilder : class, IRibbonMenuBuilder
        {
            services.AddBuilder<TBuilder>(assembly);
            services.AddSingleton(sp => GetMenuConfiguration(sp, config));
            services.AddMenuBuilder();
        }

        /// <summary>
        /// Implementation of building plugin ribbon.
        /// </summary>
        /// <param name="serviceProvider">Service locator.</param>
        internal static void BuildRibbonMenu(this IServiceProvider serviceProvider)
        {
            try
            {
                var builder = serviceProvider.GetService<IRibbonMenuBuilder>();
                var ribbonConfiguration = serviceProvider.GetService<Ribbon>();
                builder.BuildRibbonMenu(ribbonConfiguration);
            }
            catch (Exception e)
            {
                throw new MethodCallerException("Failed to build ribbon", e);
            }
        }

        private static void AddBuilder<T>(this IServiceCollection services, Assembly assembly)
            where T : class, IRibbonMenuBuilder
        {
            services
                .AddSingleton(() => new MenuData { MenuAssembly = assembly })
                .Scan(scan => scan
                    .FromExecutingAssembly()
                    .AddClasses(classes => classes.AssignableTo<IItemStrategy>())
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime()
                    .AddClasses(classes => classes.AssignableTo<IItemFromConfigStrategy>())
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime())
                .AddSingleton<IRibbonMenuBuilder, T>();
        }

        private static void AddMenuBuilder(this IServiceCollection services)
        {
            services.Decorate(typeof(IMethodCaller<>), typeof(MenuBuilderMethodCaller<>));
        }

        private static Ribbon GetMenuConfiguration(IServiceProvider serviceProvider, IConfiguration? cfg)
        {
            cfg ??= serviceProvider.GetService<IConfiguration>();
            var strategies = serviceProvider.GetServices<IItemFromConfigStrategy>().ToList();

            var builder = new RibbonBuilder();
            builder.LoadFromConfig(cfg, strategies);

            return builder.Build();
        }
    }
}