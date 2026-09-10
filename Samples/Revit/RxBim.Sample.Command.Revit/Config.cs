namespace RxBim.Sample.Command.Revit
{
    using System;
    using Di;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Models;

    /// <inheritdoc />
    public class Config : ICommandConfiguration
    {
        /// <inheritdoc />
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton(sp => sp.GetRequiredService<IConfiguration>()
                .GetSection(nameof(PluginSettings))
                .Get<PluginSettings>()
                ?? throw new InvalidOperationException($"Configuration section '{nameof(PluginSettings)}' is missing or empty."));
        }
    }
}