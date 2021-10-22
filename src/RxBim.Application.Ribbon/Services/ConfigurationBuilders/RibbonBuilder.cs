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
        public Ribbon Ribbon { get; } = new ();

        /// <inheritdoc />
        public ITabBuilder AddTab(string tabTitle)
        {
            return AddTabInternal(tabTitle);
        }

        /// <summary>
        /// Load ribbon menu from configuration
        /// </summary>
        /// <param name="config">Configuration</param>
        internal void LoadFromConfig(IConfiguration config)
        {
            var tabs = config.GetSection("Menu").GetSection(nameof(Ribbon.Tabs));
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

        private TabBuilder AddTabInternal(string tabTitle)
        {
            var builder = new TabBuilder(tabTitle, this);
            Ribbon.Tabs.Add(builder.BuildingTab);
            return builder;
        }
    }
}