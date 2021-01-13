namespace PikTools.Application.Menu.Fluent.Example
{
    using System;
    using System.IO;
    using Di;
    using Ui.Api;

    /// <inheritdoc />
    public class MyConfig : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddMenu(ribbon => ribbon
                .Tab("First")
                    .Panel("Panel1")
                        .Button(
                            "Button1",
                            "Go",
                            typeof(MyCmd),
                            button => button
                                .SetSmallImage(
                                    new Uri(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), @"img\small.png"),
                                    UriKind.Absolute))
                                .SetLargeImage(
                                    new Uri(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), @"img\large.png"),
                                    UriKind.Absolute))
                                .SetLongDescription("Button1 description")
                                .SetToolTip("Example button")
                                .SetHelpUrl("https://pikipedia.pik.ru/wiki/PIK_Tools")));
        }
    }
}