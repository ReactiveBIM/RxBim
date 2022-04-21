namespace RxBim.Application.Menu.Fluent.Revit.Sample
{
    using System.Collections.Generic;
    using Di;
    using Ribbon.Revit;
    using Shared;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddRevitMenu(ribbon => ribbon
                .SetAddVersionToCommandTooltip(true)
                .SetCommandTooltipVersionHeader("Version: ")
                .AddTab("RxBim_Tab_FromAction")
                .AddAboutButton(
                    "About",
                    new AboutBoxContent(
                        "RxBim4Revit",
                        "1.0",
                        "RxBim product module for API demo and validation",
                        GetType().Assembly.GetName().Version,
                        "ReactiveBIM",
                        new Dictionary<string, string>
                        {
                            { "Examples", "https://github.com/ReactiveBIM/RxBim.Examples" }
                        }),
                    button => button
                        .SetText("About\nbutton")
                        .SetToolTip("About information")
                        .SetDescription("Button for displaying the About window")
                        .SetLargeImage(@"img\about_32.png"))
                .AddPanel("RxBim_Panel_1")
                .AddCommandButton(
                    "Command1_Large_WithText",
                    typeof(Cmd1),
                    button => button
                        .SetToolTip("Tooltip: I'm run command #1. Push me!")
                        .SetText("Command\n#1")
                        .SetDescription("Description: This is command #1")
                        .SetLargeImage(@"img\num1_32.png")
                        .SetHelpUrl("https://github.com/ReactiveBIM/RxBim"))
                .AddCommandButton(
                    "Command2_Large_WithText",
                    typeof(Cmd2),
                    button => button
                        .SetToolTip("Tooltip: I'm run command #2. Push me!")
                        .SetText("Command\n#2")
                        .SetDescription("Description: This is command #2")
                        .SetSmallImage(@"img\num2_16.png")
                        .SetLargeImage(@"img\num2_32.png")
                        .SetHelpUrl("https://www.google.com/"))
                .AddCommandButton(
                    "Command3_Large_WithText",
                    typeof(Cmd3),
                    button => button
                        .SetToolTip("Tooltip: I'm run command #3. Push me!")
                        .SetText("Command\n#3")
                        .SetDescription("Description: This is command #3")
                        .SetSmallImage(@"img\num3_16.png")
                        .SetLargeImage(@"img\num3_32.png")
                        .SetHelpUrl("https://www.autodesk.com/"))
                .AddSeparator()
                .AddPullDownButton(
                    "Pulldown1",
                    pulldown => pulldown
                        .AddCommandButton(
                            "Command1_Pulldown1",
                            typeof(Cmd1),
                            button => button
                                .SetToolTip("Tooltip: I'm run command #1. Push me!")
                                .SetText("Command\n#1")
                                .SetDescription("Description: This is command #1")
                                .SetLargeImage(@"img\num1_32.png")
                                .SetHelpUrl("https://github.com/ReactiveBIM/RxBim"))
                        .AddCommandButton(
                            "Command2_Pulldown1",
                            typeof(Cmd2),
                            button => button
                                .SetToolTip("Tooltip: I'm run command #2. Push me!")
                                .SetText("Command\n#2")
                                .SetDescription("Description: This is command #2")
                                .SetLargeImage(@"img\num2_32.png")
                                .SetHelpUrl("https://www.google.com/"))
                        .AddCommandButton(
                            "Command3_Pulldown1",
                            typeof(Cmd3),
                            button => button
                                .SetToolTip("Tooltip: I'm run command #3. Push me!")
                                .SetText("Command\n#3")
                                .SetDescription("Description: This is command #3")
                                .SetLargeImage(@"img\num3_32.png")
                                .SetHelpUrl("https://www.autodesk.com/"))
                        .SetLargeImage(@"img\command_32.png")
                        .SetText("Pulldown\n#1"))
                .AddSlideOut()
                .AddCommandButton(
                    "Command1_SlideOut",
                    typeof(Cmd1),
                    button => button
                        .SetToolTip("Tooltip: I'm run command #1. Push me!")
                        .SetText("Command\n#1")
                        .SetDescription("Description: This is command #1")
                        .SetLargeImage(@"img\num1_32.png"))
                .AddCommandButton(
                    "Command2_SlideOut",
                    typeof(Cmd2),
                    button => button
                        .SetToolTip("Tooltip: I'm run command #2. Push me!")
                        .SetText("Command\n#2")
                        .SetDescription("Description: This is command #2")
                        .SetLargeImage(@"img\num2_32.png"))
                .AddCommandButton(
                    "Command3_SlideOut",
                    typeof(Cmd3),
                    button => button
                        .SetToolTip("Tooltip: I'm run command #3. Push me!")
                        .SetText("Command\n#3")
                        .SetDescription("Description: This is command #3")
                        .SetLargeImage(@"img\num3_32.png"))
                .ReturnToTab()
                .AddPanel("RxBim_Panel_2")
                .AddStackedItems(items => items
                    .AddCommandButton(
                        "Command1_Stacked1",
                        typeof(Cmd1),
                        button => button
                            .SetToolTip("Tooltip: I'm run command #1. Push me!")
                            .SetText("Command #1")
                            .SetDescription("Description: This is command #1")
                            .SetSmallImage(@"img\num1_16.png")
                            .SetHelpUrl("https://github.com/ReactiveBIM/RxBim"))
                    .AddCommandButton(
                        "Command2_Stacked1",
                        typeof(Cmd2),
                        button => button
                            .SetToolTip("Tooltip: I'm run command #2. Push me!")
                            .SetText("Command #2")
                            .SetDescription("Description: This is command #2")
                            .SetSmallImage(@"img\num2_16.png")
                            .SetHelpUrl("https://www.google.com/"))
                    .AddCommandButton(
                        "Command3_Stacked1",
                        typeof(Cmd3),
                        button => button
                            .SetToolTip("Tooltip: I'm run command #3. Push me!")
                            .SetText("Command #3")
                            .SetDescription("Description: This is command #3")
                            .SetSmallImage(@"img\num3_16.png")
                            .SetHelpUrl("https://www.autodesk.com/")))
                .AddSeparator()
                .AddStackedItems(items => items
                    .AddPullDownButton(
                        "Pulldown2",
                        pulldown => pulldown
                            .AddCommandButton(
                                "Command1_Pulldown2",
                                typeof(Cmd1),
                                button => button
                                    .SetToolTip("Tooltip: I'm run command #1. Push me!")
                                    .SetText("Command\n#1")
                                    .SetDescription("Description: This is command #1")
                                    .SetLargeImage(@"img\num1_32.png")
                                    .SetHelpUrl("https://github.com/ReactiveBIM/RxBim"))
                            .AddCommandButton(
                                "Command2_Pulldown2",
                                typeof(Cmd2),
                                button => button
                                    .SetToolTip("Tooltip: I'm run command #2. Push me!")
                                    .SetText("Command\n#2")
                                    .SetDescription("Description: This is command #2")
                                    .SetLargeImage(@"img\num2_32.png")
                                    .SetHelpUrl("https://www.google.com/"))
                            .AddCommandButton(
                                "Command3_Pulldown2",
                                typeof(Cmd3),
                                button => button
                                    .SetToolTip("Tooltip: I'm run command #3. Push me!")
                                    .SetText("Command\n#3")
                                    .SetDescription("Description: This is command #3")
                                    .SetLargeImage(@"img\num3_32.png")
                                    .SetHelpUrl("https://www.autodesk.com/"))
                            .SetSmallImage(@"img\command_16.png")
                            .SetText("Pulldown #2"))
                    .AddCommandButton(
                        "Command1_Stacked2",
                        typeof(Cmd1),
                        button => button
                            .SetToolTip("Tooltip: I'm run command #1. Push me!")
                            .SetText("Command #1")
                            .SetDescription("Description: This is command #1")
                            .SetSmallImage(@"img\num1_16.png")
                            .SetHelpUrl("https://github.com/ReactiveBIM/RxBim"))
                    .AddCommandButton(
                        "Command2_Stacked2",
                        typeof(Cmd2),
                        button => button
                            .SetToolTip("Tooltip: I'm run command #2. Push me!")
                            .SetText("Command #2")
                            .SetDescription("Description: This is command #2")
                            .SetSmallImage(@"img\num2_16.png")
                            .SetHelpUrl("https://www.google.com/"))));
        }
    }
}