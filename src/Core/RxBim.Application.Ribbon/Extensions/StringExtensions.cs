namespace RxBim.Application.Ribbon;

using System;

/// <summary>
/// Расширения для строк.
/// </summary>
internal static class StringExtensions
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