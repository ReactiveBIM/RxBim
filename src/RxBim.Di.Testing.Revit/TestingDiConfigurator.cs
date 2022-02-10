namespace RxBim.Di.Testing.Revit
{
    using Autodesk.Revit.UI;

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
            Container
                .AddInstance(_commandData)
                .AddInstance(_commandData.Application)
                .AddInstance(_commandData.Application.Application)
                .AddTransient(() => _commandData.Application.ActiveUIDocument)
                .AddTransient(() => _commandData.Application.ActiveUIDocument?.Document);
        }
    }
}