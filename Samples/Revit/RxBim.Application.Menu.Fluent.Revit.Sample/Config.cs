namespace RxBim.Application.Menu.Fluent.Revit.Sample
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
            container.AddRevitMenu(ribbon => ribbon
                .EnableDisplayVersion()
                .VersionPrefix("Version: ")
                .TabFromAttributes()
                .TabFromBuilder());
        }
    }
}