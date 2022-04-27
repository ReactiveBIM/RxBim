namespace RxBim.Application.Menu.Fluent.Revit.Sample
{
    using Di;
    using Ribbon;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddRevitMenu(ribbon => ribbon
                .DisplayVersion(true)
                .VersionPrefix("Version: ")
                .TabFromAttributes()
                .TabFromBuilder());
        }
    }
}