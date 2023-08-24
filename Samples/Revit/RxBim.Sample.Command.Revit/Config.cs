namespace RxBim.Sample.Command.Revit
{
    using Di;
    using Microsoft.Extensions.Configuration;
    using Models;

    /// <inheritdoc />
    public class Config : ICommandConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddSingleton(() => container.GetService<IConfiguration>()
                    .GetSection(nameof(PluginSettings))
                    .Get<PluginSettings>());
        }
    }
}