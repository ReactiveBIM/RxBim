namespace RxBim.Application.Revit
{
    using System.Reflection;
    using Autodesk.Revit.UI;
    using Di;
    using Di.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;

    /// <summary>
    /// Revit application DI container configurator.
    /// </summary>
    internal class ApplicationDiConfigurator : DiConfigurator<IApplicationConfiguration>
    {
        private readonly object _applicationObject;
        private readonly UIControlledApplication _uiControlledApp;
        private readonly UIApplication _uiApp;

        /// <summary>
        /// Initialize a new instance of <see cref="ApplicationDiConfigurator"/>.
        /// </summary>
        /// <param name="applicationObject">application object.</param>
        /// <param name="uiControlledApp">Revit ui controlled application.</param>
        /// <param name="uiApp">Revit ui application.</param>
        public ApplicationDiConfigurator(
            object applicationObject,
            UIControlledApplication uiControlledApp,
            UIApplication uiApp)
        {
            _applicationObject = applicationObject;
            _uiControlledApp = uiControlledApp;
            _uiApp = uiApp;
        }

        /// <inheritdoc/>
        public override void Configure(Assembly assembly)
        {
            base.Configure(assembly);

            Container.Services
                .AddTransient(_ => new AssemblyResolver(assembly))
                .Decorate(typeof(IMethodCaller<>), typeof(AssemblyResolveMethodCaller<>));
        }

        /// <inheritdoc />
        protected override void ConfigureBaseDependencies()
        {
            Container.Services
                .AddInstance(_uiControlledApp)
                .AddInstance(_uiApp)
                .AddInstance(_uiApp.Application)
                .AddTransient(_ => _uiApp.ActiveUIDocument)
                .AddTransient(_ => _uiApp.ActiveUIDocument?.Document!)
                .AddTransient<IMethodCaller<PluginResult>>(_ => new MethodCaller<PluginResult>(_applicationObject));
        }
    }
}