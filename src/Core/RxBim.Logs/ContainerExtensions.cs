﻿namespace RxBim.Logs
{
    using System;
    using Di;
    using Enrichers;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyModel;
    using Serilog;
    using Serilog.Events;

    /// <summary>
    /// A DI container extensions.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Adds logs in a <paramref name="container"/>.
        /// </summary>
        /// <param name="container">A DI container.</param>
        /// <param name="cfg">A configuration.</param>
        /// <param name="addEnricher">An action for additional logs configuration.</param>
        public static void AddLogs(
            this IContainer container,
            IConfiguration? cfg = null,
            Action<IContainer, LoggerConfiguration>? addEnricher = null)
        {
            RegisterLogger(container, cfg, addEnricher);
            container.Decorate(typeof(IMethodCaller<>), typeof(LoggedMethodCaller<>));
        }

        private static void RegisterLogger(
            IContainer container,
            IConfiguration? cfg,
            Action<IContainer, LoggerConfiguration>? addEnricher)
        {
            container.AddSingleton(
                () =>
                {
                    if (cfg == null)
                    {
                        TryGetConfigurationFromContainer(container, ref cfg);
                    }

                    return CreateLogger(cfg, container, addEnricher);
                });
        }

        private static void TryGetConfigurationFromContainer(IContainer container, ref IConfiguration? cfg)
        {
            try
            {
                cfg = container.GetService<IConfiguration>();
            }
            catch
            {
                // ignore
            }
        }

        private static ILogger CreateLogger(
            IConfiguration? cfg,
            IContainer container,
            Action<IContainer, LoggerConfiguration>? addEnricher)
        {
            var config = new LoggerConfiguration();
            if (cfg != null)
            {
                var dependencyContext = DependencyContext.Load(typeof(LoggedMethodCaller<>).Assembly);
                config.ReadFrom.Configuration(cfg, dependencyContext);
            }
            else
            {
                config
                    .WriteTo.Debug()
                    .WriteTo.File("log.txt", LogEventLevel.Information, fileSizeLimitBytes: 1024 * 10);
            }

            AddLogEnrichers(container, config, addEnricher);

            return config.CreateLogger();
        }

        private static void AddLogEnrichers(
            IContainer container,
            LoggerConfiguration config,
            Action<IContainer, LoggerConfiguration>? addEnricher)
        {
            config
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.With<ExceptionEnricher>()
                .Enrich.With<OsEnricher>();

            addEnricher?.Invoke(container, config);
        }
    }
}