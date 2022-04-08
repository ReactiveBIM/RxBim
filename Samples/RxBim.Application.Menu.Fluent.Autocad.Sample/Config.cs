namespace RxBim.Application.Menu.Fluent.Autocad.Sample
{
    using Commands;
    using Di;
    using Ribbon.Abstractions.ConfigurationBuilders;
    using Ribbon.Autocad.Extensions;
    using Ribbon.Models;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IContainer container)
        {
            container.AddAutocadMenu(ribbon =>
                ribbon
                    .EnableAddVersionToCommandTooltip()
                    .SetCommandTooltipVersionHeader("Version: ")
                    .AddTab("RxBim_Tab_FromAction")
                    .AddPanel("RxBim_Panel_1")
                    .AddCommandButton<Cmd1>(
                        "Command1_Large_WithText",
                        button => button
                            .SetToolTip("Tooltip: I'm run command #1. Push me!")
                            .SetDescription("Description: This is command #1")
                            .SetSmallImage(@"img\num1_16.png", ThemeType.Dark)
                            .SetSmallImage(@"img\num1_16_light.png", ThemeType.Light)
                            .SetLargeImage(@"img\num1_32.png", ThemeType.Dark)
                            .SetLargeImage(@"img\num1_32_light.png", ThemeType.Light)
                            .SetHelpUrl("https://github.com/ReactiveBIM/RxBim")
                            .SetText("Command\n#1"))
                    .AddCommandButton<Cmd2>(
                        "Command2_Large_WithText",
                        button => button
                            .SetToolTip("Tooltip: I'm run command #2. Push me!")
                            .SetDescription("Description: This is command #2")
                            .SetSmallImage(@"img\num2_16.png", ThemeType.Dark)
                            .SetSmallImage(@"img\num2_16_light.png", ThemeType.Light)
                            .SetLargeImage(@"img\num2_32.png", ThemeType.Dark)
                            .SetLargeImage(@"img\num2_32_light.png", ThemeType.Light)
                            .SetHelpUrl("https://www.google.com/")
                            .SetText("Command\n#2"))
                    .AddCommandButton<Cmd3>(
                        "Command3_Large_WithText",
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
                            .AddCommandButton<Cmd1>(
                                "Command #1",
                                button => SetupCommand1Button(button).SetText("Command\n#1"))
                            .AddCommandButton<Cmd2>(
                                "Command #2",
                                button => SetupCommand2Button(button).SetText("Command\n#2"))
                            .AddCommandButton<Cmd3>(
                                "Command #3",
                                button => SetupCommand3Button(button).SetText("Command\n#3"))
                            .SetLargeImage(@"img\command_32.png", ThemeType.Dark)
                            .SetLargeImage(@"img\command_32_light.png", ThemeType.Light)
                            .SetText("Pulldown #1"))
                    .AddSlideOut()
                    .AddCommandButton<Cmd1>(
                        "Command1_Large_SlideOut",
                        button => SetupCommand1Button(button).SetText("Command\n#1"))
                    .AddCommandButton<Cmd2>(
                        "Command2_Large_SlideOut",
                        button => SetupCommand2Button(button).SetText("Command\n#2"))
                    .AddCommandButton<Cmd3>(
                        "Command3_Large_SlideOut",
                        button => SetupCommand3Button(button).SetText("Command\n#3"))
                    .ReturnToTab()
                    .AddPanel("RxBim_Panel_2")
                    .AddStackedItems(items => items
                        .AddCommandButton<Cmd1>(
                            "Command1_Small_WithText",
                            button => SetupCommand1Button(button).SetText("Command #1"))
                        .AddCommandButton<Cmd2>(
                            "Command2_Small_WithText",
                            button => SetupCommand2Button(button).SetText("Command #2"))
                        .AddCommandButton<Cmd3>(
                            "Command3_Small_WithText",
                            button => SetupCommand3Button(button).SetText("Command #3")))
                    .AddSeparator()
                    .AddStackedItems(items => items
                        .AddCommandButton<Cmd1>(
                            "Command1_Large_WithText",
                            button => SetupCommand1Button(button).SetText("Command #1"))
                        .AddCommandButton<Cmd2>(
                            "Command2_Large_WithText",
                            button => SetupCommand2Button(button).SetText("Command #2")))
                    .AddSeparator()
                    .AddStackedItems(items => items
                        .AddPullDownButton(
                            "Pulldown2",
                            pulldown => pulldown
                                .AddCommandButton<Cmd1>("Command #1", button => SetupCommand1Button(button))
                                .AddCommandButton<Cmd2>("Command #2", button => SetupCommand2Button(button))
                                .AddCommandButton<Cmd3>("Command #3", button => SetupCommand3Button(button))
                                .SetSmallImage(@"img\command_16.png", ThemeType.Dark)
                                .SetSmallImage(@"img\command_16_light.png", ThemeType.Light))
                        .AddCommandButton<Cmd1>("Command1_Small", button => SetupCommand1Button(button))
                        .AddCommandButton<Cmd2>("Command2_Small", button => SetupCommand2Button(button))));
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