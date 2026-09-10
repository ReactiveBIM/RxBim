namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Linq;
    using Autodesk.Revit.UI;
    using Autodesk.Windows;
    using UIFramework;
    using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilder"/> for Revit.
    /// </summary>
    public class RevitRibbonMenuBuilder : RibbonMenuBuilderBase<RibbonTab, RibbonPanel>
    {
        private readonly UIControlledApplication _application;
        private readonly IThemedRibbonButtonService<Autodesk.Revit.UI.RibbonButton> _themedButtonService;

        /// <inheritdoc />
        public RevitRibbonMenuBuilder(
            UIControlledApplication application,
            MenuData menuData,
            IServiceProvider serviceProvider,
            IThemedRibbonButtonService<Autodesk.Revit.UI.RibbonButton> themedButtonService)
            : base(menuData, serviceProvider)
        {
            _application = application;
            _themedButtonService = themedButtonService;
        }

        /// <inheritdoc />
        protected override void PreBuildActions()
        {
            base.PreBuildActions();
            _themedButtonService.Run();
        }

        /// <inheritdoc />
        protected override bool CheckRibbonCondition()
        {
            return RevitRibbonControl.RibbonControl != null;
        }

        /// <inheritdoc />
        protected override RibbonTab GetOrCreateTab(string title)
        {
            var existsTab = RevitRibbonControl.RibbonControl.Tabs
                .FirstOrDefault(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (existsTab != null)
                return existsTab;

            _application.CreateRibbonTab(title);
            return RevitRibbonControl.RibbonControl.Tabs
                .First(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc />
        protected override RibbonPanel GetOrCreatePanel(RibbonTab tab, string panelName)
        {
            var existsPanel = _application.GetRibbonPanels(tab.Title)
                .FirstOrDefault(x => x.Title.Equals(panelName, StringComparison.OrdinalIgnoreCase));

            var panel = existsPanel ?? _application.CreateRibbonPanel(tab.Title, panelName);
            panel.Title = panelName;

            return panel;
        }
    }
}