namespace RxBim.Application.Autocad.Example
{
    using Di;
    using Logs.Autocad;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc/>
        public void Configure(IContainer container)
        {
            container.AddTransient<IService, Service>();
            container.AddLogs();
        }
    }
}