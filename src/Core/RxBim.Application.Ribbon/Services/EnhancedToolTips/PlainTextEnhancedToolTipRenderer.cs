namespace RxBim.Application.Ribbon;

using System.Windows.Documents;

/// <summary>
/// Displays tooltip content literally without interpreting markup.
/// </summary>
internal sealed class PlainTextEnhancedToolTipRenderer : IEnhancedToolTipRenderer
{
    /// <inheritdoc />
    public FlowDocument Render(string content, double maxWidth) => new(new Paragraph(new Run(content)));
}