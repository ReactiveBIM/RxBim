namespace PikTools.Application.Menu.Fluent.Example
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Di;
    using PikTools.Shared;
    using PikTools.Shared.Abstractions;
    using PikTools.Shared.Ui;
    using Ui.Api;

    /// <inheritdoc />
    public class MyConfig : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddUi();
            container.AddMenu(ribbon => ribbon
                .Tab("First")
                    .AboutButton(
                        "О программе",
                        "О программе",
                        "О программе",
                        button => button
                            .SetViewer(container.GetService<IAboutBox>())
                            .SetContent(new AboutBoxContent(
                                "PIKTools4Revit",
                                "21.1",
                                $"ПИК-АР - Модуль продукта PIKTools, автоматизирующий процесс проектирования для архитекторов{Environment.NewLine}Разработано для Autodesk Revit 2019",
                                GetType().Assembly.GetName().Version,
                                "ПИК-Digital",
                                new List<KeyValuePair<string, string>>
                                {
                                    new KeyValuePair<string, string>("Скачать актуальные версии плагинов", "https://drive.google.com/drive/folders/1v-KbQEKv7roJctcWSCodsFQy3KwSz_rt"),
                                    new KeyValuePair<string, string>("Сайт", "https://pikipedia.pik.ru/wiki/PIK_Tools"),
                                    new KeyValuePair<string, string>("Канал в Telegram", "https://t.me/PIKTools_News")
                                }))
                            .SetSmallImage(
                                    new Uri(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), @"img\small.png"),
                                    UriKind.Absolute))
                            .SetLargeImage(
                                    new Uri(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), @"img\large.png"),
                                    UriKind.Absolute)))
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