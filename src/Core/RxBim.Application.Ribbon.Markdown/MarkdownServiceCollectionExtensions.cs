namespace RxBim.Application.Ribbon.Markdown;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Registers Markdown rendering for interactive ribbon tooltips.
/// </summary>
public static class MarkdownServiceCollectionExtensions
{
    /// <summary>
    /// Replaces the tooltip body renderer with BimTeam.Markdig.Wpf Markdown rendering.
    /// </summary>
    /// <param name="services">The application service collection.</param>
    /// <returns>The service collection for further configuration.</returns>
    public static IServiceCollection AddMarkdownToolTips(this IServiceCollection services) =>
        services.Replace(ServiceDescriptor.Singleton<IEnhancedToolTipRenderer, MarkdigEnhancedToolTipRenderer>());
}