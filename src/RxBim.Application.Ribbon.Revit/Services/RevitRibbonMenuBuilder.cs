namespace RxBim.Application.Ribbon.Revit.Services
{
    using System;
    using System.Linq;
    using Abstractions;
    using Autodesk.Revit.UI;
    using Models;
    using Ribbon.Services;
    using Shared.Abstractions;
    using UIFramework;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilder"/> for Revit
    /// </summary>
    public class RevitRibbonMenuBuilder : RibbonMenuBuilderBase<string, RibbonPanel>
    {
        private readonly UIControlledApplication _application;

        /// <inheritdoc />
        public RevitRibbonMenuBuilder(
            UIControlledApplication application,
            MenuData menuData,
            IDiCollectionService<IAddElementStrategy> strategiesService)
            : base(menuData, strategiesService)
        {
            _application = application;
        }

        /// <inheritdoc />
        protected override bool CheckRibbonCondition()
        {
            return RevitRibbonControl.RibbonControl != null;
        }

        /// <inheritdoc />
        protected override string GetOrCreateTab(string tabName)
        {
            var existsTab = RevitRibbonControl.RibbonControl.Tabs.FirstOrDefault(t =>
                t.Title.Equals(tabName, StringComparison.OrdinalIgnoreCase));
            if (existsTab != null)
                return existsTab.Title;

            _application.CreateRibbonTab(tabName);
            return tabName;
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