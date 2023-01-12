namespace RxBim.Application.Civil;

/// <summary>
/// Data for Civil 3D not supported event.
/// </summary>
public class CivilNotSupportedEventHandlerArgs
{
    /// <summary>
    /// Error message displayed to the user.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Show a message to the user.
    /// </summary>
    public bool ShowMessage { get; set; } = true;
}