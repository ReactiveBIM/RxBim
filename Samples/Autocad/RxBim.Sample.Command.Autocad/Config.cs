namespace RxBim.Sample.Command.Autocad
{
    using Abstractions;
    using Di;
    using Logs.Autocad;
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
        }
    }
}