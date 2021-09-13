namespace RxBim.Di.Tests
{
    using TestDependencies;

    public class TestDiConfigurator : DiConfigurator<IPluginConfiguration, SimpleInjectorContainer>
    {
        protected override void ConfigureBaseDependencies()
        {
            Container.AddTransient<IBaseService, BaseService>();
        }
    }
}