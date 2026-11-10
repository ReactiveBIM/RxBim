namespace RxBim.Application.Ribbon;

/// <summary>
/// Provides optional enhanced tooltip support without extending existing button builder contracts.
/// </summary>
public interface IEnhancedToolTipBuilder
{
    /// <summary>
    /// Sets content for the interactive tooltip.
    /// </summary>
    /// <param name="markdown">
    /// Content formatted by the registered renderer, or empty content to retain the native tooltip.
    /// </param>
    void SetEnhancedToolTip(string? markdown);
}