namespace RxBim.Sample.Application.Menu.Fluent.Revit
{
    using RxBim.Application.Ribbon;

    /// <summary>
    /// Manual checks for button text visibility and attribute-based tooltips.
    /// </summary>
    internal static class TextVisibilityRibbonExtensions
    {
        /// <summary>
        /// Adds panels for comparing hidden, visible and automatic button text.
        /// </summary>
        /// <param name="tab">Tab configuration builder.</param>
        public static ITabBuilder TextVisibilityPanels(this ITabBuilder tab)
        {
            return tab
                .Panel("Hidden stacked", panel => panel
                    .StackedItems(items => items
                        .CommandButton<Cmd1>("Hidden1", button => button.ShowText(false))
                        .CommandButton<Cmd2>("Hidden2", button => button.ShowText(false))
                        .CommandButton<Cmd3>("Hidden3", button => button.ShowText(false))))
                .Panel("Visible and default", panel => panel
                    .StackedItems(items => items
                        .CommandButton<Cmd1>("Visible1", button => button.ShowText(true))
                        .CommandButton<Cmd2>("Default2")
                        .CommandButton<Cmd3>("Visible3", button => button.ShowText(true))))
                .Panel("Buttons and menus", panel => panel
                    .CommandButton<Cmd1>("LargeHidden", button => button.ShowText(false))
                    .PullDownButton("HiddenMenu", menu => menu
                        .Text("Hidden menu")
                        .ToolTip("The menu caption is hidden; its title is preserved.")
                        .Image(@"img\command_16.ico", ThemeType.Dark)
                        .LargeImage(@"img\command_32.ico", ThemeType.Dark)
                        .Image(@"img\command_16_light.ico", ThemeType.Light)
                        .LargeImage(@"img\command_32_light.ico", ThemeType.Light)
                        .ShowText(false)
                        .CommandButton<Cmd1>("MenuHidden", button => button.ShowText(false))
                        .CommandButton<Cmd2>("MenuVisible", button => button.ShowText(true))
                        .CommandButton<Cmd3>("MenuDefault")))
                .Panel("Stacked menu", panel => panel
                    .StackedItems(items => items
                        .PullDownButton("StackedHiddenMenu", menu => menu
                            .Text("Stacked hidden menu")
                            .ToolTip("The stacked menu caption is hidden; its title is preserved.")
                            .Image(@"img\command_16.ico", ThemeType.Dark)
                            .Image(@"img\command_16_light.ico", ThemeType.Light)
                            .ShowText(false)
                            .CommandButton<Cmd1>("StackedMenuDefault"))
                        .CommandButton<Cmd2>("StackedVisible", button => button.ShowText(true))
                        .CommandButton<Cmd3>("StackedHidden", button => button.ShowText(false))));
        }
    }
}