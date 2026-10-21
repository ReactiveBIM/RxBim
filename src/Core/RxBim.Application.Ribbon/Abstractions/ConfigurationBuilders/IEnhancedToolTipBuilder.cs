namespace RxBim.Application.Ribbon;

/// <summary>
/// Provides optional enhanced tooltip support without extending existing button builder contracts.
/// </summary>
public interface IEnhancedToolTipBuilder
{
    /// <summary>
    /// Sets Markdown content for the interactive tooltip.
    /// </summary>
    /// <param name="markdown">Markdown content, or empty content to retain the native tooltip.</param>
    void SetEnhancedToolTip(string? markdown);
}