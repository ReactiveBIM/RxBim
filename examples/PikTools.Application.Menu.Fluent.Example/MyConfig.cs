namespace PikTools.Application.Menu.Fluent.Example
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
            container.AddMenu(ribbon => ribbon
                .Tab("First")
                    .Panel("Panel1")
                        .Button("Button1", "Go", typeof(MyCmd))
                .And()
                .Tab("Second")
                        .Panel("Panel2"));
        }
    }
}