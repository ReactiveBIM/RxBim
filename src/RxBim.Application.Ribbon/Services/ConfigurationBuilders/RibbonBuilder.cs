namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using Abstractions.ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;
    using Models.Configurations;

    /// <inheritdoc />
    public class RibbonBuilder : IRibbonBuilder
    {
        /// <summary>
        /// Building ribbon
        /// </summary>
        public Ribbon Ribbon { get; } = new();

        /// <inheritdoc />
        public ITabBuilder AddTab(string tabTitle)
        {
            return AddTabInternal(tabTitle);
        }

        /// <inheritdoc />
        public IRibbonBuilder EnableAddVersionToCommandTooltip()
        {
            Ribbon.AddVersionToCommandTooltip = true;
            return this;
        }

        /// <inheritdoc />
        public IRibbonBuilder SetAddVersionToCommandTooltip(bool value)
        {
            Ribbon.AddVersionToCommandTooltip = value;
            return this;
        }

        /// <inheritdoc />
        public IRibbonBuilder SetCommandTooltipVersionHeader(string header)
        {
            Ribbon.CommandTooltipVersionHeader = header;
            return this;
        }

        /// <summary>
        /// Load ribbon menu from configuration
        /// </summary>
        /// <param name="config">Configuration</param>
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
                var tabBuilder = AddTabInternal(tabSection.GetSection(nameof(Tab.Name)).Value);
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

        private TabBuilder AddTabInternal(string tabTitle)
        {
            var builder = new TabBuilder(tabTitle, this);
            Ribbon.Tabs.Add(builder.BuildingTab);
            return builder;
        }
    }
}