namespace RxBim.Application.Ribbon.Services.ConfigurationBuilders
{
    using System;
    using Abstractions.ConfigurationBuilders;
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
        public Tab BuildingTab { get; } = new ();

        /// <inheritdoc />
        public IRibbonBuilder ToRibbonBuilder()
        {
            return _ribbonBuilder;
        }

        /// <inheritdoc />
        public IPanelBuilder AddPanel(string panelTitle)
        {
            var builder = new PanelBuilder(panelTitle, _ribbonBuilder, this);
            BuildingTab.Panels.Add(builder.BuildingPanel);
            return builder;
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
    }
}