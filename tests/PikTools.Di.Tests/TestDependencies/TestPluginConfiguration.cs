namespace PikTools.Di.Tests.TestDependencies
{
    public class TestPluginConfiguration : IApplicationConfiguration
    {
        public void Configure(IContainer container)
        {
            container.AddTransient<IPluginService, PluginService>();
        }
    }
}