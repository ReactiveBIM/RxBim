namespace RxBim.Sample.Command.Revit
{
    using Di;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Models;

    /// <inheritdoc />
    public class Config : ICommandConfiguration
    {
        /// <inheritdoc />
        public void Configure(IServiceCollection container)
        {
            container.AddSingleton(provider => provider.GetRequiredService<IConfiguration>()
                    .GetSection(nameof(PluginSettings))
                    .Get<PluginSettings>());
        }
    }
}