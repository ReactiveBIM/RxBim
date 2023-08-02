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
        public void Configure(IServiceCollection container)
        {
            container.AddTransient<IInfoService, InfoService>();
            container.AddAutocadLogs();
        }
    }
}