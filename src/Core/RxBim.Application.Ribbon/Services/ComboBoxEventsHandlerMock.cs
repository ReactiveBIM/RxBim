namespace RxBim.Application.Ribbon;

/// <summary>
/// Mock for IComboBoxEventsHandler.
/// </summary>
public class ComboBoxEventsHandlerMock : IComboBoxEventsHandler
{
    /// <inheritdoc />
    public void HandleCurrentChanged(string tabName, string oldPanelName, string selectedPanelName)
    {
    }
}