namespace RxBim.Application.Menu.Config.Autocad.Sample
{
    using Di;
    using Ribbon.Autocad.Extensions;
    using Shared.Abstractions;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddAutocadMenu();
        }
    }
}