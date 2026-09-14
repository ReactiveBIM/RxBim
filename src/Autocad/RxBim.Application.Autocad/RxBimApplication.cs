#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
namespace RxBim.Application.Autocad
{
    using System;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Autodesk.AutoCAD.Internal;
    using Autodesk.AutoCAD.Runtime;
    using Di;
    using Microsoft.Extensions.DependencyInjection;
    using Ribbon;
    using Shared;
    using Exception = System.Exception;

    /// <inheritdoc />
    public abstract class RxBimApplication : IExtensionApplication
    {
        private ApplicationDiConfigurator? _diConfigurator;
        private IServiceProvider _serviceProvider = null!;

#if NETCOREAPP
        /// <summary>
        /// Allows you to turn off plugin execution in separated context. Might be useful for debugging
        /// via Addin Manager.
        /// </summary>
        protected virtual bool RunInSeparatedContext => false;

        /// <summary>
        /// Reuses the context for the application's DLL directory until AutoCAD exits.
        /// Applies only when <see cref="RunInSeparatedContext"/> is enabled.
        /// Enable this setting for the application's commands as well to share the same context.
        /// </summary>
        protected virtual bool ReuseSeparatedContext => false;
#endif

        /// <inheritdoc />
        public void Initialize()
        {
#if NETCOREAPP
            if (RunInSeparatedContext)
            {
                var type = GetType();
                if (!PluginContext.IsCurrentContextRxBim(type))
                {
                    if (ReuseSeparatedContext)
                    {
                        InitializeInReusedContext(type);
                        return;
                    }

                    var appInstance = PluginContext.CreateInstanceInNewContext(type);
                    if (appInstance is IExtensionApplication application)
                    {
                        application.Initialize();
                        return;
                    }
                }
            }
#endif

            BeforeStartAction();
            Application.Idle += ApplicationOnIdle;
            Application.QuitWillStart += ApplicationOnQuitWillStart;
        }

        /// <summary>
        /// Invokes some logic before application execution.
        /// </summary>
        public virtual void BeforeStartAction()
        {
        }

        /// <inheritdoc />
        public void Terminate()
        {
            // ignore
        }

        /// <summary>
        /// If it returns true, the application will run. Otherwise, the application will not run.
        /// </summary>
        protected virtual bool CanBeStarted() => true;

#if NETCOREAPP
        private void InitializeInReusedContext(Type type)
        {
            IExtensionApplication application;

            try
            {
                application = (IExtensionApplication)PluginContext.CreateInstanceInReusedContext(type);
            }
            catch (Exception exception)
            {
                // Report loading failures without initializing in the original context.
                Application.ShowAlertDialog($"Error: {exception}");
                return;
            }

            application.Initialize();
        }
#endif

        private void ApplicationOnIdle(object? sender, EventArgs e)
        {
            Application.Idle -= ApplicationOnIdle;

            try
            {
                if (_diConfigurator is not null || !CanBeStarted())
                    return;

#if NETCOREAPP
                _diConfigurator = new ApplicationDiConfigurator(this, !RunInSeparatedContext);
#else
                _diConfigurator = new ApplicationDiConfigurator(this);
#endif
                _diConfigurator.Configure(GetType().Assembly);
                _serviceProvider = _diConfigurator.Build();

                MenuBuilderUtility.BuildMenu(_serviceProvider);

                var methodCaller = _serviceProvider.GetRequiredService<IMethodCaller<PluginResult>>();
                methodCaller.InvokeMethod(_serviceProvider, Constants.StartMethodName);
            }
            catch (Exception exception)
            {
                Application.ShowAlertDialog($"Error: {exception}");
            }
        }

        private void ApplicationOnQuitWillStart(object? sender, EventArgs e)
        {
            Application.QuitWillStart -= ApplicationOnQuitWillStart;

            if (_diConfigurator is null)
                return;

            try
            {
                var methodCaller = _serviceProvider.GetRequiredService<IMethodCaller<PluginResult>>();
                methodCaller.InvokeMethod(_serviceProvider, Constants.ShutdownMethodName);
            }
            catch (Exception exception)
            {
                Application.ShowAlertDialog($"Error: {exception}");
            }
            finally
            {
                (_serviceProvider as IDisposable)?.Dispose();
            }
        }
    }
}