namespace RxBim.Application.Revit
{
    using System.Reflection;
    using Autodesk.Revit.UI;
    using Di;
    using Shared;

    /// <summary>
    /// Revit application DI container configurator.
    /// </summary>
    internal class ApplicationDiConfigurator : DiConfigurator<IApplicationConfiguration>
    {
        private readonly object _applicationObject;
        private readonly UIControlledApplication _uiControlledApp;
        private readonly UserInterfaceApplicationProxy _uiApplicationProxy;

        /// <summary>
        /// Initialize a new instance of <see cref="ApplicationDiConfigurator"/>.
        /// </summary>
        /// <param name="applicationObject">application object.</param>
        /// <param name="uiControlledApp">Revit ui controlled application.</param>
        /// <param name="uiApplicationProxy">Proxy for Revit ui application.</param>
        public ApplicationDiConfigurator(
            object applicationObject,
            UIControlledApplication uiControlledApp,
            UserInterfaceApplicationProxy uiApplicationProxy)
        {
            _applicationObject = applicationObject;
            _uiControlledApp = uiControlledApp;
            _uiApplicationProxy = uiApplicationProxy;
        }

        /// <inheritdoc/>
        protected override void ConfigureAdditionalDependencies(Assembly assembly)
        {
            base.ConfigureAdditionalDependencies(assembly);

            #if !NETCOREAPP
            Container
                .AddTransient(() => new AssemblyResolver(assembly))
                .Decorate(typeof(IMethodCaller<>), typeof(AssemblyResolveMethodCaller<>));
            #endif
        }

        /// <inheritdoc />
        protected override void ConfigureBaseDependencies()
        {
            Container
                .AddInstance(_uiControlledApp)
                .AddSingleton(() => _uiApplicationProxy.Application)
                .AddSingleton(() => _uiApplicationProxy.Application.Application)
                .AddTransient(() => _uiApplicationProxy.Application.ActiveUIDocument)
                .AddTransient(() => _uiApplicationProxy.Application.ActiveUIDocument?.Document!)
                .AddTransient<IMethodCaller<PluginResult>>(() => new MethodCaller<PluginResult>(_applicationObject));
        }
    }
}