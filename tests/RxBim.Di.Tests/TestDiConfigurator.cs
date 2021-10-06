namespace RxBim.Di.Tests
{
    using Shared;
    using TestDependencies;

    public class TestDiConfigurator : DiConfigurator<IPluginConfiguration, SimpleInjectorContainer>
    {
        protected override void ConfigureBaseDependencies()
        {
            Container.AddTransient<IBaseService, BaseService>();
            Container.AddSharedTools();
        }
    }
}