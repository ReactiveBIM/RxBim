namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Represents a tab builder.
    /// </summary>
    public class TabBuilder : ITabBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabBuilder"/> class.
        /// </summary>
        /// <param name="name">Tab name.</param>
        /// <param name="ribbonBuilder">Ribbon builder.</param>
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
        public ITabBuilder Panel(string title, Action<IPanelBuilder> panel)
        {
            CreatePanel(title, panel);
            return this;
        }

        /// <summary>
        /// Loads a tab from configuration.
        /// </summary>
        /// <param name="section">Tab config section.</param>
        internal void LoadFromConfig(IConfigurationSection section)
        {
            var panelsSection = section.GetSection(nameof(Tab.Panels));
            if (!panelsSection.Exists())
                return;

            foreach (var panelSection in panelsSection.GetChildren())
            {
                if (!panelsSection.Exists())
                    continue;
                var panel = CreatePanel(panelSection.GetSection(nameof(Application.Ribbon.Panel.Name)).Value);
                panel.LoadFromConfig(panelSection);
            }
        }

        private PanelBuilder CreatePanel(string panelTitle, Action<IPanelBuilder>? panel = null)
        {
            var builder = new PanelBuilder(panelTitle);
            panel?.Invoke(builder);
            BuildingTab.Panels.Add(builder.BuildingPanel);
            return builder;
        }
    }
}