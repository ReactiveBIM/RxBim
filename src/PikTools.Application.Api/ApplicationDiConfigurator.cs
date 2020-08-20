namespace PikTools.Application.Api
{
    using Autodesk.Revit.UI;
    using Di;
    using Shared;
    using Result = Autodesk.Revit.UI.Result;

    /// <summary>
    /// Конфигуратор зависимостей приложения
    /// </summary>
    public class ApplicationDiConfigurator : DiConfigurator
    {
        private readonly object _applicationObject;
        private readonly UIApplication _uiApp;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="applicationObject">aplication object</param>
        /// <param name="uiApp">revit ui application</param>
        public ApplicationDiConfigurator(object applicationObject, UIApplication uiApp)
        {
            _applicationObject = applicationObject;
            _uiApp = uiApp;
        }

        /// <inheritdoc />
        protected override void ConfigureBaseDependencies()
        {
            Container.RegisterInstance(_uiApp);
            Container.RegisterInstance(_uiApp.Application);
            Container.Register(() => _uiApp.ActiveUIDocument);
            Container.Register(() => _uiApp.ActiveUIDocument.Document);
            Container.Register<IMethodCaller<PluginResult>>(() => new MethodCaller<PluginResult>(_applicationObject));
        }
    }
}