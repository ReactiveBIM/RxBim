namespace RxBim.Sample.Application.Menu.Fluent.Revit
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
            container.AddRevitMenu(ribbon => ribbon
                .EnableDisplayVersion()
                .SetVersionPrefix("Version: ")
                .TabFromAttributes()
                .TabFromBuilder());
        }
    }
}