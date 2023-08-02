namespace RxBim.Di.Tests
{
    using TestDependencies;

    public class TestDiConfigurator : DiConfigurator<IPluginConfiguration>
    {
        protected override void ConfigureBaseDependencies()
        {
            Services.AddTransient<IBaseService, BaseService>();
        }
    }
}