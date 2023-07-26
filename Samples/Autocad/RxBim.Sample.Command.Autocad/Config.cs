namespace RxBim.Sample.Command.Autocad
{
    using Abstractions;
    using Di;
    using Logs.Autocad;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Services;
    using ViewModels;
    using Views;

    /// <inheritdoc />
    public class Config : ICommandConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddTransient<ISomeService, SomeService>();
            container.AddTransient<SomeWindow>();
            container.AddTransient<SomeViewModel>();
            container.AddLogs();

            container.AddSingleton(() => container.GetService<IConfiguration>()
                .GetSection(nameof(PluginSettings))
                .Get<PluginSettings>());
        }
    }
}