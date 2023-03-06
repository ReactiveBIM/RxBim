namespace RxBim.Sample.Application.Menu.Fluent.Revit
{
    using Di;
    using RxBim.Application.Ribbon;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddRevitMenu(ribbon => ribbon
                .EnableDisplayVersion()
                .SetVersionPrefix("Version: ")
                .TabFromAttributes()
                .TabFromBuilder());
        }
    }
}