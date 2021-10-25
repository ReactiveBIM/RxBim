namespace RxBim.Application.Ribbon.Services
{
    using System;
    using System.Reflection;
    using System.Windows.Media.Imaging;
    using Abstractions;
    using Extensions;
    using Models;
    using Models.Configurations;
    using Shared;
    using Shared.Abstractions;

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
        /// Service for displaying the "About" window
        /// </summary>
        private IAboutShowService? AboutShowService { get; set; }

        /// <summary>
        /// Ribbon configuration
        /// </summary>
        private Ribbon? RibbonConfiguration { get; set; }

        /// <inheritdoc />
        public void BuildRibbonMenu(Ribbon? ribbonConfig = null, IAboutShowService? aboutShowService = null)
        {
            RibbonConfiguration ??= ribbonConfig;
            AboutShowService ??= aboutShowService;

            if (RibbonConfiguration is null || !CheckRibbonCondition())
                return;

            PreBuildActions();

            foreach (var tabConfig in RibbonConfiguration.Tabs)
            {
                CreateTab(tabConfig);
            }
        }

        /// <summary>
        /// Executed before the start of building the menu
        /// </summary>
        protected virtual void PreBuildActions()
        {
        }

        /// <summary>
        /// Attempts to display the About window
        /// </summary>
        /// <param name="content">Content</param>
        /// <returns>True if successful, otherwise false</returns>
        protected bool TryShowAboutWindow(AboutBoxContent content)
        {
            if (AboutShowService is null)
                return false;

            AboutShowService.ShowAboutBox(content);
            return true;
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

        /// <summary>
        /// Returns command class type
        /// </summary>
        /// <param name="commandTypeName">Command class type name</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Type name is invalid</exception>
        protected Type GetCommandType(string commandTypeName)
        {
            return MenuAssembly.GetTypeFromName(commandTypeName);
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
        /// Returns tooltip content for command button
        /// </summary>
        /// <param name="cmdButtonConfig">Command button configuration</param>
        /// <param name="commandType">Type of command class</param>
        protected string? GetTooltipContent(CommandButton cmdButtonConfig, Type commandType)
        {
            var toolTip = cmdButtonConfig.ToolTip;
            if (toolTip is null || !cmdButtonConfig.ToolTipSettings.AddVersion)
                return toolTip;
            if (toolTip.Length > 0)
                toolTip += Environment.NewLine;
            toolTip += $"{cmdButtonConfig.ToolTipSettings.VersionHeader}{commandType.Assembly.GetName().Version}";
            return toolTip;
        }

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
                    case PanelLayoutElement separator
                        when separator.LayoutElementType == PanelLayoutElementType.Separator:
                        AddSeparator(panel);
                        break;
                    case PanelLayoutElement separator
                        when separator.LayoutElementType == PanelLayoutElementType.SlideOut:
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