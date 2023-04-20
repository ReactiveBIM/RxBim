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
}