﻿namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Linq;
    using Abstractions;
    using Models;
    using Models.Configurations;
    using Shared.Abstractions;

    /// <inheritdoc />
    public abstract class RibbonMenuBuilderBase<TTab, TPanel> : IRibbonMenuBuilder
    {
        private readonly MenuData _menuData;
        private readonly IStrategiesFactory<IAddElementStrategy> _addElementsStrategiesFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonMenuBuilderBase{TTab, TPanel}"/> class.
        /// </summary>
        /// <param name="menuData"><see cref="MenuData"/>.</param>
        /// <param name="addElementsStrategiesFactory">Factory for collection of <see cref="IAddElementStrategy"/>.</param>
        protected RibbonMenuBuilderBase(MenuData menuData, IStrategiesFactory<IAddElementStrategy> addElementsStrategiesFactory)
        {
            _menuData = menuData;
            _addElementsStrategiesFactory = addElementsStrategiesFactory;
        }

        /// <inheritdoc />
        public event EventHandler? MenuCreated;

        /// <inheritdoc />
        public void BuildRibbonMenu(Ribbon? ribbonConfig = null)
        {
            _menuData.RibbonConfiguration ??= ribbonConfig;

            if (_menuData.RibbonConfiguration is null || !CheckRibbonCondition())
                return;

            PreBuildActions();

            foreach (var tabConfig in _menuData.RibbonConfiguration.Tabs)
                CreateTab(tabConfig);

            MenuCreated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Executed before the start of building the menu
        /// </summary>
        protected virtual void PreBuildActions()
        {
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

            var panel = GetOrCreatePanel(tab, panelConfig.Name!);

            var addElementStrategies = _addElementsStrategiesFactory.GetStrategies().ToList();

            foreach (var element in panelConfig.Elements)
            {
                var strategy = addElementStrategies.FirstOrDefault(x => x.IsApplicable(element));
                if (strategy != null)
                    strategy.CreateAndAddElement(panel!, element);
                else
                    throw new InvalidOperationException($"Unknown panel item type: {element.GetType().Name}");
            }
        }
    }
}