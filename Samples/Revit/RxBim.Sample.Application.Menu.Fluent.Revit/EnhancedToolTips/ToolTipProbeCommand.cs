namespace RxBim.Sample.Application.Menu.Fluent.Revit.EnhancedToolTips;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Command.Revit;
using Shared;

/// <summary>
/// Confirms that tooltip probes do not interfere with executing ribbon commands.
/// </summary>
[Transaction(TransactionMode.Manual)]
public class ToolTipProbeCommand : RxBimCommand
{
    /// <summary>
    /// Displays confirmation without changing the document.
    /// </summary>
    public PluginResult ExecuteCommand()
    {
        TaskDialog.Show("Tooltip probes", "The ribbon command was executed.");
        return PluginResult.Succeeded;
    }
}