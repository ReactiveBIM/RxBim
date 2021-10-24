namespace RxBim.Application.Ribbon.Revit.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Abstractions;
    using Autodesk.Revit.UI;
    using Models.Configurations;
    using Ribbon.Services;
    using UIFramework;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilder"/> for Revit
    /// </summary>
    public class RevitRibbonMenuBuilder : RibbonMenuBuilderBase<string, RibbonPanel>
    {
        private readonly UIControlledApplication _application;

        /// <inheritdoc />
        public RevitRibbonMenuBuilder(UIControlledApplication application, Assembly menuAssembly)
            : base(menuAssembly)
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
            {
                return existsTab.Title;
            }

            _application.CreateRibbonTab(tabName);
            return tabName;
        }

        /// <inheritdoc />
        protected override RibbonPanel GetOrCreatePanel(string tabName, string panelName)
        {
            var existsPanel = _application.GetRibbonPanels(tabName)
                .FirstOrDefault(x => x.Title.Equals(panelName, StringComparison.OrdinalIgnoreCase));

            return existsPanel ?? _application.CreateRibbonPanel(tabName, panelName);
        }

        /// <inheritdoc />
        protected override void CreateAboutButton(RibbonPanel panel, AboutButton aboutButton)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void CreateCommandButton(RibbonPanel panel, CommandButton cmdButton)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void CreatePullDownButton(RibbonPanel panel, PullDownButton pullDownButton)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void AddSeparator(RibbonPanel panel)
        {
            panel.AddSeparator();
        }

        /// <inheritdoc />
        protected override void AddSlideOut(RibbonPanel panel)
        {
            panel.AddSlideOut();
        }

        /// <inheritdoc />
        protected override void CreateStackedItems(RibbonPanel panel, StackedItems stackedItems)
        {
            throw new NotImplementedException();
        }
    }
}