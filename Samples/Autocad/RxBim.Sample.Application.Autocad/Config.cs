namespace RxBim.Sample.Application.Autocad
{
    using Abstractions;
    using Di;
    using Logs.Autocad;
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc/>
        public void Configure(IServiceCollection services)
        {
            services.AddTransient<IInfoService, InfoService>();
            services.AddAutocadLogs();
        }
    }
}