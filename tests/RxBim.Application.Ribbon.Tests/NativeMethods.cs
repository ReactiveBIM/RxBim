namespace RxBim.Application.Ribbon.Tests;

using System.Runtime.InteropServices;
using System.Windows.Interop;

internal static class NativeMethods
{
    [DllImport("user32.dll", EntryPoint = "PostThreadMessageW", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool PostThreadMessage(uint threadId, int message, nint wParam, nint lParam);

    [DllImport("user32.dll", EntryPoint = "PeekMessageW")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool PeekMessage(out MSG message, nint window, int min, int max, int remove);
}