#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
namespace RxBim.Application.Revit
{
    using System;
    using System.Reflection;
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
            return ExecuteApplication(assembly, application);
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

        private Result ExecuteApplication(Assembly loadedAssembly, UIControlledApplication application)
        {
            _application = application;
            _diConfigurator = new ApplicationDiConfigurator(this, _application, _uiApplicationProxy);
            _diConfigurator.Configure(loadedAssembly);
            _application.Idling += ApplicationIdling;

            // build container explicitly.
            _diConfigurator.Container.GetService<IServiceLocator>();
            return Result.Succeeded;
        }

        private Result ShutdownApplication()
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