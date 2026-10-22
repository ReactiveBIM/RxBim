namespace RxBim.Sample.Application.Menu.Fluent.Autocad;

using Commands;
using RxBim.Application.Ribbon;
using ToolTipExamples;

/// <summary>
/// Demonstrates optional Markdown tooltips without replacing native tooltips on other buttons.
/// </summary>
internal static class EnhancedToolTipRibbonExtensions
{
    /// <summary>
    /// The Markdown content shared by fluent and attribute-based examples.
    /// </summary>
    internal const string Markdown =
        "## Interactive tooltip\n\n" +
        "Move the pointer into this **Markdown card**, scroll it and follow " +
        "[Autodesk](https://www.autodesk.com).\n\n" +
        "- Test the Help link and F1.\n" +
        "- Test the same card inside the drop-down menu.\n" +
        "- Escape and a click outside must close the card.\n\n" +
        "The Popup only button has no configured native tooltip text.";

    /// <summary>
    /// Adds the native and enhanced tooltip examples.
    /// </summary>
    /// <param name="tab">The sample tab.</param>
    public static ITabBuilder EnhancedToolTipPanel(this ITabBuilder tab) => tab
        .Panel("Enhanced tooltips", panel => panel
            .CommandButton<EnhancedToolTipCommand>("TipNative", button => ConfigureNative(button, "Native"))
            .CommandButton<EnhancedToolTipCommand>("TipPopup", button => ConfigurePopup(button, "Popup"))
            .CommandButton<EnhancedToolTipCommand>(
                "TipPopupOnly", button => ConfigurePopupOnly(button, "Popup only"))
            .CommandButton<EnhancedToolTipCommand>(
                "TipRenderer", button => ConfigureRenderer(button, "Картинка\nи таблица"))
            .CommandButton<Cmd3>("TipAttribute", button => button.Text("From\nattribute"))
            .ToggleCommandButton<EnhancedToolTipCommand>("TipToggle", button => button
                .Text("Toggle")
                .Image(@"img\num1_16.png", ThemeType.Dark)
                .Image(@"img\num1_16_light.png", ThemeType.Light)
                .LargeImage(@"img\num1_32.png", ThemeType.Dark)
                .LargeImage(@"img\num1_32_light.png", ThemeType.Light)
                .EnhancedToolTip(Markdown)
                .HelpUrl("https://github.com/ReactiveBIM/RxBim"))
            .StackedItems(items => items
                .CommandButton<EnhancedToolTipCommand>(
                    "TipStackNative", button => ConfigureNative(button, "Native"))
                .CommandButton<EnhancedToolTipCommand>("TipStackPopup", button => ConfigurePopup(button, "Popup")))
            .PullDownButton("TipMenu", menu => menu
                .Text("Tooltip\nmenu")
                .ToolTip("Compare native and enhanced tooltips inside this menu.")
                .EnhancedToolTip("Open this menu to compare **native** and **interactive** tooltips.")
                .Image(@"img\num1_16.png", ThemeType.Dark)
                .Image(@"img\num1_16_light.png", ThemeType.Light)
                .LargeImage(@"img\num1_32.png", ThemeType.Dark)
                .LargeImage(@"img\num1_32_light.png", ThemeType.Light)
                .CommandButton<EnhancedToolTipCommand>("TipMenuNative", button => ConfigureNative(button, "Native"))
                .CommandButton<EnhancedToolTipCommand>("TipMenuPopup", button => ConfigurePopup(button, "Popup"))
                .CommandButton<EnhancedToolTipCommand>(
                    "TipMenuRenderer", button => ConfigureRenderer(button, "Картинка и таблица"))));

    private static void ConfigureButton(ICommandButtonBuilder button, string text) => button
        .Text(text)
        .Image(@"img\num1_16.png", ThemeType.Dark)
        .Image(@"img\num1_16_light.png", ThemeType.Light)
        .LargeImage(@"img\num1_32.png", ThemeType.Dark)
        .LargeImage(@"img\num1_32_light.png", ThemeType.Light)
        .HelpUrl("https://github.com/ReactiveBIM/RxBim");

    private static void ConfigureNative(ICommandButtonBuilder button, string text)
    {
        ConfigureButton(button, text);
        button.ToolTip("Native summary: " + text).Description("Native expanded description.");
    }

    private static void ConfigurePopup(ICommandButtonBuilder button, string text)
    {
        ConfigureNative(button, text);
        button.EnhancedToolTip(Markdown);
    }

    private static void ConfigureRenderer(ICommandButtonBuilder button, string text)
    {
        ConfigureButton(button, text);
        button.EnhancedToolTip(EnhancedToolTipSampleContent.Markdown);
    }

    private static void ConfigurePopupOnly(ICommandButtonBuilder button, string text)
    {
        ConfigureButton(button, text);
        button.EnhancedToolTip(Markdown);
    }
}