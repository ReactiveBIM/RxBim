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
        private readonly IRibbonService _ribbonService;
        private readonly IThemeService _themeService;
        private AutocadRibbonMenuBuilder? _builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutocadRibbonMenuBuilderFactory"/> class.
        /// </summary>
        /// <param name="onlineHelpService">Online help service</param>
        /// <param name="ribbonService">Ribbon service</param>
        /// <param name="themeService">Theme service</param>
        public AutocadRibbonMenuBuilderFactory(IOnlineHelpService onlineHelpService, IRibbonService ribbonService, IThemeService themeService)
        {
            _onlineHelpService = onlineHelpService;
            _ribbonService = ribbonService;
            _themeService = themeService;
        }

        /// <inheritdoc />
        public IRibbonMenuBuilder CreateMenuBuilder(Assembly menuAssembly)
        {
            if (_builder is null)
            {
                _onlineHelpService.Run();
                _ribbonService.Run();
                _themeService.Run();
                _builder = new AutocadRibbonMenuBuilder(_themeService.GetCurrentTheme, menuAssembly);
                _ribbonService.NeedRebuild += (_, _) => _builder.BuildRibbonMenu();
                _themeService.ThemeChanged += (_, _) => _builder.ApplyCurrentTheme();
            }

            return _builder;
        }
    }
}