namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    using System;
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
            AddTabInternal(title, tab);
            return this;
        }

        /// <inheritdoc />
        public IRibbonBuilder DisplayVersion(bool enable)
        {
            Ribbon.AddVersionToCommandTooltip = enable;
            return this;
        }

        /// <inheritdoc />
        public IRibbonBuilder VersionPrefix(string prefix)
        {
            Ribbon.CommandTooltipVersionHeader = prefix;
            return this;
        }

        /// <summary>
        /// Loads a ribbon menu from configuration.
        /// </summary>
        /// <param name="config">Configuration.</param>
        internal void LoadFromConfig(IConfiguration config)
        {
            SetProperties(config.GetSection(nameof(Ribbon)));

            var tabs = config.GetSection(nameof(Ribbon)).GetSection(nameof(Ribbon.Tabs));
            if (!tabs.Exists())
                return;

            foreach (var tabSection in tabs.GetChildren())
            {
                if (!tabSection.Exists())
                    continue;
                var tabBuilder = AddTabInternal(tabSection.GetSection(nameof(Application.Ribbon.Tab.Name)).Value);
                tabBuilder.LoadFromConfig(tabSection);
            }
        }

        private void SetProperties(IConfiguration config)
        {
            var versionSection = config.GetSection(nameof(Ribbon.AddVersionToCommandTooltip));
            if (versionSection.Exists())
            {
                Ribbon.AddVersionToCommandTooltip = versionSection.Get<bool>();
            }

            var headerSection = config.GetSection(nameof(Ribbon.CommandTooltipVersionHeader));
            if (headerSection.Exists())
            {
                Ribbon.CommandTooltipVersionHeader = headerSection.Value;
            }
        }

        private TabBuilder AddTabInternal(string tabTitle, Action<ITabBuilder>? tab = null)
        {
            var builder = new TabBuilder(tabTitle, this);
            tab?.Invoke(builder);
            Ribbon.Tabs.Add(builder.BuildingTab);
            return builder;
        }
    }
}