namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    using System;
    using Microsoft.Extensions.Configuration;
    using RxBim.Shared;

    /// <summary>
    /// Represents a tab buileder.
    /// </summary>
    public class TabBuilder : ITabBuilder
    {
        private readonly RibbonBuilder _ribbonBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabBuilder"/> class.
        /// </summary>
        /// <param name="name">Tab name.</param>
        /// <param name="ribbonBuilder">Ribbon builder.</param>
        public TabBuilder(string name, RibbonBuilder ribbonBuilder)
        {
            _ribbonBuilder = ribbonBuilder;
            BuildingTab.Name = name;
        }

        /// <summary>
        /// The tab to create configuration.
        /// </summary>
        public Tab BuildingTab { get; } = new();

        /// <inheritdoc />
        public IRibbonBuilder ReturnToRibbon()
        {
            return _ribbonBuilder;
        }

        /// <inheritdoc />
        public ITabBuilder Panel(string title, Action<IPanelBuilder> panel)
        {
            CreatePanel(title, panel);
            return this;
        }

        /// <inheritdoc />
        public ITabBuilder AboutButton(
            string name,
            AboutBoxContent content,
            Action<IAboutButtonBuilder>? builder = null,
            string? panelName = null)
        {
            var panel = new PanelBuilder(panelName ?? name, _ribbonBuilder);
            panel.AddAboutButton(name, content, builder);
            BuildingTab.Panels.Add(panel.BuildingPanel);
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
            var builder = new PanelBuilder(panelTitle, _ribbonBuilder);
            panel?.Invoke(builder);
            BuildingTab.Panels.Add(builder.BuildingPanel);
            return builder;
        }
    }
}