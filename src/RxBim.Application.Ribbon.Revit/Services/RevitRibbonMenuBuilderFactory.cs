namespace RxBim.Application.Ribbon.Revit.Services
{
    using System.Reflection;
    using Abstractions;
    using Autodesk.Revit.UI;

    /// <summary>
    /// Implementation of <see cref="IRibbonMenuBuilderFactory"/> for Revit.
    /// </summary>
    public class RevitRibbonMenuBuilderFactory : IRibbonMenuBuilderFactory
    {
        private readonly UIControlledApplication _application;

        /// <summary>
        /// Initializes a new instance of the <see cref="RevitRibbonMenuBuilderFactory"/> class.
        /// </summary>
        /// <param name="application">Revit application.</param>
        public RevitRibbonMenuBuilderFactory(UIControlledApplication application)
        {
            _application = application;
        }

        /// <inheritdoc />
        public IRibbonMenuBuilder CreateMenuBuilder(Assembly menuAssembly)
        {
            return new RevitRibbonMenuBuilder(_application, menuAssembly);
        }
    }
}