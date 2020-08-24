namespace PikTools.Di.Tests.TestDependencies
{
    using SimpleInjector;

    public class TestPluginConfiguration : IApplicationConfiguration
    {
        public void Configure(Container container)
        {
            container.Register<IPluginService, PluginService>();
        }
    }
}