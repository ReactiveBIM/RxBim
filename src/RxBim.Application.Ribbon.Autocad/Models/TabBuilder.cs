namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using System.Linq;
    using Application.Ribbon.Abstractions;
    using Autodesk.Windows;
    using Di;
    using Extensions;
    using Ribbon.Services;

    /// <summary>
    /// Represents the entity of a ribbon tab
    /// </summary>
    public class TabBuilder : RibbonItemsContainerBuilderBase<RibbonBuilder>, ITabBuilder
    {
        private bool _isAddAboutButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabBuilder"/> class.
        /// </summary>
        /// <param name="ribbonBuilder">The entity of a ribbon</param>
        /// <param name="tabId">The identifier of this ribbon tab control</param>
        /// <param name="container">DI container</param>
        public TabBuilder(RibbonBuilder ribbonBuilder, string tabId, IContainer container)
            : base(ribbonBuilder, container)
        {
            Id = tabId;
        }

        /// <summary>
        /// Ribbon tab control identifier
        /// </summary>
        public string Id { get; }

        /// <inheritdoc />
        public IPanelBuilder Panel(string panelTitle)
        {
            var tab = this.GetRibbonTab();
            var panel = tab.Panels.FirstOrDefault(x => x.Source.Title.Equals(panelTitle, StringComparison.Ordinal));
            var id = $"PANEL_{tab.Id}_{panelTitle.GetHashCode()}";

            if (panel is null)
            {
                panel = new RibbonPanel
                {
                    Source = new RibbonPanelSource
                    {
                        Title = panelTitle,
                        Id = id
                    }
                };

                tab.Panels.Add(panel);
            }

            return new PanelBuilder(RibbonBuilder, Container, id, this);
        }

        /// <inheritdoc />
        public ITabBuilder AddAboutButton(string name, Action<IAboutButtonBuilder> action, string? panelName = null, string? text = null)
        {
            if (_isAddAboutButton)
                return this;

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(panelName))
                panelName = name;
            if (string.IsNullOrWhiteSpace(text))
                text = name;

            var panel = Panel(panelName!);
            var tab = this.GetRibbonTab();
            panel.AddAboutButton(name, text, tab.Name, panelName, Container, action);

            _isAddAboutButton = true;
            return this;
        }
    }
}