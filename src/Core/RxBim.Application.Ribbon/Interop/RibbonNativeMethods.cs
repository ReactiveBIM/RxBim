namespace RxBim.Application.Ribbon.Interop;

using System.Runtime.InteropServices;

/// <summary>
/// Provides Win32 calls shared by the ribbon services and host adapters.
/// </summary>
public static class RibbonNativeMethods
{
    /// <summary>
    /// Processes a native thread hook notification.
    /// </summary>
    /// <param name="code">The hook notification code.</param>
    /// <param name="wParam">The notification-specific value.</param>
    /// <param name="lParam">The notification-specific value or pointer.</param>
    /// <returns>The result passed back to Windows.</returns>
    public delegate nint HookProcedure(int code, nint wParam, nint lParam);

    /// <summary>
    /// Processes a message delivered to a subclassed host window.
    /// </summary>
    /// <param name="window">The receiving window handle.</param>
    /// <param name="message">The native message identifier.</param>
    /// <param name="wParam">The first message parameter.</param>
    /// <param name="lParam">The second message parameter.</param>
    /// <param name="id">The subclass identifier.</param>
    /// <param name="referenceData">The reference data supplied during registration.</param>
    /// <returns>The result passed back to Windows.</returns>
    internal delegate nint SubclassProcedure(
        nint window,
        uint message,
        nint wParam,
        nint lParam,
        nuint id,
        nuint referenceData);

    /// <summary>
    /// Gets the cursor position in physical screen pixels.
    /// </summary>
    /// <param name="point">The cursor coordinates.</param>
    /// <returns>Whether the cursor position was retrieved.</returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(out ScreenPoint point);

    /// <summary>
    /// Installs a native hook for the specified thread.
    /// </summary>
    /// <param name="idHook">The hook type.</param>
    /// <param name="callback">The callback that must remain alive until the hook is removed.</param>
    /// <param name="module">The callback module, or zero for a hook in the current process.</param>
    /// <param name="threadId">The target thread identifier.</param>
    /// <returns>The hook handle, or zero on failure.</returns>
    [DllImport("user32.dll", EntryPoint = "SetWindowsHookExW", SetLastError = true)]
    public static extern nint SetWindowsHookEx(int idHook, HookProcedure callback, nint module, uint threadId);

    /// <summary>
    /// Removes a previously installed native hook.
    /// </summary>
    /// <param name="hook">The hook handle.</param>
    /// <returns>Whether the hook was removed.</returns>
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(nint hook);

    /// <summary>
    /// Passes a notification to the next hook in the chain.
    /// </summary>
    /// <param name="hook">The current hook handle.</param>
    /// <param name="code">The hook notification code.</param>
    /// <param name="wParam">The notification-specific value.</param>
    /// <param name="lParam">The notification-specific value or pointer.</param>
    /// <returns>The result returned by the next hook.</returns>
    [DllImport("user32.dll")]
    public static extern nint CallNextHookEx(nint hook, int code, nint wParam, nint lParam);

    /// <summary>
    /// Gets the identifier of the calling native thread.
    /// </summary>
    /// <returns>The native thread identifier.</returns>
    [DllImport("kernel32.dll")]
    public static extern uint GetCurrentThreadId();

    /// <summary>
    /// Gets the ancestor window selected by the specified Win32 flag.
    /// </summary>
    /// <param name="window">The starting window handle.</param>
    /// <param name="flags">The ancestor selection flag.</param>
    /// <returns>The ancestor handle, or zero when unavailable.</returns>
    [DllImport("user32.dll")]
    internal static extern nint GetAncestor(nint window, uint flags);

    /// <summary>
    /// Adds a window procedure callback on the window's owning thread.
    /// </summary>
    /// <param name="window">The window to observe.</param>
    /// <param name="callback">The callback retained until it is removed.</param>
    /// <param name="id">The identifier paired with the callback.</param>
    /// <param name="referenceData">The reference data passed to the callback.</param>
    /// <returns>Whether the callback was registered.</returns>
    [DllImport("comctl32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetWindowSubclass(
        nint window,
        SubclassProcedure callback,
        nuint id,
        nuint referenceData);

    /// <summary>
    /// Removes a window procedure callback on the window's owning thread.
    /// </summary>
    /// <param name="window">The observed window.</param>
    /// <param name="callback">The registered callback.</param>
    /// <param name="id">The identifier paired with the callback.</param>
    /// <returns>Whether the callback was removed.</returns>
    [DllImport("comctl32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool RemoveWindowSubclass(nint window, SubclassProcedure callback, nuint id);

    /// <summary>
    /// Passes the message to the remaining window procedure chain.
    /// </summary>
    /// <param name="window">The receiving window.</param>
    /// <param name="message">The native message identifier.</param>
    /// <param name="wParam">The first message parameter.</param>
    /// <param name="lParam">The second message parameter.</param>
    /// <returns>The result of the remaining window procedures.</returns>
    [DllImport("comctl32.dll")]
    internal static extern nint DefSubclassProc(nint window, uint message, nint wParam, nint lParam);

    /// <summary>
    /// Represents a Win32 screen position in physical pixels.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ScreenPoint
    {
        /// <summary>
        /// Horizontal screen coordinate in physical pixels.
        /// </summary>
        public int X;

        /// <summary>
        /// Vertical screen coordinate in physical pixels.
        /// </summary>
        public int Y;
    }
}