namespace RxBim.Application.Ribbon;

using System;

/// <summary>
/// Extensions for the Url.
/// </summary>
internal static class UrlExtensions
{
    /// <summary>
    /// Returns the absolute address.
    /// </summary>
    /// <param name="url">Address.</param>
    public static string GetAbsoluteUrl(this string? url)
    {
        return string.IsNullOrEmpty(url) ? string.Empty : new Uri(url).AbsoluteUri;
    }

    /// <summary>
    /// Checks whether an absolute URI uses HTTP or HTTPS.
    /// </summary>
    /// <param name="uri">The absolute URI to check.</param>
    /// <returns>Whether the URI uses HTTP or HTTPS.</returns>
    public static bool IsHttpOrHttps(this Uri uri) => uri.Scheme is "http" or "https";
}