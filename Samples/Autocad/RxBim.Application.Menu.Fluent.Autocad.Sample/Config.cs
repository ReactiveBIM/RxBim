namespace RxBim.Application.Menu.Fluent.Autocad.Sample
{
    using System.Collections.Generic;
    using Commands;
    using Di;
    using Ribbon.Abstractions.ConfigurationBuilders;
    using Ribbon.Autocad.Extensions;
    using Ribbon.Models;
    using Shared;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddAutocadMenu(ribbon =>
                ribbon
                    .SetAddVersionToCommandTooltip(true)
                    .SetCommandTooltipVersionHeader("Version: ")
                    .AddTab("RxBim_Tab_FromAction")
                    .AddAboutButton(
                        "About",
                        new AboutBoxContent(
                            "RxBim4AutoCAD",
                            "1.0",
                            "RxBim product module for API demo and validation",
                            GetType().Assembly.GetName().Version,
                            "ReactiveBIM",
                            new Dictionary<string, string>
                            {
                                { "Download examples", "https://github.com/ReactiveBIM/RxBim.Examples" }
                            }),
                        button => button
                            .SetText("About\nbutton")
                            .SetToolTip("About information")
                            .SetDescription("Button for displaying the About window")
                            .SetLargeImage(@"img\about_32.png", ThemeType.Dark)
                            .SetLargeImage(@"img\about_32_light.png", ThemeType.Light))
                    .AddPanel("RxBim_Panel_1")
                    .AddCommandButton(
                        "Command1_Large_WithText",
                        typeof(Cmd1),
                        button => button
                            .SetToolTip("Tooltip: I'm run command #1. Push me!")
                            .SetDescription("Description: This is command #1")
                            .SetSmallImage(@"img\num1_16.png", ThemeType.Dark)
                            .SetSmallImage(@"img\num1_16_light.png", ThemeType.Light)
                            .SetLargeImage(@"img\num1_32.png", ThemeType.Dark)
                            .SetLargeImage(@"img\num1_32_light.png", ThemeType.Light)
                            .SetHelpUrl("https://github.com/ReactiveBIM/RxBim")
                            .SetText("Command\n#1"))
                    .AddCommandButton(
                        "Command2_Large_WithText",
                        typeof(Cmd2),
                        button => button
                            .SetToolTip("Tooltip: I'm run command #2. Push me!")
                            .SetDescription("Description: This is command #2")
                            .SetSmallImage(@"img\num2_16.png", ThemeType.Dark)
                            .SetSmallImage(@"img\num2_16_light.png", ThemeType.Light)
                            .SetLargeImage(@"img\num2_32.png", ThemeType.Dark)
                            .SetLargeImage(@"img\num2_32_light.png", ThemeType.Light)
                            .SetHelpUrl("https://www.google.com/")
                            .SetText("Command\n#2"))
                    .AddCommandButton(
                        "Command3_Large_WithText",
                        typeof(Cmd3),
                        button => button
                            .SetToolTip("Tooltip: I'm run command #3. Push me!")
                            .SetDescription("Description: This is command #3")
                            .SetSmallImage(@"img\num3_16.png", ThemeType.Dark)
                            .SetSmallImage(@"img\num3_16_light.png", ThemeType.Light)
                            .SetLargeImage(@"img\num3_32.png", ThemeType.Dark)
                            .SetLargeImage(@"img\num3_32_light.png", ThemeType.Light)
                            .SetHelpUrl("https://www.autodesk.com/")
                            .SetText("Command\n#3"))
                    .AddSeparator()
                    .AddPullDownButton(
                        "Pulldown1",
                        pulldown => pulldown
                            .AddCommandButton(
                                "Command #1",
                                typeof(Cmd1),
                                button => SetupCommand1Button(button).SetText("Command\n#1"))
                            .AddCommandButton(
                                "Command #2",
                                typeof(Cmd2),
                                button => SetupCommand2Button(button).SetText("Command\n#2"))
                            .AddCommandButton(
                                "Command #3",
                                typeof(Cmd3),
                                button => SetupCommand3Button(button).SetText("Command\n#3"))
                            .SetLargeImage(@"img\command_32.png", ThemeType.Dark)
                            .SetLargeImage(@"img\command_32_light.png", ThemeType.Light)
                            .SetText("Pulldown #1"))
                    .AddSlideOut()
                    .AddCommandButton(
                        "Command1_Large_SlideOut",
                        typeof(Cmd1),
                        button => SetupCommand1Button(button).SetText("Command\n#1"))
                    .AddCommandButton(
                        "Command2_Large_SlideOut",
                        typeof(Cmd2),
                        button => SetupCommand2Button(button).SetText("Command\n#2"))
                    .AddCommandButton(
                        "Command3_Large_SlideOut",
                        typeof(Cmd3),
                        button => SetupCommand3Button(button).SetText("Command\n#3"))
                    .ReturnToTab()
                    .AddPanel("RxBim_Panel_2")
                    .AddStackedItems(items => items
                        .AddCommandButton(
                            "Command1_Small_WithText",
                            typeof(Cmd1),
                            button => SetupCommand1Button(button).SetText("Command #1"))
                        .AddCommandButton(
                            "Command2_Small_WithText",
                            typeof(Cmd2),
                            button => SetupCommand2Button(button).SetText("Command #2"))
                        .AddCommandButton(
                            "Command3_Small_WithText",
                            typeof(Cmd3),
                            button => SetupCommand3Button(button).SetText("Command #3")))
                    .AddSeparator()
                    .AddStackedItems(items => items
                        .AddCommandButton(
                            "Command1_Large_WithText",
                            typeof(Cmd1),
                            button => SetupCommand1Button(button).SetText("Command #1"))
                        .AddCommandButton(
                            "Command2_Large_WithText",
                            typeof(Cmd2),
                            button => SetupCommand2Button(button).SetText("Command #2")))
                    .AddSeparator()
                    .AddStackedItems(items => items
                        .AddPullDownButton(
                            "Pulldown2",
                            pulldown => pulldown
                                .AddCommandButton(
                                    "Command #1",
                                    typeof(Cmd1),
                                    button => SetupCommand1Button(button))
                                .AddCommandButton(
                                    "Command #2",
                                    typeof(Cmd2),
                                    button => SetupCommand2Button(button))
                                .AddCommandButton(
                                    "Command #3",
                                    typeof(Cmd3),
                                    button => SetupCommand3Button(button))
                                .SetSmallImage(@"img\command_16.png", ThemeType.Dark)
                                .SetSmallImage(@"img\command_16_light.png", ThemeType.Light))
                        .AddCommandButton(
                            "Command1_Small",
                            typeof(Cmd1),
                            button => SetupCommand1Button(button))
                        .AddCommandButton(
                            "Command2_Small",
                            typeof(Cmd2),
                            button => SetupCommand2Button(button))));
        }

        private static IButtonBuilder SetupCommand1Button(IButtonBuilder button)
        {
            return button
                .SetToolTip("Tooltip: I'm run command #1. Push me!")
                .SetDescription("Description: This is command #1")
                .SetSmallImage(@"img\num1_16.png", ThemeType.Dark)
                .SetSmallImage(@"img\num1_16_light.png", ThemeType.Light)
                .SetLargeImage(@"img\num1_32.png", ThemeType.Dark)
                .SetLargeImage(@"img\num1_32_light.png", ThemeType.Light)
                .SetHelpUrl("https://github.com/ReactiveBIM/RxBim");
        }

        private static IButtonBuilder SetupCommand2Button(IButtonBuilder button)
        {
            return button
                .SetToolTip("Tooltip: I'm run command #2. Push me!")
                .SetDescription("Description: This is command #2")
                .SetSmallImage(@"img\num2_16.png", ThemeType.Dark)
                .SetSmallImage(@"img\num2_16_light.png", ThemeType.Light)
                .SetLargeImage(@"img\num2_32.png", ThemeType.Dark)
                .SetLargeImage(@"img\num2_32_light.png", ThemeType.Light)
                .SetHelpUrl("https://www.google.com/");
        }

        private static IButtonBuilder SetupCommand3Button(IButtonBuilder button)
        {
            return button
                .SetToolTip("Tooltip: I'm run command #3. Push me!")
                .SetDescription("Description: This is command #3")
                .SetSmallImage(@"img\num3_16.png", ThemeType.Dark)
                .SetSmallImage(@"img\num3_16_light.png", ThemeType.Light)
                .SetLargeImage(@"img\num3_32.png", ThemeType.Dark)
                .SetLargeImage(@"img\num3_32_light.png", ThemeType.Light)
                .SetHelpUrl("https://www.autodesk.com/");
        }
    }
}