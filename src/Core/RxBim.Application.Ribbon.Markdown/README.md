# Markdown ribbon tooltips

This optional package provides `IEnhancedToolTipRenderer` using BimTeam.Markdig.Wpf, including images, tables and interactive links. It supports .NET Framework 4.7.2 and .NET 8 for Windows. The base `RxBim.Application.Ribbon` package remains independent of Markdown libraries and displays enhanced content as plain text by default.

Reference `RxBim.Application.Ribbon.Markdown` in the plugin and register the renderer in its application configuration:

```csharp
using RxBim.Application.Ribbon;
using RxBim.Application.Ribbon.Markdown;

services.AddMarkdownToolTips();
services.AddRevitMenu(ribbon => ribbon
    .Tab("My tools", tab => tab
        .Panel("Commands", panel => panel
            .CommandButton<MyCommand>("MyCommand", button => button
                .Text("My command")
                .EnhancedToolTip("**Details**\n\n[Help](https://example.com/help)")))));
```

For AutoCAD, use the same renderer registration with `AddAutocadMenu`. `AddMarkdownToolTips` works before or after menu registration and replaces the previously registered body renderer.

Only the body is rendered as Markdown. Ribbon still owns the popup, placement, mouse handling, help links, F1, title and version footer. Buttons without `EnhancedToolTip` retain the host's native tooltips. When only the base Ribbon package is used, enhanced tooltip bodies remain literal text.

Include the optional package assembly and its versioned BimTeam.Markdig dependencies in the plugin distribution. The Revit and AutoCAD fluent samples demonstrate registration, embedded images and tables.