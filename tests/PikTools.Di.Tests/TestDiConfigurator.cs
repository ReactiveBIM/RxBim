namespace PikTools.Di.Tests
{
    using TestDependencies;

    public class TestDiConfigurator : DiConfigurator<IPluginConfiguration>
    {
        protected override void ConfigureBaseDependencies()
        {
            Container.AddTransient<IBaseService, BaseService>();
        }
    }
}