namespace RxBim.Di.Testing.Revit
{
    using Autodesk.Revit.UI;
    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class TestingDiConfigurator : DiConfigurator<ITestConfiguration>
    {
        private readonly ExternalCommandData _commandData;

        /// <inheritdoc />
        public TestingDiConfigurator(ExternalCommandData commandData)
        {
            _commandData = commandData;
        }

        /// <inheritdoc />
        protected override void ConfigureBaseDependencies()
        {
            Services
                .AddSingleton(_commandData)
                .AddSingleton(_commandData.Application)
                .AddSingleton(_commandData.Application.Application)
                .AddTransient(_ => _commandData.Application.ActiveUIDocument)
                .AddTransient(_ => _commandData.Application.ActiveUIDocument?.Document!);
        }
    }
}