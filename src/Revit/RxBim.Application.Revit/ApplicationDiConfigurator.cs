namespace RxBim.Application.Revit
{
    using System;
    using System.Reflection;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Events;
    using Di;
    using Shared;

    /// <summary>
    /// Revit application DI container configurator.
    /// </summary>
    internal class ApplicationDiConfigurator : DiConfigurator<IApplicationConfiguration>
    {
        private readonly object _applicationObject;
        private readonly UIControlledApplication _uiControlledApp;
        private readonly UIApplicationProxy _uIApplicationProxy;

        /// <summary>
        /// Initialize a new instance of <see cref="ApplicationDiConfigurator"/>.
        /// </summary>
        /// <param name="applicationObject">application object.</param>
        /// <param name="uiControlledApp">Revit ui controlled application.</param>
        public ApplicationDiConfigurator(
            object applicationObject,
            UIControlledApplication uiControlledApp)
        {
            _applicationObject = applicationObject;
            _uiControlledApp = uiControlledApp;
            _uIApplicationProxy = new();

            uiControlledApp.Idling += ApplicationIdling;
        }

        /// <summary>
        /// Revit services initialization end event - meaning that the container is ready for operation.
        /// </summary>
        public event EventHandler? RevitServicesReady;

        /// <inheritdoc/>
        protected override void ConfigureAdditionalDependencies(Assembly assembly)
        {
            base.ConfigureAdditionalDependencies(assembly);

            Container
                .AddTransient(() => new AssemblyResolver(assembly))
                .Decorate(typeof(IMethodCaller<>), typeof(AssemblyResolveMethodCaller<>));
        }

        /// <inheritdoc />
        protected override void ConfigureBaseDependencies()
        {
            Container
                .AddInstance(_uiControlledApp)
                .AddSingleton(() => _uIApplicationProxy.UIApplication)
                .AddSingleton(() => _uIApplicationProxy.UIApplication.Application)
                .AddTransient(() => _uIApplicationProxy.UIApplication.ActiveUIDocument)
                .AddTransient(() => _uIApplicationProxy.UIApplication.ActiveUIDocument?.Document!)
                .AddTransient<IMethodCaller<PluginResult>>(() => new MethodCaller<PluginResult>(_applicationObject));
        }

        private void ApplicationIdling(object sender, IdlingEventArgs e)
        {
            if (sender is UIApplication uiApp)
            {
                try
                {
                    if (_uIApplicationProxy.IsInitialized)
                        return;

                    _uIApplicationProxy.Initialize(uiApp);
                    RevitServicesReady?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception exception)
                {
                    TaskDialog.Show("Error", exception.ToString());
                    throw;
                }
                finally
                {
                    _uiControlledApp.Idling -= ApplicationIdling;
                }
            }
        }
    }
}