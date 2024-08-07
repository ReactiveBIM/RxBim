namespace RxBim.Application.Ribbon
{
    using System;
    using System.Linq;
    using Di;

    /// <inheritdoc />
    public abstract class RibbonMenuBuilderBase<TTab, TPanel> : IRibbonMenuBuilder
    {
        private readonly MenuData _menuData;
        private readonly IServiceLocator _serviceLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonMenuBuilderBase{TTab, TPanel}"/> class.
        /// </summary>
        /// <param name="menuData"><see cref="MenuData"/>.</param>
        /// <param name="serviceLocator"><see cref="IServiceLocator"/>.</param>
        protected RibbonMenuBuilderBase(MenuData menuData, IServiceLocator serviceLocator)
        {
            _menuData = menuData;
            _serviceLocator = serviceLocator;
        }

        /// <inheritdoc />
        public event EventHandler? MenuCreated;

        /// <inheritdoc />
        public void BuildRibbonMenu(Ribbon? ribbonConfig = null)
        {
            _menuData.RibbonConfiguration ??= ribbonConfig;

            if (_menuData.RibbonConfiguration is null)
                return;

            PreBuildActions();

            if (!CheckRibbonCondition())
                return;

            foreach (var tabConfig in _menuData.RibbonConfiguration.Tabs)
                CreateTab(tabConfig);

            MenuCreated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Executed before the start of building the menu.
        /// </summary>
        protected virtual void PreBuildActions()
        {
        }

        /// <summary>
        /// Checks the ribbon and returns true if it is in good condition, otherwise returns false.
        /// </summary>
        protected abstract bool CheckRibbonCondition();

        /// <summary>
        /// Returns a ribbon tab with the specified name.
        /// If the tab does not exist, it will be created.
        /// </summary>
        /// <param name="title">Tab name.</param>
        protected abstract TTab GetOrCreateTab(string title);

        /// <summary>
        /// Returns a ribbon panel with the specified name on the tab.
        /// If the panel does not exist, it will be created.
        /// </summary>
        /// <param name="tab">Ribbon tab.</param>
        /// <param name="panelName">Panel name.</param>
        protected abstract TPanel GetOrCreatePanel(TTab tab, string panelName);

        private void CreateTab(Tab tabConfig)
        {
            if (string.IsNullOrWhiteSpace(tabConfig.Name))
                throw new InvalidOperationException("Tab name is not valid!");

            var tab = GetOrCreateTab(tabConfig.Name!);

            foreach (var panelConfig in tabConfig.Panels)
                CreatePanel(tab, panelConfig);
        }

        private void CreatePanel(TTab tab, Panel panelConfig)
        {
            if (string.IsNullOrWhiteSpace(panelConfig.Name))
                throw new InvalidOperationException("Panel name is not valid!");

            var panel = GetOrCreatePanel(tab, panelConfig.Name!);

            var addItemStrategies = _serviceLocator.GetServices<IItemStrategy>().ToList();

            foreach (var item in panelConfig.Items)
            {
                var strategy = addItemStrategies.FirstOrDefault(x => x.IsApplicable(item));
                if (strategy != null)
                    strategy.AddItem(tab!, panel!, item);
                else
                    throw new InvalidOperationException($"Unknown panel item type: {item.GetType().Name}");
            }
        }
    }
}