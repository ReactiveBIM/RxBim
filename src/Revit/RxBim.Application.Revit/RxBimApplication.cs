namespace RxBim.Application.Revit
{
    using Autodesk.Revit.UI;
    using Di;
    using Shared;
    using Result = Autodesk.Revit.UI.Result;

    /// <summary>
    /// Revit application.
    /// </summary>
    public abstract class RxBimApplication : IExternalApplication
    {
        private IContainer _container = null!;

        /// <inheritdoc />
        public Result OnStartup(UIControlledApplication application)
        {
            var diConfigurator = new ApplicationDiConfigurator(this, application);
            diConfigurator.RevitServicesReady += OnRevitServicesReady;
            diConfigurator.Configure(GetType().Assembly);
            _container = diConfigurator.Container;
            return Result.Succeeded;
        }

        /// <inheritdoc />
        public Result OnShutdown(UIControlledApplication application)
        {
            var methodCaller = _container.GetService<IMethodCaller<PluginResult>>();
            var result = methodCaller.InvokeMethod(_container, Constants.ShutdownMethodName);
            return result.MapResultToRevitResult();
        }

        private void OnRevitServicesReady(object sender, System.EventArgs e)
        {
            // Unsubscribe from subsequent calls
            ((ApplicationDiConfigurator)sender).RevitServicesReady -= OnRevitServicesReady;

            // Launch plugin application
            _ = _container
                .GetService<IMethodCaller<PluginResult>>()
                .InvokeMethod(_container, Constants.StartMethodName);
        }
    }
}