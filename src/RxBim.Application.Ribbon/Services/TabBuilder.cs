namespace RxBim.Application.Ribbon.Services
{
    using System;
    using Abstractions;
    using Models;
    using Shared;

    /// <summary>
    /// TabBuilder
    /// </summary>
    public class TabBuilder : RibbonControlBuilder<Tab>, ITabBuilder
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
            Control.Name = name;
        }

        /// <inheritdoc />
        public IRibbonBuilder ToRibbonBuilder()
        {
            return _ribbonBuilder;
        }

        /// <inheritdoc />
        public IPanelBuilder Panel(string panelTitle)
        {
            var builder = new PanelBuilder(panelTitle, _ribbonBuilder, this);
            Control.Panels.Add(builder.Control);
            return builder;
        }

        /// <inheritdoc />
        public ITabBuilder AddAboutButton(
            string name,
            string text,
            AboutBoxContent content,
            Action<IButtonBuilder>? action = null,
            string? panelName = null)
        {
            var builder = new PanelBuilder(panelName ?? text, _ribbonBuilder, this);
            builder.AddAboutButton(name, text, content, action);
            Control.Panels.Add(builder.Control);
            return this;
        }
    }
}