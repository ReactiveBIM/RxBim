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
        /// Menu defining assembly
        /// </summary>
        protected Assembly MenuAssembly => _menuAssembly ??
                                           throw new InvalidOperationException($"Call {nameof(Initialize)} first!");

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
        /// Returns an image of the button's icon.
        /// </summary>
        /// <param name="resourcePath">The image resource path.</param>
        /// <param name="assembly">The assembly containing image embedded resource.</param>
        protected ImageSource? GetIconImage(string? resourcePath, Assembly? assembly)
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