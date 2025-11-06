namespace RxBim.Application.Autocad
{
    using System.Reflection;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Di;
    using Microsoft.Extensions.DependencyInjection;
    using Shared;

    /// <summary>
    /// Autocad application DI configurator.
    /// </summary>
    public class ApplicationDiConfigurator : DiConfigurator<IApplicationConfiguration>
    {
        private readonly object _applicationObject;
        private readonly bool _addResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDiConfigurator"/> class.
        /// </summary>
        /// <param name="applicationObject">application object.</param>
        /// <param name="addResolver">Allows you to turn off plugin adding assembly resolver in DI.</param>
        public ApplicationDiConfigurator(object applicationObject, bool addResolver = true)
        {
            _applicationObject = applicationObject;
            _addResolver = addResolver;
        }

        /// <inheritdoc />
        protected override void ConfigureAdditionalDependencies(Assembly assembly)
        {
            base.ConfigureAdditionalDependencies(assembly);

            if (_addResolver)
            {
                Services
                    .AddTransient(_ => new AssemblyResolver(assembly))
                    .Decorate(typeof(IMethodCaller<>), typeof(AssemblyResolveMethodCaller<>));
            }
        }

        /// <inheritdoc />
        protected override void ConfigureBaseDependencies()
        {
            Services
                .AddSingleton(Application.DocumentManager)
                .AddTransient<IMethodCaller<PluginResult>>(_ => new MethodCaller<PluginResult>(_applicationObject));
        }
    }
}