namespace RxBim.Sample.Application.Menu.Fluent.Autocad
{
    using Commands;
    using Di;
    using Microsoft.Extensions.DependencyInjection;
    using RxBim.Application.Ribbon;

    /// <inheritdoc />
    public class Config : IApplicationConfiguration
    {
        /// <inheritdoc />
        public void Configure(IServiceCollection services)
        {
            services.AddAutocadMenu(ribbon =>
                ribbon
                    .EnableDisplayVersion()
                    .SetVersionPrefix("Version: ")
                    .Tab(
                        title: "RxBim_Tab_FromAction",
                        tab => tab
                            .Panel(
                                title: "RxBim_Panel_1",
                                panel => panel
                                    .ComboBox(
                                        "ComboBox1",
                                        cb => cb
                                            .AddComboBoxMember("ComboBoxMember1", cbm => cbm.Text("First row"))
                                            .AddComboBoxMember("ComboBoxMember2", cbm => cbm.Text("Second row")))
                                    .CommandButton(
                                        "Command1_Large_WithText",
                                        typeof(Cmd1),
                                        button => button
                                            .ToolTip("Tooltip: I'm run command #1. Push me!")
                                            .Description("Description: This is command #1")
                                            .Image(@"img.num1_16.png", ThemeType.Dark)
                                            .Image(@"img.num1_16_light.png", ThemeType.Light)
                                            .LargeImage(@"img.num1_32.png", ThemeType.Dark)
                                            .LargeImage(@"img.num1_32_light.png", ThemeType.Light)
                                            .HelpUrl("https://github.com/ReactiveBIM/RxBim")
                                            .Text("Command\n#1"))
                                    .CommandButton(
                                        "Command2_Large_WithText",
                                        typeof(Cmd2),
                                        button => button
                                            .ToolTip("Tooltip: I'm run command #2. Push me!")
                                            .Description("Description: This is command #2")
                                            .Image(@"img.num2_16.bmp", ThemeType.Dark)
                                            .Image(@"img.num2_16_light.bmp", ThemeType.Light)
                                            .LargeImage(@"img.num2_32.bmp", ThemeType.Dark)
                                            .LargeImage(@"img.num2_32_light.bmp", ThemeType.Light)
                                            .HelpUrl("https://www.google.com/")
                                            .Text("Command\n#2"))
                                    .CommandButton(
                                        "Command3_Large_WithText",
                                        typeof(Cmd3),
                                        button => button
                                            .ToolTip("Tooltip: I'm run command #3. Push me!")
                                            .Description("Description: This is command #3")
                                            .Image(@"img.num3_16.jpg", ThemeType.Dark)
                                            .Image(@"img.num3_16_light.jpg", ThemeType.Light)
                                            .LargeImage(@"img.num3_32.jpg", ThemeType.Dark)
                                            .LargeImage(@"img.num3_32_light.jpg", ThemeType.Light)
                                            .HelpUrl("https://www.autodesk.com/")
                                            .Text("Command\n#3"))
                                    .Separator()
                                    .PullDownButton(
                                        "Pulldown1",
                                        pulldown => pulldown
                                            .CommandButton(
                                                "Command #1",
                                                typeof(Cmd1),
                                                button => SetupCommand1Button(button).Text("Command\n#1"))
                                            .CommandButton(
                                                "Command #2",
                                                typeof(Cmd2),
                                                button => SetupCommand2Button(button).Text("Command\n#2"))
                                            .CommandButton(
                                                "Command #3",
                                                typeof(Cmd3),
                                                button => SetupCommand3Button(button).Text("Command\n#3"))
                                            .LargeImage(@"img.command_32.ico", ThemeType.Dark)
                                            .LargeImage(@"img.command_32_light.ico", ThemeType.Light)
                                            .Text("Pulldown #1"))
                                    .SlideOut()
                                    .CommandButton(
                                        "Command1_Large_SlideOut",
                                        typeof(Cmd1),
                                        button => SetupCommand1Button(button).Text("Command\n#1"))
                                    .CommandButton(
                                        "Command2_Large_SlideOut",
                                        typeof(Cmd2),
                                        button => SetupCommand2Button(button).Text("Command\n#2"))
                                    .CommandButton(
                                        "Command3_Large_SlideOut",
                                        typeof(Cmd3),
                                        button => SetupCommand3Button(button).Text("Command\n#3")))
                            .Panel("RxBim_Panel_2",
                                panel => panel
                                    .ComboBox(
                                        "ComboBox1",
                                        cb => cb
                                            .AddComboBoxMember("ComboBoxMember3", cbm => cbm.Text("Third row"))
                                            .AddComboBoxMember("ComboBoxMember4", cbm => cbm.Text("Forth row")))
                                    .StackedItems(items => items
                                        .CommandButton(
                                            "Command1_Small_WithText",
                                            typeof(Cmd1),
                                            button => SetupCommand1Button(button).Text("Command #1"))
                                        .ComboBox(
                                            "ComboBox2",
                                            cb => cb
                                                .AddComboBoxMember("ComboBoxMember1", cbm => cbm.Text("First row"))
                                                .AddComboBoxMember("ComboBoxMember2", cbm => cbm.Text("Second row")))
                                        .CommandButton(
                                            "Command3_Small_WithText",
                                            typeof(Cmd3),
                                            button => SetupCommand3Button(button).Text("Command #3")))
                                    .Separator()
                                    .StackedItems(items => items
                                        .CommandButton(
                                            "Command1_Large_WithText",
                                            typeof(Cmd1),
                                            button => SetupCommand1Button(button).Text("Command\n#1"))
                                        .CommandButton(
                                            "Command2_Large_WithText",
                                            typeof(Cmd2),
                                            button => SetupCommand2Button(button).Text("Command\n#2")))
                                    .Separator()
                                    .StackedItems(items => items
                                        .PullDownButton(
                                            "Pulldown2",
                                            pulldown => pulldown
                                                .Image(@"img.command_16.ico", ThemeType.Dark)
                                                .Image(@"img.command_16_light.ico", ThemeType.Light)
                                                .CommandButton(
                                                    "Command #1",
                                                    typeof(Cmd1),
                                                    button => SetupCommand1Button(button))
                                                .CommandButton(
                                                    "Command #2",
                                                    typeof(Cmd2),
                                                    button => SetupCommand2Button(button))
                                                .CommandButton(
                                                    "Command #3",
                                                    typeof(Cmd3),
                                                    button => SetupCommand3Button(button)))
                                        .CommandButton(
                                            "Command1_Small",
                                            typeof(Cmd1),
                                            button => SetupCommand1Button(button))
                                        .CommandButton(
                                            "Command2_Small",
                                            typeof(Cmd2),
                                            button => SetupCommand2Button(button)))))
                .Tab(
                        title: "RxBim_Tab_FromAttributes",
                        tab => tab
                            .Panel(
                                title: "RxBim_Panel_1",
                                panel => panel
                                    .CommandButton<Cmd1>()
                                    .CommandButton<Cmd2>()
                                    .CommandButton<Cmd3>()
                                    .Separator()
                                    .PullDownButton(
                                        "Pulldown1",
                                        pulldown => pulldown
                                            .LargeImage(@"img.command_32.ico", ThemeType.Dark)
                                            .LargeImage(@"img.command_32_light.ico", ThemeType.Light)
                                            .Text("Pulldown #1")
                                            .CommandButton<Cmd1>()
                                            .CommandButton<Cmd2>()
                                            .CommandButton<Cmd3>())
                                    .SlideOut()
                                    .CommandButton<Cmd1>()
                                    .CommandButton<Cmd2>()
                                    .CommandButton<Cmd3>())
                            .Panel("RxBim_Panel_2",
                                panel => panel
                                    .StackedItems(items => items
                                        .CommandButton<Cmd1>()
                                        .CommandButton<Cmd2>()
                                        .CommandButton<Cmd3>())
                                    .Separator()
                                    .StackedItems(items => items
                                        .CommandButton<Cmd1>()
                                        .CommandButton<Cmd2>())
                                    .Separator()
                                    .StackedItems(items => items
                                        .PullDownButton(
                                            "Pulldown2",
                                            pulldown => pulldown
                                                .Image(@"img.command_16.ico", ThemeType.Dark)
                                                .Image(@"img.command_16_light.ico", ThemeType.Light)
                                                .CommandButton<Cmd1>()
                                                .CommandButton<Cmd2>()
                                                .CommandButton<Cmd3>())
                                        .CommandButton<Cmd1>()
                                        .CommandButton<Cmd2>()))));
        }

        private static TButtonBuilder SetupCommand1Button<TButtonBuilder>(TButtonBuilder button)
            where TButtonBuilder : class, IButtonBuilder<TButtonBuilder>
        {
            return button
                .ToolTip("Tooltip: I'm run command #1. Push me!")
                .Description("Description: This is command #1")
                .Image(@"img.num1_16.png", ThemeType.Dark)
                .Image(@"img.num1_16_light.png", ThemeType.Light)
                .LargeImage(@"img.num1_32.png", ThemeType.Dark)
                .LargeImage(@"img.num1_32_light.png", ThemeType.Light)
                .HelpUrl("https://github.com/ReactiveBIM/RxBim");
        }

        private static TButtonBuilder SetupCommand2Button<TButtonBuilder>(TButtonBuilder button)
            where TButtonBuilder : class, IButtonBuilder<TButtonBuilder>
        {
            return button
                .ToolTip("Tooltip: I'm run command #2. Push me!")
                .Description("Description: This is command #2")
                .Image(@"img.num2_16.bmp", ThemeType.Dark)
                .Image(@"img.num2_16_light.bmp", ThemeType.Light)
                .LargeImage(@"img.num2_32.bmp", ThemeType.Dark)
                .LargeImage(@"img.num2_32_light.bmp", ThemeType.Light)
                .HelpUrl("https://www.google.com/");
        }

        private static TButtonBuilder SetupCommand3Button<TButtonBuilder>(TButtonBuilder button)
            where TButtonBuilder : class, IButtonBuilder<TButtonBuilder>
        {
            return button
                .ToolTip("Tooltip: I'm run command #3. Push me!")
                .Description("Description: This is command #3")
                .Image(@"img.num3_16.jpg", ThemeType.Dark)
                .Image(@"img.num3_16_light.jpg", ThemeType.Light)
                .LargeImage(@"img.num3_32.jpg", ThemeType.Dark)
                .LargeImage(@"img.num3_32_light.jpg", ThemeType.Light)
                .HelpUrl("https://www.autodesk.com/");
        }
    }
}