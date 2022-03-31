namespace RxBim.Application.Ribbon.Autocad.Services
{
    using System.Reflection;
    using Abstractions;
    using Ribbon.Abstractions;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilderFactory"/> for AutoCAD
    /// </summary>
    public class AutocadRibbonMenuBuilderFactory : IRibbonMenuBuilderFactory
    {
        private readonly IOnlineHelpService _onlineHelpService;
        private readonly IRibbonEventsService _ribbonEventsService;
        private readonly IThemeService _themeService;
        private readonly IPanelService _panelService;
        private readonly IButtonService _buttonService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutocadRibbonMenuBuilderFactory"/> class.
        /// </summary>
        /// <param name="onlineHelpService">Online help service</param>
        /// <param name="ribbonEventsService">Ribbon service</param>
        /// <param name="themeService">Theme service</param>
        /// <param name="panelService">Panel service.</param>
        /// <param name="buttonService">Button service.</param>
        public AutocadRibbonMenuBuilderFactory(
            IOnlineHelpService onlineHelpService,
            IRibbonEventsService ribbonEventsService,
            IThemeService themeService,
            IPanelService panelService,
            IButtonService buttonService)
        {
            _onlineHelpService = onlineHelpService;
            _ribbonEventsService = ribbonEventsService;
            _themeService = themeService;
            _panelService = panelService;
            _buttonService = buttonService;
        }

        /// <inheritdoc />
        public IRibbonMenuBuilder? CurrentBuilder { get; private set; }

        /// <inheritdoc />
        public IRibbonMenuBuilder CreateMenuBuilder(Assembly menuAssembly)
        {
            if (CurrentBuilder is not null)
                return CurrentBuilder;

            _onlineHelpService.Run();
            _ribbonEventsService.Run();
            _themeService.Run();

            CurrentBuilder =
                new AutocadRibbonMenuBuilder(menuAssembly, _onlineHelpService, _panelService, _buttonService);

            return CurrentBuilder;
        }
    }
}