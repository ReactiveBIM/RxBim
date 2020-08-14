namespace PikTools.Application.Example
{
    using Di;
    using Logs;
    using Microsoft.Extensions.Configuration;
    using SimpleInjector;

    /// <inheritdoc />
    public class MyConfig : IPluginConfiguration
    {
        /// <inheritdoc />
        public void Configure(Container container)
        {
            var cfg = new ConfigurationBuilder()
                .AddJsonFile("application.settings.json")
                .Build();

            container.Register<IService, Service>();
            container.AddLogs();
        }
    }
}