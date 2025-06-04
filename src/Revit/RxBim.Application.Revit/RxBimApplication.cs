namespace RxBim.Application.Revit
{
    using System;
    using System.Reflection;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Events;
    using Di;
    using Microsoft.Extensions.DependencyInjection;
    using Ribbon;
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

#if NETCOREAPP
        private object? _isolatedApplicationInstance;
#endif

        /// <inheritdoc />
        public Result OnStartup(UIControlledApplication application)
        {
            var type = GetType();
            var assembly = type.Assembly;

#if NETCOREAPP
            if (PluginContext.IsCurrentContextDefault(type))
            {
                _isolatedApplicationInstance = PluginContext.CreateInstance(type);
                if (_isolatedApplicationInstance is IExternalApplication app)
                {
                    return app.OnStartup(application);
                }
            }
#endif

            _application = application;
            return ExecuteApplication(application);
        }

        /// <inheritdoc />
        public Result OnShutdown(UIControlledApplication application)
        {
            #if NETCOREAPP
            if (PluginContext.IsCurrentContextDefault(GetType()) && _isolatedApplicationInstance is IExternalApplication app)
            {
                return app.OnShutdown(application);
            }
            #endif

            return ShutdownApplication();
        }

        private Result ExecuteApplication(UIControlledApplication application)
        {
            var diConfigurator = new ApplicationDiConfigurator(this, application, _uiApplicationProxy);
            diConfigurator.Configure(GetType().Assembly);
            _serviceProvider = diConfigurator.Build();

            MenuBuilderUtility.BuildMenu(_serviceProvider);

            application.Idling += ApplicationIdling;
            return Result.Succeeded;
        }

        private Result ShutdownApplication()
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