﻿namespace RxBim.Application.Ribbon;

/// <summary>
/// Combobox events handler.
/// </summary>
public interface IComboBoxEventsHandler
{
    /// <summary>
    /// Handles 'CurrentChanged' event.
    /// </summary>
    /// <param name="id">Combobox id.</param>
    /// <param name="oldPanelName">Previous panel.</param>
    /// <param name="selectedPanelName">Selected panel.</param>
    void HandleCurrentChanged(string id, string oldPanelName, string selectedPanelName);
}