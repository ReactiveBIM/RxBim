namespace RxBim.Application.Ribbon;

using System;
using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using MarkdownBlock = Markdig.Syntax.Block;
using MarkdownInline = Markdig.Syntax.Inlines.Inline;

/// <summary>
/// Builds a scrollable Markdown card with separate help and version information.
/// </summary>
internal static class EnhancedToolTipContent
{
    private const double ContentWidth = 360;
    private const double MaxContentHeight = 520;
    private const double MaxWorkAreaHeightRatio = 0.75;

    private static readonly ResourceManager Resources = new(
        "RxBim.Application.Ribbon.Services.EnhancedToolTips.EnhancedToolTipResources",
        typeof(EnhancedToolTipContent).Assembly);

    /// <summary>
    /// Gets the marker identifying the footer link that opens host-specific help.
    /// </summary>
    internal static object HelpLinkTag { get; } = new();

    /// <summary>
    /// Creates a tooltip card whose hyperlinks raise navigation requests.
    /// </summary>
    /// <param name="title">The command title.</param>
    /// <param name="markdown">The Markdown body, which may also be ordinary text.</param>
    /// <param name="helpUrl">Optional help address opened by the host.</param>
    /// <param name="version">Optional version text rendered without parsing.</param>
    /// <returns>The interactive tooltip card.</returns>
    internal static FrameworkElement Create(string title, string markdown, string? helpUrl, string? version)
    {
        var document = new FlowDocument
        {
            PagePadding = new Thickness(0),
            FontFamily = SystemFonts.MessageFontFamily,
            FontSize = SystemFonts.MessageFontSize,
            Foreground = SystemColors.ControlTextBrush,
            TextAlignment = TextAlignment.Left
        };

        if (!string.IsNullOrWhiteSpace(title))
        {
            document.Blocks.Add(new Paragraph(new Run(title))
            {
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 8)
            });
        }

        foreach (var block in Markdown.Parse(markdown))
            document.Blocks.Add(RenderBlock(block));

        AddFooter(document, helpUrl, version);

        return new Border
        {
            Child = new FlowDocumentScrollViewer
            {
                Document = document,
                IsToolBarVisible = false,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                MaxHeight = Math.Min(MaxContentHeight, SystemParameters.WorkArea.Height * MaxWorkAreaHeightRatio),
                Width = ContentWidth
            },
            Background = SystemColors.ControlBrush,
            BorderBrush = SystemColors.ControlDarkBrush,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(2),
            Padding = new Thickness(10)
        };
    }

    private static void AddFooter(FlowDocument document, string? helpUrl, string? version)
    {
        var footer = new Paragraph
        {
            BorderBrush = SystemColors.ControlDarkBrush,
            BorderThickness = new Thickness(0, 1, 0, 0),
            Padding = new Thickness(0, 8, 0, 0),
            Margin = new Thickness(0, 8, 0, 0)
        };

        if (Uri.TryCreate(helpUrl, UriKind.Absolute, out var uri))
        {
            var label = Resources.GetString("HelpLink", CultureInfo.CurrentUICulture) ?? "Help (F1)";
            footer.Inlines.Add(new Hyperlink(new Run(label))
            {
                NavigateUri = uri,
                Foreground = SystemColors.HotTrackBrush,
                Tag = HelpLinkTag
            });
        }

        if (!string.IsNullOrWhiteSpace(version))
        {
            if (footer.Inlines.Count != 0)
                footer.Inlines.Add(new LineBreak());

            footer.Inlines.Add(new Run(version));
        }

        if (footer.Inlines.Count != 0)
            document.Blocks.Add(footer);
    }

    private static System.Windows.Documents.Block RenderBlock(MarkdownBlock block)
    {
        switch (block)
        {
            case ListBlock list:
                return RenderList(list);
            case QuoteBlock quote:
                var section = RenderSection(quote);
                section.BorderBrush = SystemColors.ControlDarkBrush;
                section.BorderThickness = new Thickness(3, 0, 0, 0);
                section.Padding = new Thickness(8, 0, 0, 0);
                return section;
            case ThematicBreakBlock:
                return new Paragraph
                {
                    BorderBrush = SystemColors.ControlDarkBrush,
                    BorderThickness = new Thickness(0, 1, 0, 0),
                    Margin = new Thickness(0, 6, 0, 6)
                };
            case CodeBlock code:
                return new Paragraph(new Run(code.Lines.ToString()))
                {
                    FontFamily = new FontFamily("Consolas"),
                    Background = SystemColors.ControlLightBrush,
                    Padding = new Thickness(6),
                    Margin = new Thickness(0, 4, 0, 8)
                };
            case LeafBlock leaf:
                return RenderParagraph(leaf);
            case ContainerBlock container:
                return RenderSection(container);
            default:
                return new Paragraph();
        }
    }

    private static Paragraph RenderParagraph(LeafBlock leaf)
    {
        var paragraph = new Paragraph { Margin = new Thickness(0, 0, 0, 8) };

        if (leaf.Inline is not null)
            RenderInlines(leaf.Inline, paragraph.Inlines);
        else
            paragraph.Inlines.Add(new Run(leaf.Lines.ToString()));

        if (leaf is HeadingBlock heading)
        {
            paragraph.FontWeight = FontWeights.Bold;
            paragraph.FontSize = SystemFonts.MessageFontSize + Math.Max(0, 4 - heading.Level);
            paragraph.Margin = new Thickness(0, 4, 0, 6);
        }

        return paragraph;
    }

    private static Section RenderSection(ContainerBlock container)
    {
        var section = new Section { Margin = new Thickness(0, 0, 0, 8) };

        foreach (var child in container)
            section.Blocks.Add(RenderBlock(child));

        return section;
    }

    private static List RenderList(ListBlock source)
    {
        var list = new List
        {
            MarkerStyle = source.IsOrdered ? TextMarkerStyle.Decimal : TextMarkerStyle.Disc,
            Padding = new Thickness(20, 0, 0, 0),
            Margin = new Thickness(0, 0, 0, 8)
        };

        if (source.IsOrdered && int.TryParse(source.OrderedStart, out var start) && start > 0)
            list.StartIndex = start;

        foreach (var child in source)
        {
            if (child is not ListItemBlock item)
                continue;

            var listItem = new ListItem { Margin = new Thickness(0, 0, 0, 3) };

            foreach (var block in item)
                listItem.Blocks.Add(RenderBlock(block));

            list.ListItems.Add(listItem);
        }

        return list;
    }

    private static void RenderInlines(ContainerInline container, InlineCollection destination)
    {
        foreach (var child in container)
            destination.Add(RenderInline(child));
    }

    private static System.Windows.Documents.Inline RenderInline(MarkdownInline inline)
    {
        switch (inline)
        {
            case LiteralInline literal:
                return new Run(literal.Content.ToString());
            case CodeInline code:
                return new Run(code.Content) { FontFamily = new FontFamily("Consolas") };
            case LineBreakInline lineBreak:
                return lineBreak.IsHard ? new LineBreak() : new Run(" ");
            case EmphasisInline emphasis:
                Span formatted = emphasis.DelimiterCount >= 2 ? new Bold() : new Italic();
                RenderInlines(emphasis, formatted.Inlines);
                return formatted;
            case LinkInline link:
                return RenderLink(link);
            case ContainerInline nested:
                var span = new Span();
                RenderInlines(nested, span.Inlines);
                return span;
            case HtmlInline html:
                return new Run(html.Tag);
            default:
                return new Run();
        }
    }

    private static Span RenderLink(LinkInline link)
    {
        var label = new Span();

        if (!link.IsImage && Uri.TryCreate(link.Url, UriKind.Absolute, out var uri) &&
            uri.IsHttpOrHttps())
        {
            label = new Hyperlink
            {
                NavigateUri = uri,
                Foreground = SystemColors.HotTrackBrush
            };
        }

        RenderInlines(link, label.Inlines);
        return label;
    }
}