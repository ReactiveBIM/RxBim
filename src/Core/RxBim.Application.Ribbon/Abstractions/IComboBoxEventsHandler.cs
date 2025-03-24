namespace RxBim.Application.Ribbon;

/// <summary>
/// Combobox events handler.
/// </summary>
public interface IComboBoxEventsHandler
{
    /// <summary>
    /// Handles 'CurrentChanged' event.
    /// </summary>
    /// <param name="id">Combobox id.</param>
    /// <param name="oldValue">Old value.</param>
    /// <param name="newValue">New value.</param>
    void HandleCurrentChanged(string id, string oldValue, string newValue);
}