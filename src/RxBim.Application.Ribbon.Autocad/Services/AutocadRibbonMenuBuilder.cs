namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Autodesk.Windows;
    using Ribbon.Abstractions;
    using Ribbon.Services;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilder"/> for AutoCAD
    /// </summary>
    public class AutocadRibbonMenuBuilder : RibbonMenuBuilderBase<RibbonTab, RibbonPanel>
    {
        /// <inheritdoc />
        public AutocadRibbonMenuBuilder(Assembly menuAssembly)
            : base(menuAssembly)
        {
        }

        /// <inheritdoc />
        protected override bool CheckRibbonCondition()
        {
            return ComponentManager.Ribbon != null;
        }

        /// <inheritdoc />
        protected override RibbonTab GetOrCreateTab(string tabName)
        {
            var acRibbonTab = ComponentManager.Ribbon.Tabs.FirstOrDefault(x =>
                x.Name.Equals(tabName, StringComparison.OrdinalIgnoreCase));

            if (acRibbonTab is null)
            {
                acRibbonTab = new RibbonTab { Name = tabName };
                ComponentManager.Ribbon.Tabs.Add(acRibbonTab);
            }

            return acRibbonTab;
        }

        /// <inheritdoc />
        protected override RibbonPanel GetOrCreatePanel(RibbonTab acRibbonTab, string panelName)
        {
            var acRibbonPanel = acRibbonTab.Panels.FirstOrDefault(x =>
                x.Source.Name.Equals(panelName, StringComparison.OrdinalIgnoreCase));
            if (acRibbonPanel is null)
            {
                acRibbonPanel = new RibbonPanel
                {
                    Source = new RibbonPanelSource
                    {
                        Name = panelName,
                        Title = panelName
                    }
                };
                acRibbonTab.Panels.Add(acRibbonPanel);
            }

            return acRibbonPanel;
        }
    }
}