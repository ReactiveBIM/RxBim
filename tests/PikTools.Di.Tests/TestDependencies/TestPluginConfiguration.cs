namespace PikTools.Di.Tests.TestDependencies
{
    using SimpleInjector;

    public class TestPluginConfiguration : IPluginConfiguration
    {
        public void Configure(Container container)
        {
            container.Register<IPluginService, PluginService>();
        }
    }
}