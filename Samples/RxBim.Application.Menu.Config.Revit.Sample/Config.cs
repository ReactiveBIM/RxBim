namespace RxBim.Application.Menu.Config.Revit.Sample
{
    using Di;
    using Ribbon.Revit;

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