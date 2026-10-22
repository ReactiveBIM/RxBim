namespace RxBim.Application.Ribbon;

using System;
using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using BimTeam.Markdig.Wpf;

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
    /// Gets the marker identifying the footer link that opens the configured help address.
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
        var document = MarkdownWpf.ToFlowDocument(markdown, options: new MarkdownRenderOptions
        {
            ImageMaxWidth = ContentWidth
        });

        if (!string.IsNullOrWhiteSpace(title))
        {
            var heading = new Paragraph(new Run(title))
            {
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 8)
            };

            if (document.Blocks.FirstBlock is { } firstBlock)
                document.Blocks.InsertBefore(firstBlock, heading);
            else
                document.Blocks.Add(heading);
        }

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
}