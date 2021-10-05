namespace RxBim.Application.Ribbon.Autocad.Models
{
    using System;
    using System.Linq;
    using Application.Ribbon.Abstractions;
    using Application.Ribbon.Models;
    using Autodesk.Windows;
    using Di;

    /// <summary>
    /// Вкладка
    /// </summary>
    public class Tab : RibbonBuilderBase<Ribbon>, ITab
    {
        private readonly RibbonTab _tab;

        private bool _isAddAboutButton;

        /// <summary>
        /// Конструирует вкладку
        /// </summary>
        /// <param name="ribbon">Лента</param>
        /// <param name="tabName">Название вкладки</param>
        /// <param name="container">Контейнер</param>
        public Tab(Ribbon ribbon, string tabName, IContainer container)
            : base(ribbon, container)
        {
            var ribbonControl = Ribbon.AcadRibbonControl;
            if (ribbonControl is null)
                throw new InvalidOperationException("Ribbon control is null!");
            var tab = ribbonControl.Tabs.FirstOrDefault(t => t.Title == tabName);
            _tab = tab ?? throw new InvalidOperationException($"'{tabName}' tab not found!");
        }

        /// <inheritdoc />
        public IPanel Panel(string panelTitle)
        {
            var panel = _tab.Panels.FirstOrDefault(x => x.Source.Title.Equals(panelTitle, StringComparison.Ordinal));

            if (panel is null)
            {
                panel = new RibbonPanel
                {
                    Source = new RibbonPanelSource
                    {
                        Title = panelTitle,
                        Id = $"PANEL_{_tab.Id}_{panelTitle.GetHashCode()}"
                    }
                };

                _tab.Panels.Add(panel);
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

            panel.AddAboutButton(name, text, _tab.Name, panelName, Container, action);

            _isAddAboutButton = true;
            return this;
        }
    }
}