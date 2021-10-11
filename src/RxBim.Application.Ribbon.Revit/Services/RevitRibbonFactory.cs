namespace RxBim.Application.Ribbon.Revit.Services
{
    using Abstractions;
    using Autodesk.Revit.UI;
    using Di;
    using Models;
    using Ribbon.Services;

    /// <inheritdoc />
    public class RevitRibbonFactory : RibbonFactoryBase
    {
        private readonly UIControlledApplication _controlledApp;

        /// <summary>
        /// Initializes a new instance of the <see cref="RevitRibbonFactory"/> class.
        /// </summary>
        /// <param name="controlledApp">UIControlledApplication</param>
        public RevitRibbonFactory(UIControlledApplication controlledApp)
        {
            _controlledApp = controlledApp;
        }

        /// <inheritdoc />
        protected override IRibbon Create(IContainer container)
        {
            return new Ribbon(_controlledApp, container);
        }
    }
}