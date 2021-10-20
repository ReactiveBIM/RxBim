namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Reflection;
    using Abstractions;
    using Models.Configurations;

    /// <inheritdoc />
    public abstract class RibbonMenuBuilderBase<TTab, TPanel> : IRibbonMenuBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonMenuBuilderBase{TTab, TPanel}"/> class.
        /// </summary>
        /// <param name="menuAssembly">Menu defining assembly</param>
        protected RibbonMenuBuilderBase(Assembly menuAssembly)
        {
            MenuAssembly = menuAssembly;
        }

        /// <summary>
        /// Menu defining assembly
        /// </summary>
        protected Assembly MenuAssembly { get; }

        /// <inheritdoc />
        public void BuildRibbonMenu(Ribbon ribbonConfig)
        {
            if (!CheckRibbonCondition())
                return;

            foreach (var tabConfig in ribbonConfig.Tabs)
            {
                CreateTab(tabConfig);
            }
        }

        /// <summary>
        /// Checks the ribbon and returns true if it is in good condition, otherwise returns false
        /// </summary>
        protected abstract bool CheckRibbonCondition();

        /// <summary>
        /// Returns a ribbon tab with the specified name.
        /// If the tab does not exist, it will be created
        /// </summary>
        /// <param name="tabName">Tab name</param>
        protected abstract TTab GetOrCreateTab(string tabName);

        /// <summary>
        /// Returns a ribbon panel with the specified name on the tab.
        /// If the panel does not exist, it will be created
        /// </summary>
        /// <param name="tab">Ribbon tab</param>
        /// <param name="panelName">Panel name</param>
        protected abstract TPanel GetOrCreatePanel(TTab tab, string panelName);

        private void CreateTab(Tab tabConfig)
        {
            if (string.IsNullOrWhiteSpace(tabConfig.Name))
                throw new InvalidOperationException("Tab name is not valid!");

            var tab = GetOrCreateTab(tabConfig.Name!);

            foreach (var panelConfig in tabConfig.Panels)
            {
                CreatePanel(tab, panelConfig);
            }
        }

        private void CreatePanel(TTab tab, Panel panelConfig)
        {
            if (string.IsNullOrWhiteSpace(panelConfig.Name))
                throw new InvalidOperationException("Panel name is not valid!");

            throw new NotImplementedException();
        }
    }
}