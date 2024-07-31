namespace RxBim.Application.Revit
{
    using System;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Events;
    using Di;
    using Shared;
    using Result = Autodesk.Revit.UI.Result;

    /// <summary>
    /// Revit application.
    /// </summary>
    public abstract class RxBimApplication : IExternalApplication
    {
        private readonly UserInterfaceApplicationProxy _uiApplicationProxy = new();
        private UIControlledApplication _application = null!;
        private ApplicationDiConfigurator _diConfigurator = null!;

        /// <inheritdoc />
        public Result OnStartup(UIControlledApplication application)
        {
            _application = application;
            _diConfigurator = new ApplicationDiConfigurator(this, application, _uiApplicationProxy);
            _diConfigurator.Configure(GetType().Assembly);
            application.Idling += ApplicationIdling;
            return Result.Succeeded;
        }

        /// <inheritdoc />
        public Result OnShutdown(UIControlledApplication application)
        {
            var methodCaller = _diConfigurator.Container.GetService<IMethodCaller<PluginResult>>();
            var result = methodCaller.InvokeMethod(_diConfigurator.Container, Constants.ShutdownMethodName);
            return result.MapResultToRevitResult();
        }

        private void ApplicationIdling(object sender, IdlingEventArgs e)
        {
            if (sender is UIApplication uiApp)
            {
                try
                {
                    if (_uiApplicationProxy.IsInitialized)
                        return;

                    _uiApplicationProxy.Initialize(uiApp);

                    var methodCaller = _diConfigurator.Container.GetService<IMethodCaller<PluginResult>>();
                    methodCaller.InvokeMethod(_diConfigurator.Container, Constants.StartMethodName);
                }
                catch (Exception exception)
                {
                    TaskDialog.Show("Error", exception.ToString());
                    throw;
                }
                finally
                {
                    _application.Idling -= ApplicationIdling;
                }
            }
        }
    }
}