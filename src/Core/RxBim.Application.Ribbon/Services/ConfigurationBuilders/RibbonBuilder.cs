namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Represents a ribbon builder.
    /// </summary>
    public class RibbonBuilder : IRibbonBuilder
    {
        /// <summary>
        /// Building ribbon.
        /// </summary>
        public Ribbon Ribbon { get; } = new();

        /// <inheritdoc />
        public IRibbonBuilder Tab(string title, Action<ITabBuilder> tab)
        {
            CreateTab(title, tab);
            return this;
        }

        /// <inheritdoc />
        public IRibbonBuilder SetDisplayVersion(bool enable)
        {
            Ribbon.DisplayVersion = enable;
            return this;
        }

        /// <inheritdoc />
        public IRibbonBuilder EnableDisplayVersion()
        {
            Ribbon.DisplayVersion = true;
            return this;
        }

        /// <inheritdoc />
        public IRibbonBuilder VersionPrefix(string prefix)
        {
            Ribbon.VersionPrefix = prefix;
            return this;
        }

        /// <summary>
        /// Loads a ribbon menu from configuration.
        /// </summary>
        /// <param name="config">Configuration.</param>
        /// <param name="fromConfigStrategies">Collection of <see cref="IItemFromConfigStrategy"/>.</param>
        internal void LoadFromConfig(
            IConfiguration config,
            IReadOnlyCollection<IItemFromConfigStrategy> fromConfigStrategies)
        {
            SetProperties(config.GetSection(nameof(Ribbon)));

            var tabs = config.GetSection(nameof(Ribbon)).GetSection(nameof(Ribbon.Tabs));
            if (!tabs.Exists())
                return;

            foreach (var tabSection in tabs.GetChildren())
            {
                if (!tabSection.Exists())
                    continue;
                var tab = CreateTab(tabSection.GetSection(nameof(Application.Ribbon.Tab.Name)).Value);
                tab.LoadFromConfig(tabSection, fromConfigStrategies);
            }
        }

        private void SetProperties(IConfiguration config)
        {
            var versionSection = config.GetSection(nameof(Ribbon.DisplayVersion));
            if (versionSection.Exists())
            {
                Ribbon.DisplayVersion = versionSection.Get<bool>();
            }

            var headerSection = config.GetSection(nameof(Ribbon.VersionPrefix));
            if (headerSection.Exists())
            {
                Ribbon.VersionPrefix = headerSection.Value;
            }
        }

        private TabBuilder CreateTab(string tabTitle, Action<ITabBuilder>? tab = null)
        {
            var builder = new TabBuilder(tabTitle);
            tab?.Invoke(builder);
            Ribbon.Tabs.Add(builder.Build());
            return builder;
        }
    }
}