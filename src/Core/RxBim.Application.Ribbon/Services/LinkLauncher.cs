namespace RxBim.Application.Ribbon;

using System.Diagnostics;

/// <summary>
/// Opens ribbon links with the application associated with their address in Windows.
/// </summary>
public static class LinkLauncher
{
    /// <summary>
    /// Opens an address through the Windows shell.
    /// </summary>
    /// <param name="address">The URL or file address to open.</param>
    public static void Open(string address)
    {
        using var process = Process.Start(new ProcessStartInfo(address) { UseShellExecute = true });
    }
}