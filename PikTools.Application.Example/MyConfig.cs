namespace PikTools.Application.Example
{
    using System.IO;
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
                .SetBasePath(Path.GetDirectoryName(GetType().Assembly.Location))
                .AddJsonFile("application.settings.json")
                .Build();

            container.Register<IService, Service>();
            container.AddLogs();
        }
    }
}