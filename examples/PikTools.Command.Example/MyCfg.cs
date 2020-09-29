namespace PikTools.CommandExample
{
    using Di;
    using PikTools.CommandExample.Abstractions;
    using PikTools.CommandExample.Services;
    using PikTools.CommandExample.ViewModels;
    using PikTools.CommandExample.Views;
    using PikTools.Logs;
    using PikTools.Shared.RevitExtensions;
    using PikTools.Shared.Ui;
    using Shared;
    using SimpleInjector;

    /// <inheritdoc />
    public class MyCfg : ICommandConfiguration
    {
        /// <inheritdoc />
        public void Configure(Container container)
        {
            container.Register<IMyService, MyService>();
            container.AddRevitHelpers();
            container.AddUi();
            container.AddSharedTools();

            container.Register<MainWindowViewModel>();
            container.Register<MainWindow>();
        }
    }
}