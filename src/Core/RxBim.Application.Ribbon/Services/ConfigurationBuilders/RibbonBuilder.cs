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
            CreateTab(title, tab);
            return this;
        }

        /// <inheritdoc />
        public IRibbonBuilder SetDisplayVersion(bool enable)
        {
            Ribbon.AddVersionToCommandTooltip = enable;
            return this;
        }

        /// <inheritdoc />
        public IRibbonBuilder EnableDisplayVersion()
        {
            Ribbon.AddVersionToCommandTooltip = true;
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
                var tab = CreateTab(tabSection.GetSection(nameof(Application.Ribbon.Tab.Name)).Value);
                tab.LoadFromConfig(tabSection);
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

        private TabBuilder CreateTab(string tabTitle, Action<ITabBuilder>? tab = null)
        {
            var builder = new TabBuilder(tabTitle, this);
            tab?.Invoke(builder);
            Ribbon.Tabs.Add(builder.BuildingTab);
            return builder;
        }
    }
}