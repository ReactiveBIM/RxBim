namespace RxBim.Application.Ribbon.Revit.Services
{
    using Abstractions;
    using Autodesk.Revit.UI;
    using Di;
    using Models;

    /// <inheritdoc />
    public class RevitRibbonFactory : IRibbonFactory
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
        public IRibbon Create(IContainer container)
        {
            return new Ribbon(_controlledApp, container);
        }
    }
}