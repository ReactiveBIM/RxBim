namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using Abstractions.ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;
    using Models.Configurations;

    /// <inheritdoc />
    public class TabBuilder : ITabBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabBuilder"/> class.
        /// </summary>
        /// <param name="name">Tab name</param>
        /// <param name="ribbonBuilder">Ribbon builder</param>
        public TabBuilder(string name, RibbonBuilder ribbonBuilder)
        {
            RibbonBuilder = ribbonBuilder;
            BuildingTab.Name = name;
        }

        /// <inheritdoc />
        public IRibbonBuilder RibbonBuilder { get; }

        /// <inheritdoc />
        public Tab BuildingTab { get; } = new();

        /// <inheritdoc />
        public IRibbonBuilder ReturnToRibbon()
        {
            return RibbonBuilder;
        }

        /// <inheritdoc />
        public IPanelBuilder AddPanel(string panelTitle)
        {
            return AddPanelInternal(panelTitle);
        }

        /// <summary>
        /// Load from config
        /// </summary>
        /// <param name="tabSection">Tab config section</param>
        internal void LoadFromConfig(IConfigurationSection tabSection)
        {
            var panelsSection = tabSection.GetSection(nameof(Tab.Panels));
            if (!panelsSection.Exists())
                return;

            foreach (var panelSection in panelsSection.GetChildren())
            {
                if (!panelsSection.Exists())
                    continue;
                var panelBuilder = AddPanelInternal(panelSection.GetSection(nameof(Panel.Name)).Value);
                panelBuilder.LoadFromConfig(panelSection);
            }
        }

        private PanelBuilder AddPanelInternal(string panelTitle)
        {
            var builder = new PanelBuilder(panelTitle, this);
            BuildingTab.Panels.Add(builder.BuildingPanel);
            return builder;
        }
    }
}