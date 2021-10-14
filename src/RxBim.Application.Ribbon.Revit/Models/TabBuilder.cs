namespace RxBim.Application.Ribbon.Revit.Models
{
    using System;
    using Abstractions;
    using Di;
    using Ribbon.Services;

    /// <summary>
    /// Revit ribbon tab
    /// </summary>
    public class TabBuilder : RibbonItemsContainerBuilderBase<RibbonBuilder>, ITabBuilder
    {
        private readonly string _tabName;

        private bool _isAddAboutButton;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ribbonBuilder">панель</param>
        /// <param name="tabName">имя закладки</param>
        /// <param name="container">контейнер</param>
        public TabBuilder(RibbonBuilder ribbonBuilder, string tabName, IContainer container)
            : base(ribbonBuilder, container)
        {
            _tabName = tabName;
        }

        /// <summary>
        /// Создает панель на закладке
        /// </summary>
        /// <param name="panelTitle">имя панели</param>
        public IPanelBuilder Panel(string panelTitle)
        {
            var ribbonPanel = string.IsNullOrEmpty(_tabName)
                ? RibbonBuilder.Application.CreateRibbonPanel(panelTitle)
                : RibbonBuilder.Application.CreateRibbonPanel(_tabName, panelTitle);

            return new PanelBuilder(RibbonBuilder, ribbonPanel, Container, this);
        }

        /// <summary>
        /// Создает кнопку о программе
        /// </summary>
        /// <param name="name">Название кнопки</param>
        /// <param name="action">Дополнительны действия для кнопки о программе</param>
        /// <param name="panelName">имя панели</param>
        /// <param name="text">Тест описания</param>
        public ITabBuilder AddAboutButton(string name, Action<IAboutButtonBuilder> action, string panelName = null, string text = null)
        {
            if (_isAddAboutButton)
                return this;

            if (string.IsNullOrEmpty(panelName))
                panelName = name;
            if (string.IsNullOrEmpty(text))
                text = name;

            var panel = Panel(panelName);

            panel.AddAboutButton(name, text, _tabName, panelName, Container, action);

            // var ribbon = ComponentManager.Ribbon;
            // foreach (var tab in ribbon.Tabs)
            // {
            //     if (tab.Title.Equals(_tabName))
            //     {
            //         foreach (var panel in tab.Panels)
            //         {
            //             if (panel.Source.Title.Equals(panelTitle))
            //             {
            //                 var button = new AboutButton(
            //                     name,
            //                     text,
            //                     $"PIK_ABOUT_{_tabName?.GetHashCode()}",
            //                     Container);
            //                 action?.Invoke(button);
            //
            //                 var buttonData = button.BuildButton();
            //
            //                 panel.Source.Items.Add(buttonData);
            //                 break;
            //             }
            //         }
            //
            //         break;
            //     }
            //// }

            _isAddAboutButton = true;
            return this;
        }
    }
}