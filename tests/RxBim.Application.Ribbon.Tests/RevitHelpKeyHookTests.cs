namespace RxBim.Application.Ribbon.Tests;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using FluentAssertions;
using Services;
using Xunit;
using static NativeMethods;
using static Interop.RibbonNativeMethods;

public class RevitHelpKeyHookTests
{
    private const int F1Key = 0x70;
    private const long KeyRepeated = 1L << 30;
    private const long KeyReleased = 1L << 31;

    [Fact]
    public void HandledF1AndItsRepeatsAreConsumedUntilRelease()
    {
        RunOnMessageThread(() =>
        {
            var helpCalls = 0;
            using var hook = new RevitHelpKeyHook(() => ++helpCalls == 1);

            HandleKey(hook, F1Key).Should().BeTrue();
            HandleKey(hook, F1Key, KeyRepeated).Should().BeTrue();
            HandleKey(hook, F1Key, KeyReleased).Should().BeTrue();
            HandleKey(hook, F1Key).Should().BeFalse();

            helpCalls.Should().Be(2);
        });
    }

    [Fact]
    public void CallbackReentryConsumesRepeatWithoutOpeningHelpTwice()
    {
        RunOnMessageThread(() =>
        {
            var activeHook = new StrongBox<RevitHelpKeyHook>();
            var helpCalls = 0;
            var repeatHandled = false;
            using var hook = new RevitHelpKeyHook(() =>
            {
                if (++helpCalls != 1)
                    return false;

                repeatHandled = HandleKey(activeHook.Value!, F1Key, KeyRepeated);
                return true;
            });
            activeHook.Value = hook;

            HandleKey(hook, F1Key).Should().BeTrue();

            repeatHandled.Should().BeTrue();
            helpCalls.Should().Be(1);
        });
    }

    [Fact]
    public void KeyReleaseDuringHelpCallbackDoesNotLeaveSuppressionArmed()
    {
        RunOnMessageThread(() =>
        {
            var activeHook = new StrongBox<RevitHelpKeyHook>();
            var releaseHandled = false;
            using var hook = new RevitHelpKeyHook(() =>
            {
                releaseHandled = HandleKey(activeHook.Value!, F1Key, KeyReleased);
                return true;
            });
            activeHook.Value = hook;

            HandleKey(hook, F1Key).Should().BeTrue();

            releaseHandled.Should().BeTrue();
            HandleKey(hook, F1Key, KeyReleased).Should().BeFalse();
        });
    }

    [Fact]
    public void InactiveTooltipLeavesF1AndOtherKeysUnchanged()
    {
        RunOnMessageThread(() =>
        {
            var helpCalls = 0;
            using var hook = new RevitHelpKeyHook(() => { helpCalls++; return false; });

            HandleKey(hook, F1Key).Should().BeFalse();
            HandleKey(hook, F1Key, KeyReleased).Should().BeFalse();
            HandleKey(hook, 0x41).Should().BeFalse();

            helpCalls.Should().Be(1);
        });
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)]
    public void NonActionNotificationsDoNotOpenHelp(int code)
    {
        RunOnMessageThread(() =>
        {
            var helpCalls = 0;
            using var hook = new RevitHelpKeyHook(() => { helpCalls++; return true; });

            HandleKey(hook, F1Key, code: code).Should().BeFalse();
            helpCalls.Should().Be(0);
            HandleKey(hook, F1Key).Should().BeTrue();

            helpCalls.Should().Be(1);
        });
    }

    [Fact]
    public void FreshF1IsNotSuppressedWhenBrowserReceivedPreviousKeyUp()
    {
        RunOnMessageThread(() =>
        {
            var helpCalls = 0;
            using var hook = new RevitHelpKeyHook(() => ++helpCalls == 1);

            HandleKey(hook, F1Key).Should().BeTrue();
            // Launching the browser can move focus before F1 is released.
            HandleKey(hook, F1Key).Should().BeFalse();

            helpCalls.Should().Be(2);
        });
    }

    [Fact]
    public void DisposingHookStopsHandlingF1()
    {
        RunOnMessageThread(() =>
        {
            var helpCalls = 0;
            var hook = new RevitHelpKeyHook(() => { helpCalls++; return true; });

            hook.Dispose();
            hook.Dispose();
            HandleKey(hook, F1Key).Should().BeFalse();

            helpCalls.Should().Be(0);
        });
    }

    [Fact]
    public void QueuedF1AndItsSeparateNativeHelpRequestAreBothConsumed()
    {
        RunOnMessageThread(() =>
        {
            var helpCalls = 0;
            using var hook = new RevitHelpKeyHook(() => { helpCalls++; return true; });

            ReadMessage(0x100, F1Key).message.Should().Be(0);
            ReadMessage(0x4D).message.Should().Be(0);
            ReadMessage(0x100, F1Key, KeyRepeated).message.Should().Be(0);
            ReadMessage(0x4D).message.Should().Be(0);
            ReadMessage(0x101, F1Key, KeyReleased).message.Should().Be(0);
            // Suppression is consumed by the matching request, not left active for unrelated help.
            ReadMessage(0x4D).message.Should().Be(0x4D);

            helpCalls.Should().Be(1);
        });
    }

    [Fact]
    public void NativeHelpRequestRemainsUnchangedWithoutEnhancedHelp()
    {
        RunOnMessageThread(() =>
        {
            using var hook = new RevitHelpKeyHook(() => false);

            ReadMessage(0x100, F1Key).message.Should().Be(0x100);
            ReadMessage(0x4D).message.Should().Be(0x4D);
            ReadMessage(0x101, F1Key, KeyReleased).message.Should().Be(0x101);
        });
    }

    [Fact]
    public void DisposingHookLeavesQueuedNativeHelpUnchanged()
    {
        RunOnMessageThread(() =>
        {
            var hook = new RevitHelpKeyHook(() => true);
            ReadMessage(0x100, F1Key).message.Should().Be(0);

            hook.Dispose();

            ReadMessage(0x4D).message.Should().Be(0x4D);
        });
    }

    private static System.Windows.Interop.MSG ReadMessage(int message, int key = 0, long flags = 0)
    {
        PostThreadMessage(GetCurrentThreadId(), message, key, (nint)flags).Should().BeTrue();
        PeekMessage(out var result, 0, message, message, 1).Should().BeTrue();
        return result;
    }

    private static bool HandleKey(RevitHelpKeyHook hook, int key, long flags = 0, int code = 0) =>
        hook.TryHandleKey(code, new IntPtr(key), new IntPtr(flags));

    private static void RunOnMessageThread(Action action)
    {
        Exception? failure = null;
        var thread = new Thread(() =>
        {
            try
            {
                // Install and remove the actual thread hook, but simulate key transitions without global input.
                PeekMessage(out _, IntPtr.Zero, 0, 0, 0);
                action();
            }
            catch (Exception exception)
            {
                failure = exception;
            }
        }) { IsBackground = true };
        thread.Start();
        thread.Join(TimeSpan.FromSeconds(10)).Should().BeTrue();

        if (failure is not null)
            ExceptionDispatchInfo.Capture(failure).Throw();
    }
}