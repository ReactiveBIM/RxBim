namespace RxBim.Application.Ribbon;

/// <summary>
/// Base configuration for item.
/// </summary>
public abstract class RibbonPanelItemBase : IRibbonPanelItem
{
    /// <summary>
    /// Item name;
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The label text.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// The URI string for item image.
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// The tooltip text.
    /// </summary>
    public string? ToolTip { get; set; }

    /// <summary>
    /// The description text.
    /// </summary>
    public string? Description { get; set; }
}