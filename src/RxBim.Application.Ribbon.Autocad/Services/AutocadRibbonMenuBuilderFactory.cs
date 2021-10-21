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
        private readonly IRibbonEvents _ribbonEvents;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutocadRibbonMenuBuilderFactory"/> class.
        /// </summary>
        /// <param name="onlineHelpService">Online help service</param>
        /// <param name="ribbonEvents">Ribbon events service</param>
        public AutocadRibbonMenuBuilderFactory(IOnlineHelpService onlineHelpService, IRibbonEvents ribbonEvents)
        {
            _onlineHelpService = onlineHelpService;
            _ribbonEvents = ribbonEvents;
        }

        /// <inheritdoc />
        public IRibbonMenuBuilder CreateMenuBuilder(Assembly menuAssembly)
        {
            _onlineHelpService.Run();
            _ribbonEvents.Run();
            var builder = new AutocadRibbonMenuBuilder(menuAssembly);
            _ribbonEvents.NeedRebuild += (_, _) => builder.BuildRibbonMenu(null, null);
            return builder;
        }
    }
}