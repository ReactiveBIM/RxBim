namespace RxBim.Sample.Command.Autocad
{
    using Abstractions;
    using Di;
    using Logs.Autocad;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Services;
    using ViewModels;
    using Views;

    /// <inheritdoc />
    public class Config : ICommandConfiguration
    {
        /// <inheritdoc />
        public void Configure(IServiceCollection services)
        {
            services.AddTransient<ISomeService, SomeService>();
            services.AddTransient<SomeWindow>();
            services.AddTransient<SomeViewModel>();
            services.AddAutocadLogs();

            services.AddSingleton(provider => provider.GetRequiredService<IConfiguration>()
                .GetSection(nameof(PluginSettings))
                .Get<PluginSettings>());
        }
    }
}