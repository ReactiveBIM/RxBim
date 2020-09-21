namespace PikTools.Application.Config.Menu.Example
{
    using Di;
    using SimpleInjector;
    using Ui.Api;

    /// <inheritdoc />
    public class MyConfig : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(Container container)
        {
            container.AddMenu();
        }
    }
}