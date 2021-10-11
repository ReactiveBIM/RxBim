namespace RxBim.Application.Ribbon.Revit.Models
{
    using System;
    using Abstractions;
    using Application.Ribbon.Models;
    using Di;

    /// <summary>
    /// Закладка панели
    /// </summary>
    public class Tab : RibbonBuilderBase<Ribbon>, ITab
    {
        private bool _isAddAboutButton;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbon">панель</param>
        /// <param name="tabName">имя закладки</param>
        /// <param name="id">Tab identifier</param>
        /// <param name="container">контейнер</param>
        public Tab(Ribbon ribbon, string tabName, string id, IContainer container)
            : base(ribbon, id, container)
        {
            Title = tabName;
        }

        /// <inheritdoc />
        public string Title { get; }

        /// <summary>
        /// Создает панель на закладке
        /// </summary>
        /// <param name="panelTitle">имя панели</param>
        public IPanel Panel(string panelTitle)
        {
            var ribbonPanel = string.IsNullOrEmpty(Title)
                ? Ribbon.Application.CreateRibbonPanel(panelTitle)
                : Ribbon.Application.CreateRibbonPanel(Title, panelTitle);

            return new Panel(Ribbon, ribbonPanel, Container);
        }

        /// <summary>
        /// Создает кнопку о программе
        /// </summary>
        /// <param name="name">Название кнопки</param>
        /// <param name="action">Дополнительны действия для кнопки о программе</param>
        /// <param name="panelName">имя панели</param>
        /// <param name="text">Тест описания</param>
        public ITab AboutButton(string name, Action<IAboutButton> action, string panelName = null, string text = null)
        {
            if (_isAddAboutButton)
                return this;

            if (string.IsNullOrEmpty(panelName))
                panelName = name;
            if (string.IsNullOrEmpty(text))
                text = name;

            var panel = Panel(panelName);

            panel.AddAboutButton(name, text, Title, panelName, Container, action);

            _isAddAboutButton = true;
            return this;
        }
    }
}