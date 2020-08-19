namespace PikTools.Di.Tests
{
    using TestDependencies;

    public class TestDiConfigurator : DiConfigurator
    {
        protected override void ConfigureBaseDependencies()
        {
            Container.Register<IBaseService, BaseService>();
        }
    }
}