namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Media.Imaging;
    using Abstractions;
    using Extensions;
    using Models.Configurations;

    /// <inheritdoc />
    public abstract class RibbonMenuBuilderBase<TTab, TPanel> : IRibbonMenuBuilder
    {
        private readonly IAddElementsStrategiesFactory _addElementsStrategiesFactory;
        private Assembly? _menuAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonMenuBuilderBase{TTab, TPanel}"/> class.
        /// </summary>
        /// <param name="addElementsStrategiesFactory"><see cref="IAddElementsStrategiesFactory"/>.</param>
        protected RibbonMenuBuilderBase(IAddElementsStrategiesFactory addElementsStrategiesFactory)
        {
            _addElementsStrategiesFactory = addElementsStrategiesFactory;
        }

        /// <inheritdoc />
        public event EventHandler? MenuCreated;

        /// <summary>
        /// Menu defining assembly.
        /// </summary>
        protected Assembly MenuAssembly => _menuAssembly ??
                                           throw new InvalidOperationException($"Call {nameof(Initialize)} first!");

        /// <summary>
        /// Ribbon configuration
        /// </summary>
        private Ribbon? RibbonConfiguration { get; set; }

        /// <inheritdoc />
        public void BuildRibbonMenu(Ribbon? ribbonConfig = null)
        {
            RibbonConfiguration ??= ribbonConfig;

            if (RibbonConfiguration is null || !CheckRibbonCondition())
                return;

            PreBuildActions();

            foreach (var tabConfig in RibbonConfiguration.Tabs)
                CreateTab(tabConfig);

            MenuCreated?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public void Initialize(Assembly menuAssembly)
        {
            _menuAssembly = menuAssembly;
        }

        /// <summary>
        /// Returns tooltip content for command button
        /// </summary>
        /// <param name="cmdButtonConfig">Command button configuration</param>
        /// <param name="commandType">Type of command class</param>
        protected string? GetTooltipContent(CommandButton cmdButtonConfig, Type commandType)
        {
            var toolTip = cmdButtonConfig.ToolTip;
            if (toolTip is null || !RibbonConfiguration!.AddVersionToCommandTooltip)
                return toolTip;
            if (toolTip.Length > 0)
                toolTip += Environment.NewLine;
            toolTip += $"{RibbonConfiguration.CommandTooltipVersionHeader}{commandType.Assembly.GetName().Version}";
            return toolTip;
        }

        /// <summary>
        /// Returns an image of the button's icon
        /// </summary>
        /// <param name="fullOrRelativeImagePath">Image path</param>
        protected BitmapImage? GetIconImage(string? fullOrRelativeImagePath)
        {
            if (string.IsNullOrWhiteSpace(fullOrRelativeImagePath))
                return null;
            var uri = MenuAssembly.TryGetSupportFileUri(fullOrRelativeImagePath!);
            return uri != null ? new BitmapImage(uri) : null;
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
                    strategy.CreateElement(this, tab!, panel!, element);
                else
                    throw new InvalidOperationException($"Unknown panel item type: {element.GetType().Name}");
            }
        }
    }
}