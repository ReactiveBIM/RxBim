namespace RxBim.Application.Ribbon
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <inheritdoc />
    public abstract class RibbonMenuBuilderBase<TTab, TPanel> : IRibbonMenuBuilder
    {
        private Assembly? _menuAssembly;

        /// <summary>
        /// Menu defining assembly
        /// </summary>
        public Assembly MenuAssembly
        {
            private get
            {
                return _menuAssembly ?? throw new InvalidOperationException(
                    $"Need to set the assembly for the menu first! ({nameof(MenuAssembly)} property)");
            }
            set => _menuAssembly = value;
        }

        /// <summary>
        /// Ribbon configuration.
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
            return MenuAssembly.GetTypeByName(commandTypeName);
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

        /// <summary>
        /// Returns an image of the button's icon.
        /// </summary>
        /// <param name="resourcePath">The image resource path.</param>
        /// <param name="assembly">The assembly containing image embedded resource.</param>
        public ImageSource? GetIconImage(string? resourcePath, Assembly? assembly)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
                return null;

            assembly ??= MenuAssembly;
            var resource = assembly.GetManifestResourceNames()
                .FirstOrDefault(x => x.EndsWith(resourcePath!.Replace('\\', '.')));
            if (resource != null)
            {
                var file = assembly.GetManifestResourceStream(resource);
                if (file != null)
                {
                    var imageExtension = Path.GetExtension(resourcePath);
                    BitmapDecoder bd = imageExtension switch
                    {
                        ".png" => new PngBitmapDecoder(
                            file,
                            BitmapCreateOptions.PreservePixelFormat,
                            BitmapCacheOption.Default),
                        ".bmp" => new BmpBitmapDecoder(
                            file,
                            BitmapCreateOptions.PreservePixelFormat,
                            BitmapCacheOption.Default),
                        ".jpg" => new JpegBitmapDecoder(
                            file,
                            BitmapCreateOptions.PreservePixelFormat,
                            BitmapCacheOption.Default),
                        ".ico" => new IconBitmapDecoder(
                            file,
                            BitmapCreateOptions.PreservePixelFormat,
                            BitmapCacheOption.Default),
                        _ => throw new NotSupportedException($"Image with {imageExtension} extension is not supported.")
                    };
                    return bd.Frames[0];
                }
            }

            var uri = MenuAssembly.TryGetSupportFileUri(resourcePath!);
            return uri != null ? new BitmapImage(uri) : null;
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

        /// <summary>
        /// Creates a new about button.
        /// </summary>
        /// <param name="tab">Ribbon tab.</param>
        /// <param name="panel">Panel.</param>
        /// <param name="aboutButtonConfig">About button configuration.</param>
        protected abstract void CreateAboutButton(TTab tab, TPanel panel, AboutButton aboutButtonConfig);

        /// <summary>
        /// Creates a new command button.
        /// </summary>
        /// <param name="panel">Panel.</param>
        /// <param name="cmdButtonConfig">Command button configuration.</param>
        protected abstract void CreateCommandButton(TPanel panel, CommandButton cmdButtonConfig);

        /// <summary>
        /// Creates a new pull-down button.
        /// </summary>
        /// <param name="panel">Panel.</param>
        /// <param name="pullDownButtonConfig">Pull-down button configuration.</param>
        protected abstract void CreatePullDownButton(TPanel panel, PullDownButton pullDownButtonConfig);

        /// <summary>
        /// Adds a new separator to a given panel.
        /// </summary>
        /// <param name="panel">The given panel.</param>
        protected abstract void AddSeparator(TPanel panel); // todo naming inconsistency: add- get- create-

        /// <summary>
        /// Adds a new slide-out to a given panel.
        /// </summary>
        /// <param name="panel">Panel.</param>
        protected abstract void AddSlideOut(TPanel panel);

        /// <summary>
        /// Creates stacked buttons.
        /// </summary>
        /// <param name="panel">Panel.</param>
        /// <param name="stackedItems">Stacked.</param>
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
                        cmdButton.LoadFromAttribute(MenuAssembly);
                        CreateCommandButton(panel, cmdButton);
                        break;
                    case PullDownButton pullDownButton:
                        pullDownButton.LoadFromAttribute(MenuAssembly);
                        CreatePullDownButton(panel, pullDownButton);
                        break;
                    case PanelLayoutElement { LayoutElementType: PanelLayoutElementType.Separator } _:
                        AddSeparator(panel);
                        break;
                    case PanelLayoutElement { LayoutElementType: PanelLayoutElementType.SlideOut } _:
                        AddSlideOut(panel);
                        break;
                    case StackedItems stackedItems:
                        stackedItems.LoadFromAttribute(MenuAssembly);
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