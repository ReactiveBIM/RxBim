namespace RxBim.Application.Menu.Config.Autocad.Sample
{
    using Di;
    using Ribbon;
    using Shared.Abstractions;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddSingleton<IAboutShowService, AboutShowService>();
            container.AddAutocadMenu();
        }
    }
}