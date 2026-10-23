# Enhanced ribbon tooltips

Revit and AutoCAD menus support optional interactive tooltips. `AddRevitMenu` and `AddAutocadMenu` register a plain-text body renderer by default. The core Ribbon package has no Markdown package dependency.

For Markdown, reference the optional `RxBim.Application.Ribbon.Markdown` package and register its renderer once in the application configuration:

```csharp
using RxBim.Application.Ribbon.Markdown;

services.AddMarkdownToolTips();
services.AddRevitMenu(ribbon => /* configure the menu */);
```

AutoCAD uses the same `AddMarkdownToolTips` call with `AddAutocadMenu`. The Markdown registration may come before or after menu registration. The fluent and attribute configuration below is the same for both hosts.

```csharp
.CommandButton<MyCommand>(button => button
    .Text("My command")
    .EnhancedToolTip("**Details**\n\n[Video](https://example.com/video)")
    .HelpUrl("https://example.com/help"))
```

`EnhancedToolTip` is optional. Null, empty or whitespace content leaves native tooltip behavior unchanged. `ToolTip` and `Description` may be omitted when only the enhanced tooltip is needed. When provided, they remain the native fallback. Without a custom renderer, content is displayed literally. With `AddMarkdownToolTips`, content is interpreted as Markdown, including ordinary text. There is no format argument or special prefix.

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

The optional Markdown package uses BimTeam.Markdig.Wpf to render the body, including images and tables. Custom renderers implement `IEnhancedToolTipRenderer.Render(string content, double maxWidth)` and return a new WPF `FlowDocument` on the UI thread. Register a custom renderer as `IEnhancedToolTipRenderer` in the application service collection; the default menu registration preserves it. The shared card adds the title, help footer and version outside the body renderer. Content hyperlinks are passed to the Windows shell without filtering URI schemes in RxBim; opening failures are traced. `HelpUrl` remains separate from content links and supplies the help footer and F1 action. Both host adapters use the shared `LinkLauncher` to open enhanced tooltip help through the Windows shell. F1 is handled only while an enhanced card with a help address is visible. Otherwise the host retains its normal help behavior.

Version information follows `Ribbon.DisplayVersion` and `Ribbon.VersionPrefix`, including when native `ToolTip` is absent. It is passed separately from Markdown. The help caption follows the current UI culture, with English and Russian resources.

The pointer can move from a button or drop-down entry into the card. A click outside or application deactivation closes the popup and cancels pending display. Moving into a card from a drop-down may close the menu to release mouse capture; the card stays available for links and scrolling. Native tooltip state is restored during menu rebuilding, disposal or a display failure.

The fluent API uses the optional `IEnhancedToolTipBuilder` capability, preserving the existing `IButtonBuilder` interface. Custom builders can implement this capability to support the extension method.

When using the optional Markdown package, include `RxBim.Application.Ribbon.Markdown.*.dll` and both versioned `BimTeam.Markdig*.dll` assemblies in the plugin distribution. Also retain the Ribbon `ru` resource directory. The fluent samples reference the optional package, call `AddMarkdownToolTips`, and set `CopyLocalLockFileAssemblies` to `true` so their installers include these files.
