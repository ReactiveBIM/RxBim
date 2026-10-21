# Enhanced ribbon tooltips

Revit and AutoCAD menus support an optional interactive Markdown tooltip. No additional service registration is required beyond `AddRevitMenu` or `AddAutocadMenu`.

```csharp
.CommandButton<MyCommand>(button => button
    .Text("My command")
    .EnhancedToolTip("**Details**\n\n[Video](https://example.com/video)")
    .HelpUrl("https://example.com/help"))
```

`EnhancedToolTip` is optional. Null, empty or whitespace content leaves native tooltip behavior unchanged. `ToolTip` and `Description` may be omitted when only the enhanced tooltip is needed. When provided, they remain the native fallback. Ordinary text is valid Markdown; there is no format argument or special prefix.

Command attributes support the same content:

```csharp
[RxBimCommand(
    ToolTip = "Optional native summary",
    EnhancedToolTip = "**Details**\n\n[Video](https://example.com/video)",
    HelpUrl = "https://example.com/help")]
```

AutoCAD's `RxBimCommandClass` inherits these properties as well. Explicit fluent/configuration values take precedence over command attributes. An explicitly empty enhanced tooltip disables an attribute-provided enhanced tooltip.

Configuration uses the same button property:

```json
{
  "Name": "MyCommand",
  "CommandType": "MyPlugin.MyCommand",
  "EnhancedToolTip": "**Details**\n\n[Video](https://example.com/video)",
  "HelpUrl": "https://example.com/help"
}
```

The card supports headings, lists, emphasis, code and HTTP(S) hyperlinks. Images are not loaded, and HTML is displayed as text. `HelpUrl` remains separate from content links and supplies the help footer and F1 action. F1 is handled only while an enhanced card with a help address is visible. Otherwise the host retains its normal help behavior.

Version information follows `Ribbon.DisplayVersion` and `Ribbon.VersionPrefix`, including when native `ToolTip` is absent. It is passed separately from Markdown. The help caption follows the current UI culture, with English and Russian resources.

The pointer can move from a button or drop-down entry into the card. Escape, a click outside, or application deactivation closes the popup and cancels pending display. Moving into a card from a drop-down may close the menu to release mouse capture; the card stays available for links and scrolling. Native tooltip state is restored during menu rebuilding, disposal or a display failure.

The fluent API uses the optional `IEnhancedToolTipBuilder` capability, preserving the existing `IButtonBuilder` interface. Custom builders can implement this capability to support the extension method.

When packaging a .NET plugin, include runtime dependencies such as `Markdig.dll` and the `ru` resource directory. The samples set `CopyLocalLockFileAssemblies` to `true` so their installers include these files.
