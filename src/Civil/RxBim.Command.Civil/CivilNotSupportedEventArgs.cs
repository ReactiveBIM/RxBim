namespace RxBim.Shared.Civil;

/// <summary>
/// Data for Civil 3D not supported event.
/// </summary>
public class CivilNotSupportedEventArgs
{
    /// <summary>
    /// The message to display.
    /// </summary>
    /// <remarks>Displayed if <see cref="ShowMessage"/> is true.</remarks>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// If true, displays a message on the screen. If false, no message is displayed.
    /// </summary>
    /// <remarks>The message text is contained in <see cref="Message"/>.</remarks>
    public bool ShowMessage { get; set; } = true;

    /// <summary>
    /// If true, the plugin stops. If false, plugin execution continues.
    /// </summary>
    public bool StopExecution { get; set; } = true;
}