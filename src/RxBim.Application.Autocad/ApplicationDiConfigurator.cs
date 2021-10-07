namespace RxBim.Application.Autocad
{
    using System.Reflection;
    using Autodesk.AutoCAD.ApplicationServices.Core;
    using Di;
    using Shared;

    /// <summary>
    /// Конфигуратор зависимостей приложения
    /// </summary>
    public class ApplicationDiConfigurator : DiConfigurator<IApplicationConfiguration>
    {
        private readonly object _applicationObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDiConfigurator"/> class.
        /// </summary>
        /// <param name="applicationObject">Объект приложения</param>
        public ApplicationDiConfigurator(object applicationObject)
        {
            _applicationObject = applicationObject;
        }

        /// <inheritdoc />
        public override void Configure(Assembly assembly)
        {
            base.Configure(assembly);

            Container
                .AddTransient(() => new AssemblyResolver(assembly))
                .Decorate(typeof(IMethodCaller<>), typeof(AssemblyResolveMethodCaller));
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