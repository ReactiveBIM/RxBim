namespace RxBim.Application.Revit
{
    using System.Reflection;
    using Autodesk.Revit.UI;
    using Di;
    using Shared;

    /// <summary>
    /// Конфигуратор зависимостей приложения
    /// </summary>
    internal class ApplicationDiConfigurator : DiConfigurator<IApplicationConfiguration, SimpleInjectorContainer>
    {
        private readonly object _applicationObject;
        private readonly UIControlledApplication _uiControlledApp;
        private readonly UIApplication _uiApp;

        /// <summary>
        /// Создает экземпляр класса <see cref="ApplicationDiConfigurator"/>
        /// </summary>
        /// <param name="applicationObject">Объект приложения</param>
        /// <param name="uiControlledApp">Пользовательский интерфейс Revit</param>
        /// <param name="uiApp">Активная сессия пользовательского интерфейса Revit</param>
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

            Container
                .AddTransient(() => new AssemblyResolver(assembly))
                .Decorate(typeof(IMethodCaller<>), typeof(AssemblyResolveMethodCaller));
        }

        /// <inheritdoc />
        protected override void ConfigureBaseDependencies()
        {
            Container
                .AddInstance(_uiControlledApp)
                .AddInstance(_uiApp)
                .AddInstance(_uiApp.Application)
                .AddTransient(() => _uiApp.ActiveUIDocument)
                .AddTransient(() => _uiApp.ActiveUIDocument?.Document)
                .AddTransient<IMethodCaller<PluginResult>>(() => new MethodCaller<PluginResult>(_applicationObject));
        }
    }
}