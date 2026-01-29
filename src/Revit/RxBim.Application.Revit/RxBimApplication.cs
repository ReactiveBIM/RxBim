namespace RxBim.Application.Revit
{
    using System;
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

#if RVT2025
        private object? _isolatedApplicationInstance;

        /// <summary>
        /// Allows you to turn off plugin execution in separated context. Might be useful for debugging
        /// via Addin Manager.
        /// </summary>
        protected virtual bool RunInSeparatedContext => true;
#endif

        /// <inheritdoc />
        public Result OnStartup(UIControlledApplication application)
        {
#if RVT2025
            if (RunInSeparatedContext)
            {
                var type = GetType();
                if (PluginContext.IsCurrentContextDefault(type))
                {
                    _isolatedApplicationInstance = PluginContext.CreateInstanceInNewContext(type);
                    if (_isolatedApplicationInstance is IExternalApplication app)
                    {
                        return app.OnStartup(application);
                    }
                }
            }
#endif

            _application = application;
            return ExecuteApplication(application);
        }

        /// <inheritdoc />
        public Result OnShutdown(UIControlledApplication application)
        {
            #if RVT2025
            if (PluginContext.IsCurrentContextDefault(GetType()) && _isolatedApplicationInstance is IExternalApplication app)
            {
                return app.OnShutdown(application);
            }
            #endif

            return ShutdownApplication();
        }

        private Result ExecuteApplication(UIControlledApplication application)
        {
#if RVT2025
            var diConfigurator = new ApplicationDiConfigurator(this, application, _uiApplicationProxy, !RunInSeparatedContext);
#else
            var diConfigurator = new ApplicationDiConfigurator(this, application, _uiApplicationProxy);
#endif
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