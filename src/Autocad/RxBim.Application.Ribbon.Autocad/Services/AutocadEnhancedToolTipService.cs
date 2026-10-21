namespace RxBim.Application.Ribbon.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Threading;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

/// <inheritdoc />
internal sealed class AutocadEnhancedToolTipService : IAutocadEnhancedToolTipService
{
    private const int KeyDownMessage = 0x100;
    private const int AutoCadHelpMessage = 0x4D;
    private const int F1Key = 0x70;
    private readonly MenuData _menuData;
    private readonly EnhancedToolTipService _toolTips;
    private readonly Dictionary<RibbonButton, bool> _nativeToolTipStates = [];
    private readonly HashSet<RibbonListButton> _listButtons = [];
    private bool _subscribed;
    private bool _suppressNextHelp;
    private bool _disposed;

    /// <summary>
    /// Initializes the AutoCAD adapter for the shared interactive tooltip service.
    /// </summary>
    /// <param name="menuData">The menu metadata used to display command versions.</param>
    public AutocadEnhancedToolTipService(MenuData menuData)
    {
        _menuData = menuData;
        _toolTips = new EnhancedToolTipService(
            () => ComponentManager.Ribbon,
            OpenHelp);
        _toolTips.DisplayFailed += OnDisplayFailed;
    }

    /// <inheritdoc />
    public void Register(RibbonButton button, Button config, RibbonListButton? parentList = null)
    {
        if (_disposed || string.IsNullOrWhiteSpace(config.EnhancedToolTip))
            return;

        if (!_toolTips.Register(button, config, _menuData.GetEnhancedToolTipVersion(config), parentList))
            return;

        if (!_nativeToolTipStates.ContainsKey(button))
            _nativeToolTipStates.Add(button, button.IsToolTipEnabled);

        button.IsToolTipEnabled = false;
        SubscribeList(button as RibbonListButton);
        SubscribeList(parentList);

        if (_subscribed)
            return;

        ComponentManager.ToolTipOpened += OnNativeToolTipOpened;
        Application.PreTranslateMessage += OnApplicationMessage;
        _subscribed = true;
    }

    /// <inheritdoc />
    public void Clear()
    {
        _toolTips.Clear();

        foreach (var entry in _nativeToolTipStates)
            entry.Key.IsToolTipEnabled = entry.Value;

        foreach (var listButton in _listButtons)
        {
            listButton.DropDownOpened -= OnDropDownOpened;
            listButton.DropDownClosed -= OnDropDownClosed;
        }

        _nativeToolTipStates.Clear();
        _listButtons.Clear();

        if (_subscribed)
        {
            ComponentManager.ToolTipOpened -= OnNativeToolTipOpened;
            Application.PreTranslateMessage -= OnApplicationMessage;
            _subscribed = false;
        }

        _suppressNextHelp = false;
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

    private void SubscribeList(RibbonListButton? listButton)
    {
        if (listButton is null || !_listButtons.Add(listButton))
            return;

        listButton.DropDownOpened += OnDropDownOpened;
        listButton.DropDownClosed += OnDropDownClosed;
    }

    private void OnDropDownOpened(object? sender, EventArgs e)
    {
        if (sender is RibbonListButton listButton)
            _toolTips.NotifyDropDownOpened(listButton);
    }

    private void OnDropDownClosed(object? sender, EventArgs e)
    {
        if (sender is RibbonListButton listButton)
            _toolTips.NotifyDropDownClosed(listButton);
    }

    private void OnNativeToolTipOpened(object? sender, EventArgs e) => _toolTips.NotifyNativeToolTipOpened();

    private void OnDisplayFailed(object? sender, EventArgs e) => Clear();

    private void OnApplicationMessage(object? sender, PreTranslateMessageEventArgs e)
    {
        if (e.Handled)
            return;

        if (e.Message.message == AutoCadHelpMessage && _suppressNextHelp)
        {
            _suppressNextHelp = false;
            e.Handled = true;
            return;
        }

        if (e.Message.message != KeyDownMessage || (int)e.Message.wParam != F1Key || !_toolTips.TryOpenHelp())
            return;

        e.Handled = true;
    }

    private void OpenHelp(string url)
    {
        _suppressNextHelp = true;

        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        finally
        {
            // F1 can reach the shared WPF handler before AutoCAD's message handler.
            ComponentManager.Ribbon?.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                _suppressNextHelp = false;
            }));
        }
    }
}