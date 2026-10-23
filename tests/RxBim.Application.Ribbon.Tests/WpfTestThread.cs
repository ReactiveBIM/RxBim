namespace RxBim.Application.Ribbon.Tests;

using System;
using System.Runtime.ExceptionServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Threading;
using FluentAssertions;

/// <summary>
/// Runs WPF checks on an STA thread and shuts down its dispatcher before the thread exits.
/// </summary>
[SupportedOSPlatform("windows7.0")]
internal static class WpfTestThread
{
    internal static void RunOnUiThread(Action action)
    {
        Exception? failure = null;
        var thread = new Thread(() =>
        {
            Dispatcher? dispatcher = null;

            try
            {
                dispatcher = Dispatcher.CurrentDispatcher;
                action();
            }
            catch (Exception exception)
            {
                failure = exception;
            }
            finally
            {
                try
                {
                    // Destroy dispatcher-owned HWNDs while the managed thread can still receive native callbacks.
                    dispatcher?.InvokeShutdown();
                }
                catch (Exception exception)
                {
                    failure = failure is null ? exception : new AggregateException(failure, exception);
                }
            }
        }) { IsBackground = true };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join(TimeSpan.FromSeconds(10)).Should().BeTrue("the WPF check and dispatcher cleanup must complete");

        if (failure is not null)
            ExceptionDispatchInfo.Capture(failure).Throw();
    }
}