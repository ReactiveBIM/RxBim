namespace RxBim.Application.Ribbon.Tests;

using System;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Threading;
using FluentAssertions;
using Xunit;
using static NativeMethods;
using static WpfTestThread;

[SupportedOSPlatform("windows7.0")]
public class EnhancedToolTipActivationTests
{
    private const int ActivateAppMessage = 0x1C;

    [Fact]
    public void TestThreadShutsDownItsDispatcherBeforeExiting()
    {
        Dispatcher? dispatcher = null;

        RunOnUiThread(() => dispatcher = Dispatcher.CurrentDispatcher);

        dispatcher.Should().NotBeNull();
        dispatcher!.HasShutdownFinished.Should().BeTrue();
    }

    [Fact]
    public void FailedTestAlsoShutsDownItsDispatcherBeforeExiting()
    {
        Dispatcher? dispatcher = null;
        Action test = () => RunOnUiThread(() =>
        {
            dispatcher = Dispatcher.CurrentDispatcher;
            throw new InvalidOperationException("Test failure");
        });

        test.Should().Throw<InvalidOperationException>().WithMessage("Test failure");

        dispatcher.Should().NotBeNull();
        dispatcher!.HasShutdownFinished.Should().BeTrue();
    }

    [Fact]
    public void SentDeactivationClosesTheVisiblePopup()
    {
        RunOnUiThread(() =>
        {
            using var host = CreateHostWindow();
            using var ribbon = CreateRibbonWindow(host.Handle);
            using var service = CreateService(ribbon);
            var popup = GetField<Popup>(service, "_popup");
            popup.Child = new Border();
            popup.IsOpen = true;
            popup.IsOpen.Should().BeTrue();

            SendMessage(host.Handle, ActivateAppMessage, 0, 0);

            popup.IsOpen.Should().BeFalse();
        });
    }

    [Fact]
    public void SentDeactivationCancelsPendingDisplay()
    {
        RunOnUiThread(() =>
        {
            using var host = CreateHostWindow();
            using var ribbon = CreateRibbonWindow(host.Handle);
            using var service = CreateService(ribbon);
            var timer = GetField<DispatcherTimer>(service, "_showTimer");
            timer.Start();

            SendMessage(host.Handle, ActivateAppMessage, 0, 0);

            timer.IsEnabled.Should().BeFalse();
        });
    }

    [Fact]
    public void SentActivationPreservesPendingDisplay()
    {
        RunOnUiThread(() =>
        {
            using var host = CreateHostWindow();
            using var ribbon = CreateRibbonWindow(host.Handle);
            using var service = CreateService(ribbon);
            var timer = GetField<DispatcherTimer>(service, "_showTimer");
            timer.Start();

            SendMessage(host.Handle, ActivateAppMessage, 1, 0);

            timer.IsEnabled.Should().BeTrue();
        });
    }

    [Fact]
    public void ClearDetachesTheWindowCallbackAndRegistrationAttachesItAgain()
    {
        RunOnUiThread(() =>
        {
            using var host = CreateHostWindow();
            using var ribbon = CreateRibbonWindow(host.Handle);
            using var service = CreateService(ribbon);
            var timer = GetField<DispatcherTimer>(service, "_showTimer");
            service.Clear();
            timer.Start();

            SendMessage(host.Handle, ActivateAppMessage, 0, 0);
            timer.IsEnabled.Should().BeTrue("Clear must remove the native window callback");

            service.Register(new object(), new CommandButton { EnhancedToolTip = "Details" });
            SendMessage(host.Handle, ActivateAppMessage, 0, 0);
            timer.IsEnabled.Should().BeFalse("registration must restore the window callback");
        });
    }

    [Fact]
    public void ClearingOneServicePreservesAnotherServicesWindowCallback()
    {
        RunOnUiThread(() =>
        {
            using var host = CreateHostWindow();
            using var ribbon = CreateRibbonWindow(host.Handle);
            using var first = CreateService(ribbon);
            using var second = CreateService(ribbon);
            var firstTimer = GetField<DispatcherTimer>(first, "_showTimer");
            var secondTimer = GetField<DispatcherTimer>(second, "_showTimer");
            first.Clear();
            firstTimer.Start();
            secondTimer.Start();

            SendMessage(host.Handle, ActivateAppMessage, 0, 0);

            firstTimer.IsEnabled.Should().BeTrue("the cleared service must no longer receive window messages");
            secondTimer.IsEnabled.Should().BeFalse("the other plugin must retain its window callback");
        });
    }

    [Fact]
    public void DisposeDetachesTheWindowCallback()
    {
        RunOnUiThread(() =>
        {
            using var host = CreateHostWindow();
            using var ribbon = CreateRibbonWindow(host.Handle);
            var service = CreateService(ribbon);
            var timer = GetField<DispatcherTimer>(service, "_showTimer");

            try
            {
                service.Dispose();
                timer.Start();

                SendMessage(host.Handle, ActivateAppMessage, 0, 0);

                timer.IsEnabled.Should().BeTrue("the disposed service must no longer receive window messages");
            }
            finally
            {
                timer.Stop();
                service.Dispose();
            }
        });
    }

    [Fact]
    public void DestroyingTheOwnerCancelsPendingDisplay()
    {
        RunOnUiThread(() =>
        {
            var host = CreateHostWindow();

            try
            {
                using var ribbon = CreateRibbonWindow(host.Handle);
                using var service = CreateService(ribbon);
                var timer = GetField<DispatcherTimer>(service, "_showTimer");
                timer.Start();

                host.Dispose();

                timer.IsEnabled.Should().BeFalse();
                service.Clear();
            }
            finally
            {
                host.Dispose();
            }
        });
    }

    private static HwndSource CreateHostWindow() => new(new HwndSourceParameters("Tooltip native host")
    {
        WindowStyle = unchecked((int)0x80000000),
        Width = 100,
        Height = 100
    });

    private static HwndSource CreateRibbonWindow(nint parent)
    {
        // The host's top-level HWND receives activation, while the ribbon's WPF HWND is its child.
        return new HwndSource(new HwndSourceParameters("Tooltip ribbon child")
        {
            ParentWindow = parent,
            WindowStyle = 0x40000000,
            Width = 100,
            Height = 100
        }) { RootVisual = new Border() };
    }

    private static EnhancedToolTipService CreateService(HwndSource ribbon)
    {
        var service = new EnhancedToolTipService(() => (FrameworkElement)ribbon.RootVisual, _ => { });
        service.Register(new object(), new CommandButton { EnhancedToolTip = "Details" });
        return service;
    }

    private static T GetField<T>(EnhancedToolTipService service, string name) =>
        (T)typeof(EnhancedToolTipService).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(service)!;
}