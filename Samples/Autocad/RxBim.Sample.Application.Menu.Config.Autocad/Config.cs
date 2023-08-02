namespace RxBim.Sample.Application.Menu.Config.Autocad
{
    using Di;
    using Microsoft.Extensions.DependencyInjection;
    using RxBim.Application.Ribbon;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IServiceCollection container)
        {
            container.AddAutocadMenu();
        }
    }
}