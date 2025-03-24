namespace RxBim.Application.Ribbon;

using JetBrains.Annotations;

/// <summary>
/// Combobox events handler.
/// </summary>
[PublicAPI]
public interface IComboBoxEventsHandler
{
    /// <summary>
    /// Handles 'CurrentChanged' event.
    /// </summary>
    /// <param name="name">Combobox internal name.</param>
    /// <param name="oldValue">Old value.</param>
    /// <param name="newValue">New value.</param>
    void HandleCurrentChanged(string name, string oldValue, string newValue);
}