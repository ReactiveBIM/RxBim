namespace PikTools.Application.Ui.Api.Builder
{
    using System;
    using System.Linq;
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using PikTools.Di;
    using UIFramework;

    /// <summary>
    /// Ribbon wrapper
    /// </summary>
    public class Ribbon
    {
        private readonly RibbonControl _ribbonControl;
        private readonly IContainer _container;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="application">UIControlledApplication</param>
        /// <param name="container"><see cref="IContainer"/></param>
        public Ribbon(UIControlledApplication application, IContainer container)
        {
            Application = application;
            _container = container;

            _ribbonControl = RevitRibbonControl.RibbonControl;
            if (_ribbonControl == null)
                throw new NotSupportedException("Could not initialize Revit ribbon control");
        }

        /// <summary>
        /// UIControlledApplication
        /// </summary>
        public UIControlledApplication Application { get; }

        /// <summary>
        /// Создает или возвращает существующий <see cref="Tab"/>
        /// </summary>
        /// <param name="tabTitle">Заголовок вкладки</param>
        public Tab Tab(string tabTitle = null)
        {
            if (!string.IsNullOrEmpty(tabTitle))
            {
                if (TryFindTab(tabTitle, out var tab))
                {
                    return tab;
                }

                Application.CreateRibbonTab(tabTitle);
            }

            return new Tab(this, tabTitle, _container);
        }

        private bool TryFindTab(string tabTitle, out Tab tab)
        {
            if (_ribbonControl.Tabs.Any(t => t.Title.Equals(tabTitle)))
            {
                tab = new Tab(this, tabTitle, _container);
                return true;
            }

            tab = default;
            return false;
        }
    }
}