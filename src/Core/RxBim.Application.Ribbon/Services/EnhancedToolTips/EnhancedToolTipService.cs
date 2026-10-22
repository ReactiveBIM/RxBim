namespace RxBim.Application.Ribbon;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Threading;

/// <summary>
/// Displays interactive Markdown tooltips for explicitly configured ribbon buttons.
/// </summary>
public class EnhancedToolTipService : IDisposable
{
    private const int KeyDownMessage = 0x100;
    private const int ActivateAppMessage = 0x1C;
    private const int F1Key = 0x70;
    private const double PlacementGap = 8;

    private readonly Dictionary<object, Registration> _buttons = new(ReferenceComparer.Instance);
    private readonly HashSet<object> _openLists = new(ReferenceComparer.Instance);
    private readonly Popup _popup = new() { AllowsTransparency = true, StaysOpen = true };
    private readonly DispatcherTimer _showTimer = new() { Interval = TimeSpan.FromMilliseconds(400) };
    private readonly DispatcherTimer _closeTimer = new() { Interval = TimeSpan.FromMilliseconds(500) };
    private readonly InputManager _inputManager = InputManager.Current;
    private readonly Func<FrameworkElement?> _getOwner;
    private readonly Action<string> _openHelp;
    private object? _button;
    private FrameworkElement? _target;
    private FrameworkElement? _content;
    private Rect _bounds;
    private bool _beside;
    private bool _disposed;

    /// <summary>
    /// Creates the popup and its timers on the host UI thread.
    /// </summary>
    /// <param name="getOwner">Returns the ribbon element that owns the popup.</param>
    /// <param name="openHelp">Opens a button's help address.</param>
    public EnhancedToolTipService(Func<FrameworkElement?> getOwner, Action<string> openHelp)
    {
        _getOwner = getOwner ?? throw new ArgumentNullException(nameof(getOwner));
        _openHelp = openHelp ?? throw new ArgumentNullException(nameof(openHelp));
        _popup.Placement = PlacementMode.Custom;
        _popup.CustomPopupPlacementCallback = GetPlacements;
        _showTimer.Tick += OnShowTimerTick;
        _closeTimer.Tick += OnCloseTimerTick;
    }

    /// <summary>
    /// Occurs when displaying a popup fails and the host should restore native tooltips.
    /// </summary>
    public event EventHandler? DisplayFailed;

    /// <summary>
    /// Registers or updates a button only when it has nonempty enhanced tooltip content.
    /// </summary>
    /// <param name="button">The ribbon button used as the WPF control's data context.</param>
    /// <param name="configuration">The button configuration containing Markdown and help information.</param>
    /// <param name="version">Optional version text displayed verbatim in the footer.</param>
    /// <param name="parentList">The button's owning drop-down, when present.</param>
    /// <returns>Whether the button has enhanced content and was registered.</returns>
    public bool Register(object button, Button configuration, string? version = null, object? parentList = null)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(EnhancedToolTipService));

        if (button is null)
            throw new ArgumentNullException(nameof(button));

        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        if (string.IsNullOrWhiteSpace(configuration.EnhancedToolTip))
            return false;

        if (_buttons.Count == 0)
        {
            _inputManager.PostProcessInput += OnPostProcessInput;
            _inputManager.PreProcessInput += OnPreProcessInput;
            ComponentDispatcher.ThreadFilterMessage += OnThreadMessage;
        }

        _buttons[button] = new Registration(
            configuration.Text ?? configuration.Name ?? string.Empty,
            configuration.EnhancedToolTip!,
            configuration.HelpUrl,
            version,
            parentList);
        return true;
    }

    /// <summary>
    /// Records an open drop-down and closes the previous tooltip.
    /// </summary>
    /// <param name="parent">The drop-down containing registered buttons.</param>
    public void NotifyDropDownOpened(object parent)
    {
        if (_disposed)
            return;

        _openLists.Add(parent);
        Close();
    }

    /// <summary>
    /// Allows the active tooltip to remain open while the pointer leaves its drop-down.
    /// </summary>
    /// <param name="parent">The drop-down that has closed.</param>
    public void NotifyDropDownClosed(object parent)
    {
        _openLists.Remove(parent);

        if (_button is not null && ReferenceEquals(_buttons[_button].ParentList, parent))
        {
            // The popup belongs to the ribbon and outlives the menu while the pointer crosses the gap.
            ScheduleClose();
        }
    }

    /// <summary>
    /// Closes an enhanced tooltip when the host displays another button's native tooltip.
    /// </summary>
    public void NotifyNativeToolTipOpened()
    {
        if (!_popup.IsOpen)
            Close();
        else if (!IsPointerOverContent())
            ScheduleClose();
    }

    /// <summary>
    /// Opens the active enhanced tooltip's help topic and closes the popup.
    /// </summary>
    /// <returns>Whether a visible tooltip with a help address handled the request.</returns>
    public bool TryOpenHelp()
    {
        if (!_popup.IsOpen || _button is null || string.IsNullOrWhiteSpace(_buttons[_button].HelpUrl))
            return false;

        var helpUrl = _buttons[_button].HelpUrl!;
        Close();

        try
        {
            _openHelp(helpUrl);
        }
        catch (Exception exception)
        {
            Trace.TraceError("Could not open the ribbon tooltip help: {0}", exception);
        }

        return true;
    }

    /// <summary>
    /// Closes the popup and cancels pending show and close delays.
    /// </summary>
    public void Close()
    {
        _showTimer.Stop();
        _closeTimer.Stop();
        _popup.IsOpen = false;

        if (_content is not null)
        {
            _content.MouseEnter -= OnContentMouseEnter;
            _content.MouseLeave -= OnContentMouseLeave;
            _content.RemoveHandler(
                Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(OnRequestNavigate));
            _content = null;
        }

        _popup.Child = null;
        _popup.PlacementTarget = null;
        _button = null;
        _target = null;
    }

    /// <summary>
    /// Closes the popup, removes all registrations, and detaches input handlers.
    /// </summary>
    public void Clear()
    {
        Close();
        _inputManager.PostProcessInput -= OnPostProcessInput;
        _inputManager.PreProcessInput -= OnPreProcessInput;
        ComponentDispatcher.ThreadFilterMessage -= OnThreadMessage;
        _buttons.Clear();
        _openLists.Clear();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        Clear();
        _showTimer.Tick -= OnShowTimerTick;
        _closeTimer.Tick -= OnCloseTimerTick;
        _popup.CustomPopupPlacementCallback = null;
        DisplayFailed = null;
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetCursorPos(out ScreenPoint point);

    private void OnPostProcessInput(object sender, ProcessInputEventArgs e)
    {
        if (_disposed || e.StagingItem.Input is not MouseEventArgs ||
            e.StagingItem.Input is MouseButtonEventArgs || Mouse.LeftButton == MouseButtonState.Pressed ||
            Mouse.RightButton == MouseButtonState.Pressed)
            return;

        if (IsPointerOverContent())
        {
            KeepContentInteractive();
            return;
        }

        DependencyObject? source = Mouse.DirectlyOver switch
        {
            UIElement element => element,
            ContentElement element => element,
            UIElement3D element => element,
            _ => null
        };
        var (button, target) = FindTarget(source);

        if (button is null || target is null || _openLists.Contains(button))
        {
            _showTimer.Stop();
            ScheduleClose();
            return;
        }

        _closeTimer.Stop();

        if (ReferenceEquals(button, _button))
        {
            _target = target;

            if (!_popup.IsOpen && !_showTimer.IsEnabled)
                _showTimer.Start();

            return;
        }

        Close();
        _button = button;
        _target = target;
        _showTimer.Start();
    }

    private (object? Button, FrameworkElement? Target) FindTarget(DependencyObject? source)
    {
        object? button = null;
        FrameworkElement? target = null;

        while (source is not null)
        {
            if (source is FrameworkElement { DataContext: { } data } element && _buttons.ContainsKey(data))
            {
                if (button is not null && !ReferenceEquals(button, data))
                    break;

                button = data;
                target = element;

                if (element is ButtonBase or ListBoxItem)
                    break;
            }

            source = source is Visual or Visual3D
                ? VisualTreeHelper.GetParent(source) ?? LogicalTreeHelper.GetParent(source)
                : LogicalTreeHelper.GetParent(source);
        }

        return (button, target);
    }

    private void OnShowTimerTick(object? sender, EventArgs e)
    {
        _showTimer.Stop();

        try
        {
            var owner = _getOwner();

            if (_button is null || _openLists.Contains(_button) ||
                _target is null || !_target.IsVisible || owner is null ||
                PresentationSource.FromVisual(owner) is null || PresentationSource.FromVisual(_target) is null)
                return;

            var registration = _buttons[_button];
            _bounds = new Rect(owner.PointFromScreen(_target.PointToScreen(new Point(0, 0))), _target.RenderSize);
            _beside = registration.ParentList is not null;
            _content = EnhancedToolTipContent.Create(
                registration.Title, registration.Markdown, registration.HelpUrl, registration.Version);
            _content.MouseEnter += OnContentMouseEnter;
            _content.MouseLeave += OnContentMouseLeave;
            _content.AddHandler(
                Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(OnRequestNavigate));
            _popup.PlacementTarget = owner;
            _popup.Child = _content;
            _popup.IsOpen = true;
        }
        catch (Exception exception)
        {
            Close();
            Trace.TraceError("Could not display the enhanced ribbon tooltip: {0}", exception);
            DisplayFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    private CustomPopupPlacement[] GetPlacements(Size popupSize, Size targetSize, Point offset) => _beside
        ?
        [
            new(new Point(_bounds.Right + PlacementGap, _bounds.Top), PopupPrimaryAxis.Vertical),
            new(new Point(_bounds.Left - popupSize.Width - PlacementGap, _bounds.Top),
                PopupPrimaryAxis.Vertical)
        ]
        :
        [
            new(new Point(_bounds.Left, _bounds.Bottom + PlacementGap), PopupPrimaryAxis.Horizontal),
            new(new Point(_bounds.Left, _bounds.Top - popupSize.Height - PlacementGap),
                PopupPrimaryAxis.Horizontal)
        ];

    private void OnPreProcessInput(object sender, PreProcessInputEventArgs e)
    {
        if (e.StagingItem.Input is MouseButtonEventArgs { ButtonState: MouseButtonState.Pressed } &&
            !IsPointerOverContent())
        {
            Close();
            return;
        }

        if (e.StagingItem.Input is KeyEventArgs { IsDown: true, Key: Key.Escape })
        {
            Close();
            return;
        }

        if (e.StagingItem.Input is KeyEventArgs { IsDown: true, Key: Key.F1 } key && TryOpenHelp())
            key.Handled = true;
    }

    private void OnThreadMessage(ref MSG message, ref bool handled)
    {
        if (_disposed || handled)
            return;

        if (message.message == ActivateAppMessage && message.wParam == IntPtr.Zero)
        {
            Close();
            return;
        }

        if (message.message == KeyDownMessage &&
            message.wParam.ToInt32() == KeyInterop.VirtualKeyFromKey(Key.Escape))
        {
            Close();
            return;
        }

        if (message.message == KeyDownMessage && message.wParam.ToInt32() == F1Key && TryOpenHelp())
            handled = true;
    }

    private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        if (e.OriginalSource is Hyperlink link && ReferenceEquals(link.Tag, EnhancedToolTipContent.HelpLinkTag))
        {
            e.Handled = TryOpenHelp();
            return;
        }

        e.Handled = true;
        Close();

        try
        {
            LinkLauncher.Open(e.Uri.OriginalString);
        }
        catch (Exception exception)
        {
            Trace.TraceError("Could not open the ribbon tooltip link: {0}", exception);
        }
    }

    private void OnContentMouseEnter(object sender, MouseEventArgs e) => KeepContentInteractive();

    private void OnContentMouseLeave(object sender, MouseEventArgs e) => ScheduleClose();

    private void ScheduleClose()
    {
        if (_popup.IsOpen && !_closeTimer.IsEnabled)
            _closeTimer.Start();
    }

    private void OnCloseTimerTick(object? sender, EventArgs e)
    {
        _closeTimer.Stop();

        if (IsPointerOverContent())
            KeepContentInteractive();
        else
            Close();
    }

    private bool IsPointerOverContent()
    {
        if (!_popup.IsOpen || _content is null || PresentationSource.FromVisual(_content) is null ||
            !GetCursorPos(out var point))
            return false;

        // Menu capture can make IsMouseOver refer to the menu instead of this separate popup HWND.
        var position = _content.PointFromScreen(new Point(point.X, point.Y));
        return new Rect(_content.RenderSize).Contains(position);
    }

    private void KeepContentInteractive()
    {
        if (_button is not null && _buttons[_button].ParentList is { } parent &&
            _openLists.Contains(parent) && Mouse.Captured is not null)
        {
            // Release the active menu's capture only after the pointer enters its tooltip card.
            // The menu may close here; its close notification must preserve the popup during the handover.
            Mouse.Capture(null);
        }

        _closeTimer.Stop();
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ScreenPoint
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

    /// <summary>
    /// Stores the content and placement of a registered button.
    /// </summary>
    /// <param name="title">The command title.</param>
    /// <param name="markdown">The Markdown body.</param>
    /// <param name="helpUrl">The optional host help address.</param>
    /// <param name="version">The optional version footer.</param>
    /// <param name="parentList">The owning drop-down, when present.</param>
    private sealed class Registration(
        string title,
        string markdown,
        string? helpUrl,
        string? version,
        object? parentList)
    {
        /// <summary>
        /// Gets the command title.
        /// </summary>
        public string Title { get; } = title;

        /// <summary>
        /// Gets the Markdown body.
        /// </summary>
        public string Markdown { get; } = markdown;

        /// <summary>
        /// Gets the host's help address.
        /// </summary>
        public string? HelpUrl { get; } = helpUrl;

        /// <summary>
        /// Gets the optional version footer.
        /// </summary>
        public string? Version { get; } = version;

        /// <summary>
        /// Gets the owning drop-down.
        /// </summary>
        public object? ParentList { get; } = parentList;
    }

    /// <summary>
    /// Keeps registrations tied to the actual ribbon object instead of its value equality.
    /// </summary>
    private sealed class ReferenceComparer : IEqualityComparer<object>
    {
        /// <summary>
        /// The shared comparer for ribbon objects.
        /// </summary>
        public static readonly ReferenceComparer Instance = new();

        /// <inheritdoc />
        public new bool Equals(object? x, object? y) => ReferenceEquals(x, y);

        /// <inheritdoc />
        public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
    }
}