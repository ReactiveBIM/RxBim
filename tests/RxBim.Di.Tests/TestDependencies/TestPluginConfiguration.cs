namespace RxBim.Di.Tests.TestDependencies
{
    using Microsoft.Extensions.DependencyInjection;

    public class TestPluginConfiguration : IApplicationConfiguration
    {
        public void Configure(IServiceCollection services)
        {
            services.AddTransient<IPluginService, PluginService>();
        }
    }
}