namespace RxBim.Application.Ribbon;

/// <summary>
/// Combobox events handler.
/// </summary>
public interface IComboBoxEventsHandler
{
    /// <summary>
    /// Handles 'CurrentChanged' event.
    /// </summary>
    /// <param name="tabName">Tab name.</param>
    /// <param name="oldPanelName">Previous panel.</param>
    /// <param name="selectedPanelName">Selected panel.</param>
    void HandleCurrentChanged(string tabName, string oldPanelName, string selectedPanelName);
}