namespace RxBim.Application.Menu.Fluent.Revit.Sample
{
    using System.Collections.Generic;
    using System.Reflection;
    using Ribbon;

    /// <summary>
    /// Sample ribbon configurations.
    /// </summary>
    internal static class InternalRibbonExtensions
    {
        /// <summary>
        /// Bilds a new Tab using attributes.
        /// </summary>
        /// <param name="ribbon">A ribbon.</param>
        public static IRibbonBuilder TabFromAttributes(this IRibbonBuilder ribbon)
        {
            return ribbon
                .Tab("RxBim_Tab_FromAttr", tab => tab
                    .AboutButton(
                        name: "About",
                        new AboutBoxContent(
                            title: "RxBim4Revit",
                            "1.0",
                            "RxBim product module for API demo and validation",
                            Assembly.GetExecutingAssembly().GetName().Version,
                            "ReactiveBIM",
                            new Dictionary<string, string>
                            {
                                { "Examples", "https://github.com/ReactiveBIM/RxBim.Examples" }
                            }),
                        button => button
                            .Text("About\nbutton")
                            .ToolTip("About information")
                            .Description("Button for displaying the About window")
                            .LargeImage(@"img\about_32.png"))
                    .Panel(
                        title: "RxBim_Panel_1",
                        panel => panel
                            .CommandButton<Cmd1>("Command1_Large_WithText")
                            .CommandButton<Cmd2>("Command2_Large_WithText")
                            .CommandButton<Cmd3>("Command3_Large_WithText")
                            .Separator()
                            .PullDownButton(
                                "Pulldown1",
                                pulldown => pulldown
                                    .CommandButton<Cmd1>("Command1_Pulldown1")
                                    .CommandButton<Cmd2>("Command2_Pulldown1")
                                    .CommandButton<Cmd3>("Command3_Pulldown1")
                                    .LargeImage(@"img\command_32.png")
                                    .Text("Pulldown\n#1"))
                            .SlideOut()
                            .CommandButton<Cmd1>("Command1_SlideOut")
                            .CommandButton<Cmd2>("Command2_SlideOut")
                            .CommandButton<Cmd3>("Command3_SlideOut"))
                    .Panel(
                        "RxBim_Panel_2",
                        panel => panel
                            .StackedItems(items => items
                                .CommandButton<Cmd1>("Command1_Stacked1")
                                .CommandButton<Cmd2>("Command2_Stacked1")
                                .CommandButton<Cmd3>("Command3_Stacked1"))
                            .Separator()
                            .StackedItems(items => items
                                .PullDownButton(
                                    "Pulldown2",
                                    pulldown => pulldown
                                        .CommandButton<Cmd1>("Command1_Pulldown2")
                                        .CommandButton<Cmd2>("Command2_Pulldown2")
                                        .CommandButton<Cmd3>("Command3_Pulldown2")
                                        .SmallImage(@"img\command_16.png")
                                        .Text("Pulldown #2"))
                                .CommandButton<Cmd1>("Command1_Stacked2")
                                .CommandButton<Cmd2>("Command2_Stacked2"))));
        }

        /// <summary>
        /// Builds a ribbon tab using fluent builder.
        /// </summary>
        /// <param name="ribbon">A ribbon.</param>
        /// <returns></returns>
        public static IRibbonBuilder TabFromBuilder(this IRibbonBuilder ribbon)
        {
            return ribbon
                .Tab("RxBim_Tab_FromAction", tab => tab
                    .AboutButton(
                        name: "About",
                        new AboutBoxContent(
                            title: "RxBim4Revit",
                            "1.0",
                            "RxBim product module for API demo and validation",
                            Assembly.GetExecutingAssembly().GetName().Version,
                            "ReactiveBIM",
                            new Dictionary<string, string>
                            {
                                { "Examples", "https://github.com/ReactiveBIM/RxBim.Examples" }
                            }),
                        button => button
                            .Text("About\nbutton")
                            .ToolTip("About information")
                            .Description("Button for displaying the About window")
                            .LargeImage(@"img\about_32.png"))
                    .Panel(
                        title: "RxBim_Panel_1",
                        panel => panel
                            .CommandButton(
                                name: "Command1_Large_WithText",
                                typeof(Cmd1),
                                button => button
                                    .ToolTip("Tooltip: I'm run command #1. Push me!")
                                    .Text("Command\n#1")
                                    .Description("Description: This is command #1")
                                    .LargeImage(@"img\num1_32.png")
                                    .HelpUrl("https://github.com/ReactiveBIM/RxBim"))
                            .CommandButton(
                                "Command2_Large_WithText",
                                typeof(Cmd2),
                                button => button
                                    .ToolTip("Tooltip: I'm run command #2. Push me!")
                                    .Text("Command\n#2")
                                    .Description("Description: This is command #2")
                                    .SmallImage(@"img\num2_16.png")
                                    .LargeImage(@"img\num2_32.png")
                                    .HelpUrl("https://www.google.com/"))
                            .CommandButton(
                                "Command3_Large_WithText",
                                typeof(Cmd3),
                                button => button
                                    .ToolTip("Tooltip: I'm run command #3. Push me!")
                                    .Text("Command\n#3")
                                    .Description("Description: This is command #3")
                                    .SmallImage(@"img\num3_16.png")
                                    .LargeImage(@"img\num3_32.png")
                                    .HelpUrl("https://www.autodesk.com/"))
                            .Separator()
                            .PullDownButton(
                                "Pulldown1",
                                pulldown => pulldown
                                    .CommandButton(
                                        "Command1_Pulldown1",
                                        typeof(Cmd1),
                                        button => button
                                            .ToolTip("Tooltip: I'm run command #1. Push me!")
                                            .Text("Command\n#1")
                                            .Description("Description: This is command #1")
                                            .LargeImage(@"img\num1_32.png")
                                            .HelpUrl("https://github.com/ReactiveBIM/RxBim"))
                                    .CommandButton(
                                        "Command2_Pulldown1",
                                        typeof(Cmd2),
                                        button => button
                                            .ToolTip("Tooltip: I'm run command #2. Push me!")
                                            .Text("Command\n#2")
                                            .Description("Description: This is command #2")
                                            .LargeImage(@"img\num2_32.png")
                                            .HelpUrl("https://www.google.com/"))
                                    .CommandButton(
                                        "Command3_Pulldown1",
                                        typeof(Cmd3),
                                        button => button
                                            .ToolTip("Tooltip: I'm run command #3. Push me!")
                                            .Text("Command\n#3")
                                            .Description("Description: This is command #3")
                                            .LargeImage(@"img\num3_32.png")
                                            .HelpUrl("https://www.autodesk.com/"))
                                    .LargeImage(@"img\command_32.png")
                                    .Text("Pulldown\n#1"))
                            .SlideOut()
                            .CommandButton(
                                "Command1_SlideOut",
                                typeof(Cmd1),
                                button => button
                                    .ToolTip("Tooltip: I'm run command #1. Push me!")
                                    .Text("Command\n#1")
                                    .Description("Description: This is command #1")
                                    .LargeImage(@"img\num1_32.png"))
                            .CommandButton(
                                "Command2_SlideOut",
                                typeof(Cmd2),
                                button => button
                                    .ToolTip("Tooltip: I'm run command #2. Push me!")
                                    .Text("Command\n#2")
                                    .Description("Description: This is command #2")
                                    .LargeImage(@"img\num2_32.png"))
                            .CommandButton(
                                "Command3_SlideOut",
                                typeof(Cmd3),
                                button => button
                                    .ToolTip("Tooltip: I'm run command #3. Push me!")
                                    .Text("Command\n#3")
                                    .Description("Description: This is command #3")
                                    .LargeImage(@"img\num3_32.png")))
                    .Panel(
                        "RxBim_Panel_2",
                        panel => panel
                            .StackedItems(items => items
                                .CommandButton(
                                    "Command1_Stacked1",
                                    typeof(Cmd1),
                                    button => button
                                        .ToolTip("Tooltip: I'm run command #1. Push me!")
                                        .Text("Command #1")
                                        .Description("Description: This is command #1")
                                        .SmallImage(@"img\num1_16.png")
                                        .HelpUrl("https://github.com/ReactiveBIM/RxBim"))
                                .CommandButton(
                                    "Command2_Stacked1",
                                    typeof(Cmd2),
                                    button => button
                                        .ToolTip("Tooltip: I'm run command #2. Push me!")
                                        .Text("Command #2")
                                        .Description("Description: This is command #2")
                                        .SmallImage(@"img\num2_16.png")
                                        .HelpUrl("https://www.google.com/"))
                                .CommandButton(
                                    "Command3_Stacked1",
                                    typeof(Cmd3),
                                    button => button
                                        .ToolTip("Tooltip: I'm run command #3. Push me!")
                                        .Text("Command #3")
                                        .Description("Description: This is command #3")
                                        .SmallImage(@"img\num3_16.png")
                                        .HelpUrl("https://www.autodesk.com/")))
                            .Separator()
                            .StackedItems(items => items
                                .PullDownButton(
                                    "Pulldown2",
                                    pulldown => pulldown
                                        .CommandButton(
                                            "Command1_Pulldown2",
                                            typeof(Cmd1),
                                            button => button
                                                .ToolTip("Tooltip: I'm run command #1. Push me!")
                                                .Text("Command\n#1")
                                                .Description("Description: This is command #1")
                                                .LargeImage(@"img\num1_32.png")
                                                .HelpUrl("https://github.com/ReactiveBIM/RxBim"))
                                        .CommandButton(
                                            "Command2_Pulldown2",
                                            typeof(Cmd2),
                                            button => button
                                                .ToolTip("Tooltip: I'm run command #2. Push me!")
                                                .Text("Command\n#2")
                                                .Description("Description: This is command #2")
                                                .LargeImage(@"img\num2_32.png")
                                                .HelpUrl("https://www.google.com/"))
                                        .CommandButton(
                                            "Command3_Pulldown2",
                                            typeof(Cmd3),
                                            button => button
                                                .ToolTip("Tooltip: I'm run command #3. Push me!")
                                                .Text("Command\n#3")
                                                .Description("Description: This is command #3")
                                                .LargeImage(@"img\num3_32.png")
                                                .HelpUrl("https://www.autodesk.com/"))
                                        .SmallImage(@"img\command_16.png")
                                        .Text("Pulldown #2"))
                                .CommandButton(
                                    "Command1_Stacked2",
                                    typeof(Cmd1),
                                    button => button
                                        .ToolTip("Tooltip: I'm run command #1. Push me!")
                                        .Text("Command #1")
                                        .Description("Description: This is command #1")
                                        .SmallImage(@"img\num1_16.png")
                                        .HelpUrl("https://github.com/ReactiveBIM/RxBim"))
                                .CommandButton(
                                    "Command2_Stacked2",
                                    typeof(Cmd2),
                                    button => button
                                        .ToolTip("Tooltip: I'm run command #2. Push me!")
                                        .Text("Command #2")
                                        .Description("Description: This is command #2")
                                        .SmallImage(@"img\num2_16.png")
                                        .HelpUrl("https://www.google.com/")))));
        }
    }
}