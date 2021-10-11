namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using System.Linq;
    using Application.Ribbon.Abstractions;
    using Application.Ribbon.Models;
    using Autodesk.Windows;
    using Di;
    using Extensions;

    /// <summary>
    /// Represents the entity of a ribbon tab
    /// </summary>
    public class Tab : RibbonBuilderBase<Ribbon>, ITab
    {
        private bool _isAddAboutButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tab"/> class.
        /// </summary>
        /// <param name="ribbon">The entity of a ribbon</param>
        /// <param name="tabName">The name of this ribbon tab</param>
        /// <param name="tabId">The identifier of this ribbon tab</param>
        /// <param name="container">DI container</param>
        public Tab(Ribbon ribbon, string tabName, string tabId, IContainer container)
            : base(ribbon, tabId, container)
        {
            Title = tabName;
        }

        /// <inheritdoc />
        public string Title { get; }

        /// <inheritdoc />
        public IPanel Panel(string panelTitle)
        {
            var tab = this.GetRibbonTab();
            var panel = tab.Panels.FirstOrDefault(x => x.Source.Title.Equals(panelTitle, StringComparison.Ordinal));

            if (panel is null)
            {
                panel = new RibbonPanel
                {
                    Source = new RibbonPanelSource
                    {
                        Title = panelTitle,
                        Id = $"PANEL_{tab.Id}_{panelTitle.GetHashCode()}"
                    }
                };

                tab.Panels.Add(panel);
            }

            return new Panel(Ribbon, panel, Container);
        }

        /// <inheritdoc />
        public ITab AboutButton(string name, Action<IAboutButton> action, string? panelName = null, string? text = null)
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

            panel.AddAboutButton(name, text, Name, panelName, Container, action);

            _isAddAboutButton = true;
            return this;
        }
    }
}