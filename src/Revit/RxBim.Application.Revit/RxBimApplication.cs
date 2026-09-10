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

#if NETCOREAPP
        private object? _isolatedApplicationInstance;

        /// <summary>
        /// Enables plugin execution in an isolated context managed by RxBim.
        /// In Revit 2026 and newer, set it to <see langword="false"/> to let the manifest select
        /// Revit's context. For the RxBim context, <c>UseRevitContext=true</c> or an omitted setting
        /// is preferred to avoid nesting it inside Revit's isolated context.
        /// </summary>
        protected virtual bool RunInSeparatedContext => false;
#endif

        /// <inheritdoc />
        public Result OnStartup(UIControlledApplication application)
        {
#if NETCOREAPP
            if (RunInSeparatedContext)
            {
                var type = GetType();
                if (!PluginContext.IsCurrentContextRxBim(type))
                {
                    _isolatedApplicationInstance = PluginContext.CreateInstanceInNewContext(type);
                    if (_isolatedApplicationInstance is IExternalApplication app)
                    {
                        return app.OnStartup(application);
                    }
                }
            }
#endif

            BeforeStartAction();
            _application = application;
            return ExecuteApplication(application);
        }

        /// <summary>
        /// Invokes some logic before application execution.
        /// </summary>
        public virtual void BeforeStartAction()
        {
        }

        /// <inheritdoc />
        public Result OnShutdown(UIControlledApplication application)
        {
#if NETCOREAPP
            if (_isolatedApplicationInstance is IExternalApplication app)
            {
                return app.OnShutdown(application);
            }
#endif

            return ShutdownApplication();
        }

        private Result ExecuteApplication(UIControlledApplication application)
        {
#if NETCOREAPP
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
            try
            {
                var methodCaller = _serviceProvider.GetService<IMethodCaller<PluginResult>>();
                var result = methodCaller.InvokeMethod(_serviceProvider, Constants.ShutdownMethodName);
                return result.MapResultToRevitResult();
            }
            finally
            {
                (_serviceProvider as IDisposable)?.Dispose();
            }
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