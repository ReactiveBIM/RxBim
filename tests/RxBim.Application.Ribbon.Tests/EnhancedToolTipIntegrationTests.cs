namespace RxBim.Application.Ribbon.Tests;

using System;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Input;
using System.Windows.Threading;
using FluentAssertions;
using Xunit;
using static NativeMethods;
using static WpfTestThread;

/// <summary>
/// Checks native popup behavior on an interactive Windows desktop, outside the unit-test CI filter.
/// </summary>
[Collection(nameof(WpfTestThread))]
[SupportedOSPlatform("windows7.0")]
public class EnhancedToolTipIntegrationTests
{
    private const int ActivateAppMessage = 0x1C;

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

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ClickingACommandPreventsItsTooltipFromReopeningUntilThePointerLeaves(bool visible)
    {
        RunOnUiThread(() =>
        {
            var command = new object();
            var button = new Button { DataContext = command };
            using var host = CreateHostWindow();
            host.RootVisual = button;
            using var service = new EnhancedToolTipService(() => button, _ => { });
            service.Register(command, new CommandButton { EnhancedToolTip = "Details" });
            var timer = GetField<DispatcherTimer>(service, "_showTimer");
            var popup = GetField<Popup>(service, "_popup");
            UpdateTarget(service, button);
            timer.IsEnabled.Should().BeTrue();

            if (visible)
            {
                popup.Child = new Border();
                popup.IsOpen = true;
            }

            InputManager.Current.ProcessInput(new MouseButtonEventArgs(
                Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left)
            {
                RoutedEvent = Mouse.PreviewMouseDownEvent,
                Source = button
            });
            UpdateTarget(service, button);

            popup.IsOpen.Should().BeFalse("clicking the command must dismiss its visible tooltip");
            timer.IsEnabled.Should().BeFalse("later mouse input must not reopen the clicked command's tooltip");

            UpdateTarget(service, null);
            UpdateTarget(service, button);

            timer.IsEnabled.Should().BeTrue("leaving and hovering the command again must restore its tooltip");
        });
    }

    private static void UpdateTarget(EnhancedToolTipService service, DependencyObject? source) =>
        typeof(EnhancedToolTipService).GetMethod("UpdateTarget", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(service, [source]);

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