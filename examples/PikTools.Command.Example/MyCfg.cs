namespace PikTools.CommandExample
{
    using Di;
    using Microsoft.Extensions.Configuration;
    using PikTools.CommandExample.Abstractions;
    using PikTools.CommandExample.Services;
    using PikTools.CommandExample.ViewModels;
    using PikTools.CommandExample.Views;
    using PikTools.Shared.FmHelpers;
    using PikTools.Shared.RevitExtensions;
    using PikTools.Shared.Ui;
    using Shared;

    /// <inheritdoc />
    public class MyCfg : ICommandConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddTransient<IMyService, MyService>();
            container.AddRevitHelpers();
            container.AddUi();
            container.AddSharedTools();
            container.AddFmHelpers(container.GetService<IConfiguration>);

            container.AddTransient<MainWindowViewModel>();
            container.AddTransient<MainWindow>();
        }
    }
}