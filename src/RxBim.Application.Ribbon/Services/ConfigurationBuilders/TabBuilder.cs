namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using System;
    using Abstractions.ConfigurationBuilders;
    using Microsoft.Extensions.Configuration;
    using Models.Configurations;
    using Shared;

    /// <summary>
    /// TabBuilder
    /// </summary>
    public class TabBuilder : ITabBuilder
    {
        private readonly RibbonBuilder _ribbonBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabBuilder"/> class.
        /// </summary>
        /// <param name="name">Tab name</param>
        /// <param name="ribbonBuilder">Ribbon builder</param>
        public TabBuilder(string name, RibbonBuilder ribbonBuilder)
        {
            _ribbonBuilder = ribbonBuilder;
            BuildingTab.Name = name;
        }

        /// <summary>
        /// Building tab
        /// </summary>
        public Tab BuildingTab { get; } = new();

        /// <inheritdoc />
        public IRibbonBuilder ReturnToRibbon()
        {
            return _ribbonBuilder;
        }

        /// <inheritdoc />
        public IPanelBuilder AddPanel(string panelTitle)
        {
            return AddPanelInternal(panelTitle);
        }

        /// <inheritdoc />
        public ITabBuilder AddAboutButton(
            string name,
            AboutBoxContent content,
            Action<IButtonBuilder>? action = null,
            string? panelName = null)
        {
            var builder = new PanelBuilder(panelName ?? name, _ribbonBuilder, this);
            builder.AddAboutButton(name, content, action);
            BuildingTab.Panels.Add(builder.BuildingPanel);
            return this;
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
            var builder = new PanelBuilder(panelTitle, _ribbonBuilder, this);
            BuildingTab.Panels.Add(builder.BuildingPanel);
            return builder;
        }
    }
}