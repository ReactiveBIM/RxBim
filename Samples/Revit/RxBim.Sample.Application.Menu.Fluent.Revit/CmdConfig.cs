namespace RxBim.Sample.Application.Menu.Fluent.Revit
{
    using Di;
    using Microsoft.Extensions.DependencyInjection;
    using PikTools.Ui;
    using RxBim.Application.Ribbon;

    /// <inheritdoc />
    public class CmdConfig : ICommandConfiguration
    {
        /// <inheritdoc />
        public void Configure(IServiceCollection services)
        {
            services.AddUi();
        }
    }
}