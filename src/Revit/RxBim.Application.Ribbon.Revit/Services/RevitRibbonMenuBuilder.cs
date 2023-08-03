namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Linq;
    using Autodesk.Revit.UI;
    using UIFramework;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilder"/> for Revit.
    /// </summary>
    public class RevitRibbonMenuBuilder : RibbonMenuBuilderBase<string, RibbonPanel>
    {
        private readonly UIControlledApplication _application;

        /// <inheritdoc />
        public RevitRibbonMenuBuilder(
            UIControlledApplication application,
            MenuData menuData,
            IServiceProvider serviceProvider)
            : base(menuData, serviceProvider)
        {
            _application = application;
        }

        /// <inheritdoc />
        protected override bool CheckRibbonCondition()
        {
            return RevitRibbonControl.RibbonControl != null;
        }

        /// <inheritdoc />
        protected override string GetOrCreateTab(string title)
        {
            var existsTab = RevitRibbonControl.RibbonControl.Tabs
                .FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (existsTab != null)
                return existsTab.Title;

            _application.CreateRibbonTab(title);
            return title;
        }

        /// <inheritdoc />
        protected override RibbonPanel GetOrCreatePanel(string tabName, string panelName)
        {
            var existsPanel = _application.GetRibbonPanels(tabName)
                .FirstOrDefault(x => x.Title.Equals(panelName, StringComparison.OrdinalIgnoreCase));

            var panel = existsPanel ?? _application.CreateRibbonPanel(tabName, panelName);
            panel.Title = panelName;

            return panel;
        }
    }
}