namespace RxBim.Application.Ribbon.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;
using Autodesk.Windows;
using RibbonButton = Autodesk.Windows.RibbonButton;
using RibbonItem = Autodesk.Windows.RibbonItem;

/// <summary>
/// Connects enhanced tooltips to Revit ribbon buttons without changing native tooltip content.
/// </summary>
internal sealed class RevitEnhancedToolTipService : IDisposable
{
    private readonly MenuData _menuData;
    private readonly EnhancedToolTipService _toolTips;
    private readonly Dictionary<RibbonButton, bool> _nativeToolTips = [];
    private readonly HashSet<RibbonListButton> _lists = [];
    private RevitHelpKeyHook? _helpKeyHook;
    private bool _subscribed;
    private bool _failed;
    private bool _disposed;

    /// <summary>
    /// Creates the Revit adapter on the ribbon UI thread.
    /// </summary>
    /// <param name="menuData">Ribbon configuration and command assembly data.</param>
    /// <param name="renderer">The configured tooltip body renderer.</param>
    public RevitEnhancedToolTipService(MenuData menuData, IEnhancedToolTipRenderer renderer)
    {
        _menuData = menuData;
        _toolTips = new EnhancedToolTipService(() => ComponentManager.Ribbon, OpenHelp, renderer);
        _toolTips.DisplayFailed += OnDisplayFailed;
    }

    /// <summary>
    /// Registers enhanced tooltips within the button's owning panel and pull-down.
    /// </summary>
    /// <param name="config">Button configuration.</param>
    /// <param name="tab">Owning ribbon tab.</param>
    /// <param name="panelName">Owning ribbon panel name.</param>
    public void Register(Button config, RibbonTab tab, string panelName)
    {
        if (_disposed || _failed || !HasEnhancedToolTip(config))
            return;

        var panel = tab.Panels.FirstOrDefault(x => x.Source.AutomationName == panelName);

        if (panel is not null)
            Register(config, panel.Source.Items);
    }

    /// <summary>
    /// Restores native tooltips and removes registrations before rebuilding the ribbon.
    /// </summary>
    public void Clear()
    {
        _toolTips.Clear();
        RestoreNativeToolTips();
        _failed = false;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        Clear();
        _toolTips.DisplayFailed -= OnDisplayFailed;
        _toolTips.Dispose();
    }

    private static void OpenHelp(string url)
    {
        // ShellExecute can pump messages. Return from the input hook before starting the browser.
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => OpenQueuedHelp(url)));
    }

    private static void OpenQueuedHelp(string url)
    {
        try
        {
            LinkLauncher.Open(url);
        }
        catch (Exception exception)
        {
            Trace.TraceError("Could not open the Revit ribbon tooltip help: {0}", exception);
        }
    }

    private static bool HasEnhancedToolTip(Button config) =>
        !string.IsNullOrWhiteSpace(config.EnhancedToolTip) ||
        (config is PullDownButton pullDown && pullDown.CommandButtonsList.Any(HasEnhancedToolTip));

    private static IEnumerable<RibbonItem> EnumerateItems(IEnumerable<RibbonItem> items)
    {
        foreach (var item in items)
        {
            yield return item;

            if (item is RibbonRowPanel row)
            {
                foreach (var child in EnumerateItems(row.Items))
                    yield return child;
            }
        }
    }

    private void Register(Button config, IEnumerable<RibbonItem> items, RibbonListButton? parent = null)
    {
        if (!HasEnhancedToolTip(config))
            return;

        var button = EnumerateItems(items).OfType<RibbonButton>().FirstOrDefault(x =>
            x.Id?.EndsWith("%" + config.Name, StringComparison.Ordinal) == true);

        if (button is null)
            return;

        Register(button, config, parent);

        if (config is PullDownButton pullDown && button is RibbonListButton list)
        {
            foreach (var command in pullDown.CommandButtonsList)
                Register(command, list.Items, list);
        }
    }

    private void Register(RibbonButton button, Button config, RibbonListButton? parent)
    {
        if (_nativeToolTips.ContainsKey(button) ||
            !_toolTips.Register(button, config, _menuData.GetEnhancedToolTipVersion(config), parent))
        {
            return;
        }

        _nativeToolTips.Add(button, button.IsToolTipEnabled);

        if (!_subscribed)
        {
            try
            {
                _helpKeyHook = new RevitHelpKeyHook(_toolTips.TryOpenHelp);
            }
            catch (Exception exception)
            {
                Trace.TraceError("Could not install the Revit tooltip help hook: {0}", exception);
                OnDisplayFailed(this, EventArgs.Empty);
                return;
            }

            ComponentManager.ToolTipOpened += OnNativeToolTipOpened;
            _subscribed = true;
        }

        SubscribeList(button as RibbonListButton);
        SubscribeList(parent);

        button.IsToolTipEnabled = false;
    }

    private void SubscribeList(RibbonListButton? list)
    {
        if (list is null || !_lists.Add(list))
            return;

        list.DropDownOpened += OnDropDownOpened;
        list.DropDownClosed += OnDropDownClosed;
    }

    private void OnNativeToolTipOpened(object? sender, EventArgs e)
    {
        _toolTips.NotifyNativeToolTipOpened();
    }

    private void OnDropDownOpened(object? sender, EventArgs e)
    {
        if (sender is RibbonListButton list)
            _toolTips.NotifyDropDownOpened(list);
    }

    private void OnDropDownClosed(object? sender, EventArgs e)
    {
        if (sender is RibbonListButton list)
            _toolTips.NotifyDropDownClosed(list);
    }

    private void OnDisplayFailed(object? sender, EventArgs e)
    {
        Clear();
        _failed = true;
    }

    private void RestoreNativeToolTips()
    {
        _helpKeyHook?.Dispose();
        _helpKeyHook = null;

        if (_subscribed)
        {
            ComponentManager.ToolTipOpened -= OnNativeToolTipOpened;
            _subscribed = false;
        }

        foreach (var list in _lists)
        {
            list.DropDownOpened -= OnDropDownOpened;
            list.DropDownClosed -= OnDropDownClosed;
        }

        foreach (var entry in _nativeToolTips)
            entry.Key.IsToolTipEnabled = entry.Value;

        _lists.Clear();
        _nativeToolTips.Clear();
    }
}