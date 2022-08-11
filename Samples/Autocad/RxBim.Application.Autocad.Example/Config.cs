namespace RxBim.Application.Autocad.Example
{
    using Abstractions;
    using Di;
    using Logs.Autocad;
    using Services;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc/>
        public void Configure(IContainer container)
        {
            container.AddTransient<IInfoService, InfoService>();
            container.AddLogs();
        }
    }
}