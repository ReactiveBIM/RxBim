namespace RxBim.Application.Ribbon.Services;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

/// <summary>
/// Intercepts tooltip F1 requests before Revit translates them into its native help command.
/// </summary>
internal sealed class RevitHelpKeyHook : IDisposable
{
    private const int KeyboardHook = 2;
    private const int KeyAction = 0;
    private const int F1Key = 0x70;
    private const long PreviousKeyState = 1L << 30;
    private const long KeyReleased = 1L << 31;
    private readonly Func<bool> _tryOpenHelp;
    private readonly HookProcedure _callback;
    private nint _handle;
    private bool _suppressF1;

    /// <summary>
    /// Installs a keyboard hook for the current ribbon UI thread only.
    /// </summary>
    /// <param name="tryOpenHelp">Opens help and returns true only for a visible tooltip with a help address.</param>
    public RevitHelpKeyHook(Func<bool> tryOpenHelp)
    {
        _tryOpenHelp = tryOpenHelp;
        _callback = OnKey;
        _handle = SetWindowsHookEx(KeyboardHook, _callback, 0, GetCurrentThreadId());

        if (_handle == 0)
            throw new Win32Exception(Marshal.GetLastWin32Error());

        TraceHelp($"Keyboard hook installed: {_handle}");
    }

    private delegate nint HookProcedure(int code, nint wParam, nint lParam);

    /// <inheritdoc />
    public void Dispose()
    {
        if (_handle == 0)
            return;

        if (!UnhookWindowsHookEx(_handle))
            throw new Win32Exception(Marshal.GetLastWin32Error());

        TraceHelp($"Keyboard hook removed: {_handle}");
        _handle = 0;
        GC.KeepAlive(_callback);
    }

    /// <summary>
    /// Records Debug-only help routing diagnostics without affecting native callbacks on an I/O failure.
    /// </summary>
    /// <param name="message">The routing event to record.</param>
    [Conditional("DEBUG")]
    internal static void TraceHelp(string message)
    {
        try
        {
            using var process = Process.GetCurrentProcess();
            var path = Path.Combine(Path.GetTempPath(), "RxBim.Revit.ToolTip.F1.log");
            var entry = $"{DateTime.UtcNow:O} {process.ProcessName}/{process.Id} T{GetCurrentThreadId()} {message}";
            File.AppendAllText(path, entry + Environment.NewLine);
        }
        catch (Exception exception)
        {
            Trace.TraceError("Could not write tooltip help diagnostics: {0}", exception);
        }
    }

    /// <summary>
    /// Handles an actual F1 transition while preserving previews and unrelated keys.
    /// </summary>
    /// <param name="code">The keyboard hook notification code.</param>
    /// <param name="key">The virtual key code.</param>
    /// <param name="keyFlags">The Win32 keyboard transition flags.</param>
    /// <returns>Whether the key must be consumed before reaching the host.</returns>
    internal bool TryHandleKey(int code, nint key, nint keyFlags)
    {
        if (_handle == 0 || code != KeyAction || key != F1Key)
            return false;

        var flags = (long)keyFlags;

        if ((flags & KeyReleased) != 0)
        {
            var handled = _suppressF1;
            _suppressF1 = false;
            return handled;
        }

        // The browser can receive the previous key-up after it takes focus. A fresh press resets that state.
        if ((flags & PreviousKeyState) == 0)
            _suppressF1 = false;

        if (_suppressF1)
            return true;

        // Arm suppression before callbacks: closing a card or opening help can reenter input processing.
        _suppressF1 = true;

        try
        {
            if (_tryOpenHelp())
                return true;
        }
        catch
        {
            _suppressF1 = false;
            throw;
        }

        _suppressF1 = false;
        return false;
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowsHookExW", SetLastError = true)]
    private static extern nint SetWindowsHookEx(int idHook, HookProcedure callback, nint module, uint threadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(nint hook);

    [DllImport("user32.dll")]
    private static extern nint CallNextHookEx(nint hook, int code, nint wParam, nint lParam);

    [DllImport("kernel32.dll")]
    private static extern uint GetCurrentThreadId();

    private nint OnKey(int code, nint wParam, nint lParam)
    {
        TraceKeyNotification("enter", code, wParam, lParam);

        try
        {
            // Returning nonzero consumes the key before Windows or another hook can launch native help.
            if (TryHandleKey(code, wParam, lParam))
            {
                TraceKeyNotification("consumed", code, wParam, lParam);
                return 1;
            }
        }
        catch (Exception exception)
        {
            // Exceptions must not escape a native hook callback.
            Trace.TraceError("Could not handle the Revit tooltip help key: {0}", exception);
        }

        TraceKeyNotification("passed", code, wParam, lParam);
        return CallNextHookEx(_handle, code, wParam, lParam);
    }

    [Conditional("DEBUG")]
    private void TraceKeyNotification(string stage, int code, nint key, nint flags)
    {
        if (key == F1Key)
            TraceHelp($"F1 {stage}: code={code}, flags=0x{(long)flags:X}, suppressed={_suppressF1}");
    }
}