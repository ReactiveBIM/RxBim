namespace RxBim.Application.Ribbon.Services
{
    using System.Reflection;
    using Autodesk.Revit.UI;
    using JetBrains.Annotations;
    using Shared.Abstractions;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilderFactory"/> for Revit.
    /// </summary>
    [UsedImplicitly]
    public class RevitRibbonMenuBuilderFactory : IRibbonMenuBuilderFactory
    {
        private readonly UIControlledApplication _application;
        private readonly IAboutShowService _aboutShowService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RevitRibbonMenuBuilderFactory"/> class.
        /// </summary>
        /// <param name="application">Revit application.</param>
        /// <param name="aboutShowService"><see cref="IAboutShowService"/>.</param>
        public RevitRibbonMenuBuilderFactory(UIControlledApplication application, IAboutShowService aboutShowService)
        {
            _application = application;
            _aboutShowService = aboutShowService;
        }

        /// <inheritdoc />
        public IRibbonMenuBuilder? CurrentBuilder { get; private set; }

        /// <inheritdoc />
        public IRibbonMenuBuilder CreateMenuBuilder(Assembly menuAssembly)
        {
            return CurrentBuilder ??= new RevitRibbonMenuBuilder(_application, menuAssembly, _aboutShowService);
        }
    }
}