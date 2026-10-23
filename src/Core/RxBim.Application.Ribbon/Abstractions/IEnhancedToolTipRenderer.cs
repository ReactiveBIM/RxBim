namespace RxBim.Application.Ribbon;

using System.Windows.Documents;

/// <summary>
/// Renders the body of an interactive ribbon tooltip.
/// </summary>
public interface IEnhancedToolTipRenderer
{
    /// <summary>
    /// Creates a document for the tooltip body on the current UI thread.
    /// </summary>
    /// <param name="content">The tooltip content in the format supported by this renderer.</param>
    /// <param name="maxWidth">The available content width in device-independent units.</param>
    /// <returns>A new document without the command title, help footer or version.</returns>
    FlowDocument Render(string content, double maxWidth);
}