namespace RxBim.Application.Ribbon.ConfigurationBuilders
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Represents a tab builder.
    /// </summary>
    public class TabBuilder : ITabBuilder
    {
        private readonly Tab _tab = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="TabBuilder"/> class.
        /// </summary>
        /// <param name="name">Tab name.</param>
        public TabBuilder(string name)
        {
            _tab.Name = name;
        }

        /// <inheritdoc />
        public ITabBuilder Panel(string title, Action<IPanelBuilder> panel)
        {
            CreatePanel(title, panel);
            return this;
        }

        /// <summary>
        /// Returns tab.
        /// </summary>
        internal Tab Build()
        {
            return _tab;
        }

        /// <summary>
        /// Load from config
        /// </summary>
        /// <param name="section">Tab config section</param>
        /// <param name="fromConfigStrategies">Collection of <see cref="IItemFromConfigStrategy"/>.</param>
        internal void LoadFromConfig(
            IConfigurationSection section,
            IReadOnlyCollection<IItemFromConfigStrategy> fromConfigStrategies)
        {
            var panelsSection = section.GetSection(nameof(Tab.Panels));
            if (!panelsSection.Exists())
                return;

            foreach (var panelSection in panelsSection.GetChildren())
            {
                if (!panelsSection.Exists())
                    continue;
                var panel = CreatePanel(panelSection.GetSection(nameof(Application.Ribbon.Panel.Name)).Value);
                panel.LoadFromConfig(panelSection, fromConfigStrategies);
            }
        }

        private PanelBuilder CreatePanel(string panelTitle, Action<IPanelBuilder>? panel = null)
        {
            var builder = new PanelBuilder(panelTitle);
            panel?.Invoke(builder);
            _tab.Panels.Add(builder.Build());
            return builder;
        }
    }
}