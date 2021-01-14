namespace PikTools.Application.Config.Menu.Example
{
    using Di;
    using Ui.Api;

    /// <inheritdoc />
    public class MyConfig : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddMenu();
        }
    }
}