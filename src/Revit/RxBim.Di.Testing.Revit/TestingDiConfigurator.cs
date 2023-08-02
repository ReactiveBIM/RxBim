namespace RxBim.Di.Testing.Revit
{
    using Autodesk.Revit.UI;
    using Extensions;
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
                .AddInstance(_commandData)
                .AddInstance(_commandData.Application)
                .AddInstance(_commandData.Application.Application)
                .AddTransient(_ => _commandData.Application.ActiveUIDocument)
                .AddTransient(_ => _commandData.Application.ActiveUIDocument?.Document!);
        }
    }
}