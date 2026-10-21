namespace RxBim.Sample.Application.Menu.Fluent.Revit.EnhancedToolTips;

using RxBim.Application.Ribbon;

/// <summary>
/// Adds isolated comparisons of native tooltips and interactive popups.
/// </summary>
internal static class ToolTipProbeRibbonExtensions
{
    /// <summary>
    /// The panel that exclusively owns the probe buttons.
    /// </summary>
    internal const string PanelName = "Tooltip probes";

    /// <summary>
    /// The help topic shared by native contextual help and the interactive cards.
    /// </summary>
    internal const string HelpUrl = "https://github.com/ReactiveBIM/RxBim";

    /// <summary>
    /// The Markdown content shared by the interactive popup probes.
    /// </summary>
    internal const string Markdown =
        "## Interactive tooltip\n\n" +
        "This **Markdown card** is shared by the popup probes.\n\n" +
        "1. Move the pointer from the button into this card.\n" +
        "2. Follow [Autodesk](https://www.autodesk.com) without executing the command.\n" +
        "3. Return to Revit and test the help link and **F1**.\n\n" +
        "### Closing and focus\n\n" +
        "- Move away and check the closing delay.\n" +
        "- Press Escape, click outside, or switch to another application.\n" +
        "- Repeat inside the stacked buttons and the drop-down menu.\n\n" +
        "### Scrolling\n\n" +
        "The card has a limited height. Scroll this content and then follow a link. " +
        "The pointer must remain usable inside the card without running the ribbon command.\n\n" +
        "The `Popup only` button has no configured native tooltip or description.";

    /// <summary>
    /// Adds the manual tooltip comparison panel to the sample tab.
    /// </summary>
    /// <param name="tab">The sample ribbon tab.</param>
    public static ITabBuilder ToolTipProbePanel(this ITabBuilder tab)
    {
        return tab.Panel(PanelName, panel => panel
            .CommandButton<ToolTipProbeCommand>("TipNative", button => ConfigureNative(button, "Native"))
            .CommandButton<ToolTipProbeCommand>("TipPopup", button => ConfigurePopup(button, "Popup\nwith native"))
            .CommandButton<ToolTipProbeCommand>("TipPopupOnly", button => ConfigurePopupOnly(button, "Popup\nonly"))
            .CommandButton<Cmd3>("TipAttribute", button => button.Text("From\nattribute"))
            .StackedItems(items => items
                .CommandButton<ToolTipProbeCommand>("TipStackNative", button => ConfigureNative(button, "Native"))
                .CommandButton<ToolTipProbeCommand>("TipStackPopup", button => ConfigurePopup(button, "Popup")))
            .PullDownButton("TipMenu", menu => menu
                .Text("Tooltip\nmenu")
                .ToolTip("Compare native and interactive tooltips inside this menu.")
                .EnhancedToolTip("Open this menu to compare **native** and **interactive** tooltips.")
                .Image(@"img\command_16.ico", ThemeType.Dark)
                .Image(@"img\command_16_light.ico", ThemeType.Light)
                .LargeImage(@"img\command_32.ico", ThemeType.Dark)
                .LargeImage(@"img\command_32_light.ico", ThemeType.Light)
                .CommandButton<ToolTipProbeCommand>("TipMenuNative", button => ConfigureNative(button, "Native"))
                .CommandButton<ToolTipProbeCommand>("TipMenuPopup", button => ConfigurePopup(button, "Popup"))));
    }

    private static void ConfigureButton(ICommandButtonBuilder button, string text)
    {
        button.Text(text)
            .Image(@"img\command_16.ico", ThemeType.Dark)
            .Image(@"img\command_16_light.ico", ThemeType.Light)
            .LargeImage(@"img\command_32.ico", ThemeType.Dark)
            .LargeImage(@"img\command_32_light.ico", ThemeType.Light)
            .HelpUrl(HelpUrl);
    }

    private static void ConfigureNative(ICommandButtonBuilder button, string text)
    {
        ConfigureButton(button, text);
        button.ToolTip("Native Revit summary: " + text.Replace('\n', ' '))
            .Description("Native Revit long description. Press F1 to open the RxBim project help.");
    }

    private static void ConfigurePopup(ICommandButtonBuilder button, string text)
    {
        ConfigureNative(button, text);
        button.EnhancedToolTip(Markdown);
    }

    private static void ConfigurePopupOnly(ICommandButtonBuilder button, string text)
    {
        ConfigureButton(button, text);
        button.EnhancedToolTip(Markdown);
    }
}