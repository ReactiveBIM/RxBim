namespace RxBim.Application.Ribbon;

using System;

/// <summary>
/// Configures interactive ribbon tooltips.
/// </summary>
public static class EnhancedToolTipBuilderExtensions
{
    /// <summary>
    /// Sets an interactive tooltip while retaining the native tooltip as a fallback.
    /// </summary>
    /// <param name="builder">A button builder that supports enhanced tooltips.</param>
    /// <param name="markdown">
    /// Content formatted by the registered renderer. Empty content retains the native tooltip.
    /// </param>
    /// <typeparam name="TBuilder">The fluent button builder type.</typeparam>
    /// <returns>The original builder.</returns>
    public static TBuilder EnhancedToolTip<TBuilder>(this IButtonBuilder<TBuilder> builder, string? markdown)
        where TBuilder : class, IButtonBuilder<TBuilder>
    {
        if (builder is not IEnhancedToolTipBuilder enhancedBuilder)
            throw new NotSupportedException("The button builder does not support enhanced tooltips.");

        enhancedBuilder.SetEnhancedToolTip(markdown);
        return (TBuilder)builder;
    }
}