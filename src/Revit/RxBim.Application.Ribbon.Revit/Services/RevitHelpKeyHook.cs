namespace RxBim.Application.Ribbon.Services;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using static Interop.RibbonNativeMethods;

/// <summary>
/// Intercepts tooltip F1 requests before Revit translates them into its native help command.
/// </summary>
internal sealed class RevitHelpKeyHook : IDisposable
{
    private const int GetMessageHook = 3;
    private const int RemoveMessage = 1;
    private const int KeyDownMessage = 0x100;
    private const int KeyUpMessage = 0x101;
    private const int NativeF1HelpMessage = 0x4D;
    private const int KeyAction = 0;
    private const int F1Key = 0x70;
    private const long PreviousKeyState = 1L << 30;
    private const long KeyReleased = 1L << 31;
    private readonly Func<bool> _tryOpenHelp;
    private readonly HookProcedure _callback;
    private nint _handle;
    private bool _suppressF1;
    private bool _suppressNativeHelp;

    /// <summary>
    /// Installs a message hook for F1 and its separate native help request on the current ribbon UI thread.
    /// </summary>
    /// <param name="tryOpenHelp">Opens help and returns true only for a visible tooltip with a help address.</param>
    public RevitHelpKeyHook(Func<bool> tryOpenHelp)
    {
        _tryOpenHelp = tryOpenHelp;
        _callback = OnMessage;
        _handle = SetWindowsHookEx(GetMessageHook, _callback, 0, GetCurrentThreadId());

        if (_handle == 0)
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_handle == 0)
            return;

        if (!UnhookWindowsHookEx(_handle))
            throw new Win32Exception(Marshal.GetLastWin32Error());

        _handle = 0;
        GC.KeepAlive(_callback);
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
        {
            _suppressNativeHelp = true;
            return true;
        }

        // Arm suppression before callbacks: closing a card or opening help can reenter input processing.
        _suppressF1 = true;
        var previousNativeHelp = _suppressNativeHelp;
        _suppressNativeHelp = true;

        try
        {
            if (_tryOpenHelp())
                return true;
        }
        catch
        {
            _suppressF1 = false;
            _suppressNativeHelp = previousNativeHelp;
            throw;
        }

        _suppressF1 = false;
        _suppressNativeHelp = previousNativeHelp;
        return false;
    }

    private nint OnMessage(int code, nint wParam, nint lParam)
    {
        if (code < 0 || wParam != RemoveMessage)
            return CallNextHookEx(_handle, code, wParam, lParam);

        try
        {
            var message = Marshal.PtrToStructure<MSG>(lParam);

            if (TryHandleMessage(message))
            {
                message.message = 0;
                Marshal.StructureToPtr(message, lParam, false);
            }
        }
        catch (Exception exception)
        {
            // Exceptions must not escape a native hook callback.
            Trace.TraceError("Could not handle the Revit tooltip help message: {0}", exception);
        }

        return CallNextHookEx(_handle, code, wParam, lParam);
    }

    private bool TryHandleMessage(MSG message)
    {
        if (message.message == NativeF1HelpMessage)
        {
            // Windows queues WM_KEYF1 separately; consuming WM_KEYDOWN does not remove this request.
            var handled = _suppressNativeHelp;
            _suppressNativeHelp = false;
            return handled;
        }

        return (message.message is KeyDownMessage or KeyUpMessage) &&
               TryHandleKey(KeyAction, message.wParam, message.lParam);
    }
}