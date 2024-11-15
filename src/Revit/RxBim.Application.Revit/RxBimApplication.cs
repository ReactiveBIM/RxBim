namespace RxBim.Application.Revit
{
    using System;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Events;
    using Di;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;
    using Result = Autodesk.Revit.UI.Result;

    /// <summary>
    /// Revit application.
    /// </summary>
    public abstract class RxBimApplication : IExternalApplication
    {
        private readonly UserInterfaceApplicationProxy _uiApplicationProxy = new();
        private UIControlledApplication _application = null!;
        private IServiceProvider _serviceProvider = null!;

        /// <inheritdoc />
        public Result OnStartup(UIControlledApplication application)
        {
            _application = application;
            var diConfigurator = new ApplicationDiConfigurator(this, application, _uiApplicationProxy);
            diConfigurator.Configure(GetType().Assembly);
            _serviceProvider = diConfigurator.Build();

            application.Idling += ApplicationIdling;

            return Result.Succeeded;
        }

        /// <inheritdoc />
        public Result OnShutdown(UIControlledApplication application)
        {
            var methodCaller = _serviceProvider.GetService<IMethodCaller<PluginResult>>();
            var result = methodCaller.InvokeMethod(_serviceProvider, Constants.ShutdownMethodName);
            return result.MapResultToRevitResult();
        }

        private void ApplicationIdling(object? sender, IdlingEventArgs e)
        {
            if (sender is UIApplication uiApp)
            {
                try
                {
                    if (_uiApplicationProxy.IsInitialized)
                        return;

                    _uiApplicationProxy.Initialize(uiApp);

                    var methodCaller = _serviceProvider.GetService<IMethodCaller<PluginResult>>();
                    methodCaller.InvokeMethod(_serviceProvider, Constants.StartMethodName);
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