namespace RxBim.Application.Ribbon.Markdown;

using System.Windows.Documents;
using BimTeam.Markdig.Wpf;

/// <summary>
/// Renders interactive tooltip bodies with BimTeam.Markdig.Wpf.
/// </summary>
public sealed class MarkdigEnhancedToolTipRenderer : IEnhancedToolTipRenderer
{
    /// <inheritdoc />
    public FlowDocument Render(string content, double maxWidth) =>
        MarkdownWpf.ToFlowDocument(content, options: new MarkdownRenderOptions
        {
            ImageMaxWidth = maxWidth
        });
}