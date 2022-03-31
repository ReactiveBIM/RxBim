namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Reflection;
    using System.Windows.Media.Imaging;
    using Abstractions;
    using Extensions;
    using Models;
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
        private Assembly MenuAssembly { get; }

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
        }

        /// <inheritdoc />
        public Type GetCommandType(string commandTypeName)
        {
            return MenuAssembly.GetTypeFromName(commandTypeName);
        }

        /// <inheritdoc />
        public string? GetTooltipContent(CommandButton cmdButtonConfig, Type commandType)
        {
            var toolTip = cmdButtonConfig.ToolTip;
            if (toolTip is null || !RibbonConfiguration!.AddVersionToCommandTooltip)
                return toolTip;
            if (toolTip.Length > 0)
                toolTip += Environment.NewLine;
            toolTip += $"{RibbonConfiguration.CommandTooltipVersionHeader}{commandType.Assembly.GetName().Version}";
            return toolTip;
        }

        /// <inheritdoc />
        public BitmapImage? GetIconImage(string? fullOrRelativeImagePath)
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

        /// <summary>
        /// Creates about button
        /// </summary>
        /// <param name="tab">Ribbon tab</param>
        /// <param name="panel">Panel</param>
        /// <param name="aboutButtonConfig">About button configuration</param>
        protected abstract void CreateAboutButton(TTab tab, TPanel panel, AboutButton aboutButtonConfig);

        /// <summary>
        /// Creates command button
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <param name="cmdButtonConfig">Command button configuration</param>
        protected abstract void CreateCommandButton(TPanel panel, CommandButton cmdButtonConfig);

        /// <summary>
        /// Creates pull-down button
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <param name="pullDownButtonConfig">Pull-down button configuration</param>
        protected abstract void CreatePullDownButton(TPanel panel, PullDownButton pullDownButtonConfig);

        /// <summary>
        /// Creates and adds separator
        /// </summary>
        /// <param name="panel">Panel</param>
        protected abstract void AddSeparator(TPanel panel);

        /// <summary>
        /// Creates and adds slide-out
        /// </summary>
        /// <param name="panel">Panel</param>
        protected abstract void AddSlideOut(TPanel panel);

        /// <summary>
        /// Creates stacked buttons
        /// </summary>
        /// <param name="panel">Panel</param>
        /// <param name="stackedItems">Stacked</param>
        protected abstract void CreateStackedItems(TPanel panel, StackedItems stackedItems);

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

            foreach (var elementConfig in panelConfig.Elements)
            {
                switch (elementConfig)
                {
                    case AboutButton aboutButton:
                        CreateAboutButton(tab, panel, aboutButton);
                        break;
                    case CommandButton cmdButton:
                        CreateCommandButton(panel, cmdButton);
                        break;
                    case PullDownButton pullDownButton:
                        CreatePullDownButton(panel, pullDownButton);
                        break;
                    case PanelLayoutElement { LayoutElementType: PanelLayoutElementType.Separator } _:
                        AddSeparator(panel);
                        break;
                    case PanelLayoutElement { LayoutElementType: PanelLayoutElementType.SlideOut } _:
                        AddSlideOut(panel);
                        break;
                    case StackedItems stackedItems:
                        CreateStackedItems(panel, stackedItems);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"Unknown panel item type: {elementConfig.GetType().Name}");
                }
            }
        }
    }
}