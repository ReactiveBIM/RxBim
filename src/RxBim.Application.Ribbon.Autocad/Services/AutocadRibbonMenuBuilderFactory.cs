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
        private AutocadRibbonMenuBuilder? _builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutocadRibbonMenuBuilderFactory"/> class.
        /// </summary>
        /// <param name="onlineHelpService">Online help service</param>
        /// <param name="ribbonEventsService">Ribbon service</param>
        /// <param name="themeService">Theme service</param>
        public AutocadRibbonMenuBuilderFactory(
            IOnlineHelpService onlineHelpService,
            IRibbonEventsService ribbonEventsService,
            IThemeService themeService)
        {
            _onlineHelpService = onlineHelpService;
            _ribbonEventsService = ribbonEventsService;
            _themeService = themeService;
        }

        /// <inheritdoc />
        public IRibbonMenuBuilder CreateMenuBuilder(Assembly menuAssembly)
        {
            if (_builder is null)
            {
                _onlineHelpService.Run();
                _ribbonEventsService.Run();
                _themeService.Run();
                _builder = new AutocadRibbonMenuBuilder(menuAssembly,
                    _themeService.GetCurrentTheme,
                    _onlineHelpService.ClearToolTipsCache,
                    _onlineHelpService.AddToolTip);
                _ribbonEventsService.NeedRebuild += (_, _) => _builder.BuildRibbonMenu();
                _themeService.ThemeChanged += (_, _) => _builder.ApplyCurrentTheme();
            }

            return _builder;
        }
    }
}