namespace PikTools.Command.Autocad.Example
{
    using Abstractions;
    using Di;
    using Logs.Autocad;
    using Services;
    using Shared.Ui;
    using Views;

    /// <inheritdoc />
    public class Config : ICommandConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddTransient<IMyService, MyService>();
            container.AddUi();
            container.AddTransient<MainWindow>();
            container.AddLogs();
        }
    }
}