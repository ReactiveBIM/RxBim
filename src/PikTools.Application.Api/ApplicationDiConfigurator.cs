namespace PikTools.Application.Api
{
    using Autodesk.Revit.UI;
    using Di;
    using Shared;

    /// <summary>
    /// Конфигуратор зависимостей приложения
    /// </summary>
    internal class ApplicationDiConfigurator : DiConfigurator<IApplicationConfiguration>
    {
        private readonly object _applicationObject;
        private readonly UIControlledApplication _uiControlledApp;
        private readonly UIApplication _uiApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="applicationObject">aplication object</param>
        /// <param name="uiControlledApp">revit ui controlled application</param>
        /// <param name="uiApp">revit ui application</param>
        public ApplicationDiConfigurator(
            object applicationObject,
            UIControlledApplication uiControlledApp,
            UIApplication uiApp)
        {
            _applicationObject = applicationObject;
            _uiControlledApp = uiControlledApp;
            _uiApp = uiApp;
        }

        /// <inheritdoc />
        protected override void ConfigureBaseDependencies()
        {
            Container.RegisterInstance(_uiControlledApp);
            Container.RegisterInstance(_uiApp);
            Container.RegisterInstance(_uiApp.Application);
            Container.Register(() => _uiApp.ActiveUIDocument);
            Container.Register(() => _uiApp.ActiveUIDocument.Document);
            Container.Register<IMethodCaller<PluginResult>>(() => new MethodCaller<PluginResult>(_applicationObject));
        }
    }
}