namespace RxBim.Sample.Application.Menu.Config.Revit
{
    using Di;
    using RxBim.Application.Ribbon;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddRevitMenu();
        }
    }
}