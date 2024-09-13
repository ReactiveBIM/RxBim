namespace RxBim.Application.Autocad
{
    using System.Reflection;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Di;
    using Shared;

    /// <summary>
    /// Autocad application DI configurator.
    /// </summary>
    public class ApplicationDiConfigurator : DiConfigurator<IApplicationConfiguration>
    {
        private readonly object _applicationObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDiConfigurator"/> class.
        /// </summary>
        /// <param name="applicationObject">application object.</param>
        public ApplicationDiConfigurator(object applicationObject)
        {
            _applicationObject = applicationObject;
        }

        /// <inheritdoc />
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
                .AddInstance(Application.DocumentManager)
                .AddTransient<IMethodCaller<PluginResult>>(() => new MethodCaller<PluginResult>(_applicationObject));
        }
    }
}